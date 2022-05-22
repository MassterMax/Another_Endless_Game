using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.EventSystems;

// Author: https://github.com/Oponn-1/Unity-Gesture-Recognizer/blob/master/GestureRecognizer.cs
public class GestureRecognizer : MonoBehaviour
{

    public bool recording = true;
    public bool anomaliesTesting = false;
    public string templateSaveName;
    public int pointsPerGesture = 30;
    public float samplingRate = 0.01f;
    public bool limitSamples = false;
    public int maxPointsAllowed = 100;
    public float standardRatio = 100f;
    public float devTightness = 1f;
    public float anomaliesFactor = 5f;

    private bool gestureStarted;
    private bool gestureComplete;
    private bool inputReady;

    private string gestureFileName = "gestures.json";
    private TwoDPoint startPoint;
    private TwoDPoint currentPoint;
    private DrawnGesture currentGesture;
    private List<TwoDPoint> currentPointList;
    private TwoDPoint[] reducedPoints;
    private GestureTemplates templates;
    private float tempTime = 0f;


    SpellManager spellManager;
    Drawing drawing;

    private void Awake()
    {
        spellManager = FindObjectOfType<SpellManager>();
        drawing = FindObjectOfType<Drawing>();
    }

    void Start()
    {
        LoadTemplates();
        varInitialization();
    }

    #region variable initialization and reset
    private void varInitialization()
    {
        currentPoint = new TwoDPoint(0, 0);
        startPoint = new TwoDPoint(0, 0);
        currentPointList = new List<TwoDPoint>();
        currentPointList.Add(new TwoDPoint(0, 0));
        reducedPoints = new TwoDPoint[pointsPerGesture];
        for (int i = 0; i < pointsPerGesture; i++)
        {
            reducedPoints[i] = new TwoDPoint(0, 0);
        }
        gestureStarted = false;
        gestureComplete = false;
        inputReady = false;
        currentGesture = new DrawnGesture("currentGesture", pointsPerGesture);
    }


    private void varReset()
    {
        for (int i = 0; i < pointsPerGesture; i++)
        {
            reducedPoints[i].SetX(0);
            reducedPoints[i].SetY(0);
        }
        currentPointList.Clear();
        currentPointList.Add(new TwoDPoint(0, 0));
        gestureStarted = false;
        gestureComplete = false;
    }

    #endregion

