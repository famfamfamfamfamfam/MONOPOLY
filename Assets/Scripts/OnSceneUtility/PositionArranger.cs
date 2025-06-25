using UnityEngine;

public class PositionArranger
{
    GameObject gameObject;
    Vector3[] circularResult;

    float delta;

    static PositionArranger instance;
    public static PositionArranger Instance {
        get {
            if (instance == null)
            {
                instance = new PositionArranger();
            }
            return instance;
        }
    }

    public Vector3[] GetCircularPositions(Vector3 origin, int positionsArraySize, float deltaDistance)
    {
        circularResult = new Vector3[positionsArraySize];
        delta = deltaDistance;
        CreateThePositionsOnCircle(origin, positionsArraySize, delta);
        return circularResult;
    }
    public Vector3[] GetCircularPositions(Vector3 origin)
    {
        if (circularResult == null || delta == 0)
        {
            Debug.LogAssertion("Not yet create necessary data to work - call the other overload");
            return null;
        }
        CreateThePositionsOnCircle(origin, circularResult.Length, delta);
        return circularResult;
    }
    void CreateThePositionsOnCircle(Vector3 origin, int positionsArraySize, float deltaDistance)
    {
        if (gameObject == null)
        {
            gameObject = new GameObject();
        }
        gameObject.transform.position = origin;
        for (int i = 0; i < positionsArraySize; i++)
        {
            gameObject.transform.Rotate(Vector3.up, 360f / positionsArraySize);
            circularResult[i] = deltaDistance * gameObject.transform.forward + gameObject.transform.position;
        }
    }

}
