using UnityEngine;

public abstract class GroundedSpell : Spell
{
    public void AfterCastSpell()
    {
        // EXPERIMENTAL
        float x = transform.position.x;
        float y = transform.position.y;
        transform.position = new Vector2(CastToNearestHalf(x), CastToNearestHalf(y));
    }

    private float CastToNearestHalf(float number)
    {
        return number - number % 0.5f + 0.5f * (int)((number % 0.5f) / 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