    void Update()
    {
        tempTime += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (inputReady)
            {
                if (!gestureStarted)
                {
                    // verify pointer is not on top of GUI; if it is, return
                    // https://forum.unity.com/threads/prevent-mouse-clicking-through-ui.1027765/
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        //Debug.LogWarning("Do not start gesture!");
                        return;
                    }

                    gestureStarted = true;
                    StartGesture();
                }
                if ((!gestureComplete) && (tempTime > samplingRate))
                {
                    tempTime = 0f;
                    ContinueGesture();
                }
                if (gestureComplete)
                {
                    EndGesture();
                }
            }
        }
        else
        {
            if (gestureStarted)
            {
                EndGesture();
            }
            inputReady = true;
        }

        if (recording && Input.GetKeyDown(KeyCode.J))
        {
            SaveTemplates();
            Debug.LogWarning("saved!");
        }
    }


    //******************************************
    //      Save and Load Gestures
    //
    //      SaveTemplates
    //      use:                writes templates to json file
    //      LoadTemplates
    //      use:                called on start to read json templates
    //                          object from file if it's there
    private void SaveTemplates()
    {
        string filePath = Application.dataPath + "/StreamingAssets/" + gestureFileName;
        string saveData = JsonUtility.ToJson(templates);
        //Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(templates));
        Debug.Log(JsonUtility.ToJson(templates));
        File.WriteAllText(filePath, saveData);
    }

    private void LoadTemplates()
    {
        templates = new GestureTemplates();
        string filePath = Path.Combine(Application.streamingAssetsPath, gestureFileName);
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            templates = JsonUtility.FromJson<GestureTemplates>(data);
        }

        if (templates == null) templates = new GestureTemplates();
    }


    //***************************************
    //      StartGesture
    //
    //      use:            Set up recording of gesture by
    //                      setting the start point and control bool.
    //                      Called when player first clicks.
    private void StartGesture()
    {
        Debug.Log("gesture started");
        startPoint.SetX(Input.mousePosition.x);
        startPoint.SetY(Input.mousePosition.y);
        gestureComplete = false;
    }


    //***************************************
    //      ContinueGesture
    //
    //      use:            Update min and max x and y values for
    //                      the current gesture being recorded
    //                      and add the new point to the list.
    //                      Called while player holds input down.
    private void ContinueGesture()
    {
        currentPoint.SetX(Input.mousePosition.x - startPoint.GetX());
        currentPoint.SetY(Input.mousePosition.y - startPoint.GetY());
        currentPointList.Add(new TwoDPoint(currentPoint.GetX(), currentPoint.GetY()));
        if (currentPoint.GetX() > currentGesture.GetMaxX())
        {
            currentGesture.SetMaxX(currentPoint.GetX());
        }
        if (currentPoint.GetX() < currentGesture.GetMinX())
        {
            currentGesture.SetMinX(currentPoint.GetX());
        }
        if (currentPoint.GetY() > currentGesture.GetMaxY())
        {
            currentGesture.SetMaxY(currentPoint.GetY());
        }
        if (currentPoint.GetY() < currentGesture.GetMinY())
        {
            currentGesture.SetMinY(currentPoint.GetY());
        }
        if (limitSamples && currentPointList.Count >= maxPointsAllowed)
        {
            gestureComplete = true;
            Debug.Log(message: "Gesture Complete!");
        }
    }


    //***************************************
    //      EndGesture
    //
    //      use:            Resets control bools and other variables
    //                      records gesture to the templates object
    //                      or calls recognition.
    //                      Called when max recording points reached.
    private void EndGesture()
    {
        if (inputReady) inputReady = false;
        gestureStarted = false;
        gestureComplete = true;

        // Debug.Log(currentGesture.GetPoints().Length + " before scaling " + currentPointList.Count);

        Rescale(currentGesture);
        MapPoints(currentGesture);
        if (recording)
        {
            currentGesture.SetName(templateSaveName);
            var gesture = new DrawnGesture(currentGesture.GetName(), pointsPerGesture, currentGesture.GetMaxX(), currentGesture.GetMaxY(),
                currentGesture.GetMinX(), currentGesture.GetMinY(), currentGesture.GetPoints());

            // main:
            templates.templates.Add(gesture);

            // experimental:
            /*
            foreach(var el in gesture.NormalizedGestures())
            {
                templates.templates.Add(el);
            }*/
        }
        else
        {
            // Debug.Log(currentGesture.GetPoints().Length + " after rescaling " + currentPointList.Count);
            //DrawnGesture m = FindMatch(currentGesture, templates);
            var result = FindMatchAndDifference(currentGesture, templates);
            // Debug.Log("best match: " + result.Key.GetName() + " difference: " + result.Value.ToString());


            // call spell manager
            if (spellManager != null)
            {
                spellManager.CastSpell(result.Key.GetName(), result.Value);
            }
        }
        varReset();
    }


    //***************************************
    //      Rescale
    //
    //      use:        scales recorded list of points to a square field
    //                  of a chosen size by multiplication of the factor
    //                  of the desired size it already is
    //                  Called on every gesture after recording
    private void Rescale(DrawnGesture gesture)
    {
        float scale = 1f;
        float xrange = gesture.GetMaxX() - gesture.GetMinX();
        float yrange = gesture.GetMaxY() - gesture.GetMinY();
        if (xrange >= yrange)
        {
            scale = standardRatio / (gesture.GetMaxX() - gesture.GetMinX());
        }
        else
        {
            scale = standardRatio / (gesture.GetMaxY() - gesture.GetMinY());
        }
        if (scale != 1)
        {
            foreach (TwoDPoint point in currentPointList)
            {
                point.SetX(point.GetX() * scale);
                point.SetY(point.GetY() * scale);
            }
        }
    }


    //***************************************
    //      MapPoints
    //
    //      use:        maps the list of recorded points to a desired
    //                  number of points by calculating an even distance
    //                  between such a number of points and interpolating
    //                  when that distance is reached upon traversal of the
    //                  list
    //                  Called after scaling on every gesture
    //
    //      param:      gesture:    the object to store the new array
    private void MapPoints(DrawnGesture gesture)
    {
        reducedPoints[0].SetX(currentPointList[0].GetX());
        reducedPoints[0].SetY(currentPointList[0].GetY());
        int newIndex = 1;
        float totalDistance = TotalDistance();
        float coveredDistance = 0;
        float thisDistance = 0;
        float idealInterval = totalDistance / pointsPerGesture;
        for (int i = 0; i < currentPointList.Count - 1; i++)
        {
            thisDistance = PointDistance(currentPointList[i], currentPointList[i + 1]);
            bool passedIdeal = (coveredDistance + thisDistance) >= idealInterval;
            if (passedIdeal)
            {
                TwoDPoint reference = currentPointList[i];
                while (passedIdeal && newIndex < reducedPoints.Length)
                {
                    float percentNeeded = (idealInterval - coveredDistance) / thisDistance;
                    if (percentNeeded > 1f) percentNeeded = 1f;
                    if (percentNeeded < 0f) percentNeeded = 0f;
                    float new_x = (((1f - percentNeeded) * reference.GetX()) + (percentNeeded * currentPointList[i + 1].GetX()));
                    float new_y = (((1f - percentNeeded) * reference.GetY()) + (percentNeeded * currentPointList[i + 1].GetY()));
                    reducedPoints[newIndex] = new TwoDPoint(new_x, new_y);
                    reference = reducedPoints[newIndex];
                    newIndex++;
                    thisDistance = (coveredDistance + thisDistance) - idealInterval;
                    coveredDistance = 0;
                    passedIdeal = (coveredDistance + thisDistance) >= idealInterval;
                }
                coveredDistance = thisDistance;
            }
            else
            {
                coveredDistance += thisDistance;
            }
            gesture.SetPoints(reducedPoints);
        }

    }


    //***************************************
    //      FindMatch
    //
    //      use:        determines template gesture with the minimum
    //                  average distance between points to the 
    //                  currently recorded gesture
    //                  Called after finishing a gesture when not
    //                  recording
    //
    //      param:      playerGesture:  current gesture to be matched
    //                  templates:      object containting list of 
    //                                  gestures to compare against
    //
    //      return:     returns gesture object of the minimum 
    //                  difference template
    private DrawnGesture FindMatch(DrawnGesture playerGesture, GestureTemplates templates)
    {
        float minAvgDifference = float.MaxValue;
        DrawnGesture match = new DrawnGesture("no match", pointsPerGesture);
        foreach (DrawnGesture template in templates.templates)
        {
            // Debug.Log(template.GetName());
            float d = AverageDifference(playerGesture, template);
            // Debug.Log(d.ToString());
            if (d < minAvgDifference)
            {
                minAvgDifference = d;
                match = template;
            }
        }
        return match;
    }

    private KeyValuePair<DrawnGesture, float> FindMatchAndDifference(DrawnGesture playerGesture, GestureTemplates templates)
    {
        float minAvgDifference = float.MaxValue;
        DrawnGesture match = new DrawnGesture("no match", pointsPerGesture);

        // var playerGesture = playerGesture.NormalizedGestures()[0];

        foreach (DrawnGesture template in templates.templates)
        {
            // Debug.Log(template.GetName());
            float d = AverageDifference(playerGesture, template);
            // Debug.Log(d.ToString());
            if (d < minAvgDifference)
            {
                minAvgDifference = d;
                match = template;
            }
        }
        return new KeyValuePair<DrawnGesture, float>(match, minAvgDifference);
    }


    //***************************************
    //      AverageDifference
    //
    //      use:        caluclates the average distance between 
    //                  the points of two gestures
    //
    //      param:      playerGesture:  first to be compared
    //                  template:       gesture to be compared against
    //
    //      return:     returns float value of the average distance
    //                  between points of two parameter gestures
    private float AverageDifference(DrawnGesture playerGesture, DrawnGesture template)
    {
        int numPoints = playerGesture.GetNumPoints();

        if (numPoints != template.GetNumPoints())
        {
            Debug.Log("Number of points differs from templates");
            return -1f;
        }

        float totalDifference = 0;

        for (int i = 0; i < numPoints; i++)
        {
            totalDifference += PointDistance(playerGesture.GetPoints()[i], template.GetPoints()[i]);
        }

        return (totalDifference / numPoints);
    }


    //***************************************
    //      AverageDistanceWithAnomalies
    //
    //      use:        calculates the average difference between 
    //                  the points of two gestures but weighing
    //                  those which deviate significantly by 
    //                  multiplying them
    //                  Both the tightness of this and the factor
    //                  of multiplication are customizable
    //                  above
    //
    //      param:      playerGesture:  first to be compared
    //                  template:       gesture to be compared against
    //
    //      return:     returns float value of the average distance
    //                  between points of two parameter gestures
    //                  with weights
    private float AverageDifferenceWithAnomalies(DrawnGesture playerGesture, DrawnGesture template)
    {
        int numPoints = playerGesture.GetNumPoints();

        if (numPoints != template.GetNumPoints())
        {
            Debug.Log("Number of points differs from templates");
            return -1f;
        }

        float totalDifference = 0;
        float[] sampleDifferences = new float[numPoints];
        float[] sampleDeviations = new float[numPoints];
        float standardDev = 0;

        for (int i = 0; i < numPoints; i++)
        {
            float thisDistance = PointDistance(playerGesture.GetPoints()[i], template.GetPoints()[i]);
            sampleDifferences[i] = thisDistance;
            totalDifference += thisDistance;
        }

        float average = totalDifference / numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            sampleDeviations[i] = Math.Abs(sampleDifferences[i] - average);
            standardDev += sampleDifferences[i];
        }

        standardDev = standardDev / numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            if (Math.Abs(sampleDeviations[i]) > devTightness * standardDev)
            {
                totalDifference -= sampleDifferences[i];
                totalDifference += anomaliesFactor * sampleDifferences[i];
            }
        }

        average = totalDifference / numPoints;

        return (average);
    }

    //***************************************
    //      TotalDistance
    //
    //      use:        calculates the total distance covered
    //                  when traversing the current list of recorded
    //                  points in order of recording
    //                  Called when determining ideal intervals
    //                  for mapping onto desired number of points
    private float TotalDistance()
    {
        float totalDistance = 0;
        for (int i = 0; i < currentPointList.Count - 1; i++)
        {
            totalDistance += PointDistance(currentPointList[i], currentPointList[i + 1]);
        }
        // Debug.Log("total distance: " + totalDistance);
        return totalDistance;
    }


    //***************************************
    //      PointDistance
    //
    //      use:        calculates the absolute value of the distance
    //                  between two points using pythagorean theorem
    private float PointDistance(TwoDPoint a, TwoDPoint b)
    {
        float xDif = a.GetX() - b.GetX();
        float yDif = a.GetY() - b.GetY();
        return Mathf.Sqrt((xDif * xDif) + (yDif * yDif));
    }
}





