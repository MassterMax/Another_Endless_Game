using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CallJSlib : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();


    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);

    public void HelloButton()
    {
        Hello();
    }

    public void SetToYandexleaderboard(int value)
    {
        try
        {
            SetToLeaderboard(value);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