//******************************************************* Templates ******************************************************//
//
//      Use:    Groups gestures to be used for comparison to a player's attempts

[Serializable]
public class GestureTemplates
{
    public List<DrawnGesture> templates;

    public GestureTemplates()
    {
        templates = new List<DrawnGesture>();
    }

}





//******************************************************** Gestures ******************************************************//
//
//      Use:    Groups all information pertinent to a 'gesture'
//              which is essentially a single stroke drawing represented by points
//
//      Fields:     points:     list of points representing the gesture, only populated once a hand drawn gesture is 
//                              reduced by the MapPoints method
//
//                  min/max:    these are the minimum and maximum x and y values of the points (starting point 
//                              is used as the origin)
//
//                  numPoints:  the size of the points array (set to a variable of the GestureRecognizer class to 
//                              keep control there)
//
//                  name:       string that will be returned when matched with a non-recorded gesture
//
//      Methods:    Initializer(2 parameters):  use when creating a new gesture for later use
//
//                  Initializer(7 parameters):  use when copying data from another gesture
//
//                  Reset:                      for use in clearing the gesture used for each player gesture attempt

[Serializable]
public class DrawnGesture
{
    [SerializeField] private TwoDPoint[] points;
    [SerializeField] private string name;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private int numPoints;

    public DrawnGesture(string newName, int pointsPerGesture)
    {
        numPoints = pointsPerGesture;
        points = new TwoDPoint[numPoints];
        name = newName;
        maxX = 0;
        maxY = 0;
    }
    public DrawnGesture(string newName, int pointsPerGesture, float max_x, float max_y, float min_x, float min_y, TwoDPoint[] newPoints)
    {
        numPoints = pointsPerGesture;
        points = new TwoDPoint[numPoints];
        SetPoints(newPoints);
        name = newName;
        maxX = max_x;
        minX = min_x;
        maxY = max_y;
        minY = min_y;
    }
    public void Reset()
    {
        maxX = 0;
        minX = 0;
        maxY = 0;
        minY = 0;
        name = "";
        Array.Clear(points, 0, numPoints);
    }


    // this method make most high and left point to be the first
    public DrawnGesture[] NormalizedGestures()
    {
        float maxY = float.MinValue;
        float minX = float.MaxValue;
        int j = -1;

        for (int i = 0; i < numPoints; ++i)
        {
            if (points[i].GetY() > maxY || points[i].GetY() == maxY && points[i].GetX() < minX)
            {
                maxY = points[i].GetY();
                minX = points[i].GetX();
                j = i;
            }
        }

        var pointsCopyClockwise = new TwoDPoint[numPoints];
        for (int i = 0; i < numPoints; ++i)
        {
            pointsCopyClockwise[i] = new TwoDPoint(points[(i + j) % numPoints]);
            pointsCopyClockwise[i].MinusX(minX);
            pointsCopyClockwise[i].MinusY(maxY);
        }

        var pointsCopyCounterClockwise = new TwoDPoint[numPoints];
        for (int i = 0; i < numPoints; ++i)
        {
            pointsCopyCounterClockwise[i] = new TwoDPoint(points[(numPoints - i + j) % numPoints]);
            pointsCopyCounterClockwise[i].MinusX(minX);
            pointsCopyCounterClockwise[i].MinusY(maxY);
        }

        var gestures = new DrawnGesture[2];

        float max_x = float.MinValue;
        float max_y = float.MinValue;
        float min_y = float.MaxValue;
        float min_x = float.MaxValue;
        for (int i = 0; i < numPoints; ++i)
        {
            max_x = Math.Max(max_x, pointsCopyClockwise[i].GetX());
            max_y = Math.Max(max_y, pointsCopyClockwise[i].GetY());
            min_y = Math.Min(min_y, pointsCopyClockwise[i].GetY());
            min_x = Math.Min(min_x, pointsCopyClockwise[i].GetX());
        }

        gestures[0] = new DrawnGesture(name + "_cw", numPoints, max_x, max_y, min_x, min_y, pointsCopyClockwise);
        gestures[1] = new DrawnGesture(name + "_ccw", numPoints, max_x, max_y, min_x, min_y, pointsCopyCounterClockwise);
        return gestures;
    }

    public TwoDPoint[] GetPoints()
    {
        return points;
    }
    public void SetPoints(TwoDPoint[] new_points)
    {
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new TwoDPoint(new_points[i].GetX(), new_points[i].GetY());
        }
    }

    public string GetName()
    {
        return name;
    }
    public void SetName(string n)
    {
        name = n;
    }
    public float GetMaxX()
    {
        return maxX;
    }
    public void SetMaxX(float x)
    {
        maxX = x;
    }
    public float GetMaxY()
    {
        return maxY;
    }
    public void SetMaxY(float y)
    {
        maxY = y;
    }
    public float GetMinY()
    {
        return minY;
    }
    public void SetMinY(float y)
    {
        minY = y;
    }
    public float GetMinX()
    {
        return minX;
    }
    public void SetMinX(float x)
    {
        minX = x;
    }
    public int GetNumPoints()
    {
        return numPoints;
    }
    public void SetNumPoints(int n)
    {
        numPoints = n;
    }
}






//******************************************************** Points ********************************************************//
//
//      Use:    This is a class to maintain 2D coordinates
//      
//      Fields:     x:  the x coordinate (relative to the first point when recorded)
//                  y:  the y coordinate (also relative to first point)

[Serializable]
public class TwoDPoint
{
    [SerializeField] private float x;
    [SerializeField] private float y;

    public TwoDPoint(float startx, float starty)
    {
        x = startx;
        y = starty;
    }

    public TwoDPoint(TwoDPoint other)
    {
        x = other.x;
        y = other.y;
    }


    public override string ToString()
    {
        return "X: " + x + " Y: " + y;
    }

    public float GetX()
    {
        return x;
    }
    public void SetX(float new_x)
    {
        x = new_x;
    }
    public float GetY()
    {
        return y;
    }
    public void SetY(float new_y)
    {
        y = new_y;
    }
    public void MinusX(float val)
    {
        x -= val;
    }
    public void MinusY(float val)
    {
        y -= val;
    }
}