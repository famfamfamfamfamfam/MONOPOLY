using UnityEngine;

public class PositionArranger
{
    GameObject gameObject;
    Vector3[] result;
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

    public Vector3[] GetThePositions(Vector3 origin, int positionsArraySize, float deltaDistance)
    {
        result = new Vector3[positionsArraySize];
        delta = deltaDistance;
        CreateThePositions(origin, positionsArraySize, delta);
        return result;
    }
    public Vector3[] GetThePositions(Vector3 origin)
    {
        if (result == null || delta == 0)
        {
            Debug.LogAssertion("Not yet create necessary data to work - call the other overload");
            return null;
        }
        CreateThePositions(origin, result.Length, delta);
        return result;
    }
    void CreateThePositions(Vector3 origin, int positionsArraySize, float deltaDistance)
    {
        if (gameObject == null)
        {
            gameObject = new GameObject();
        }
        gameObject.transform.position = origin;
        for (int i = 0; i < positionsArraySize; i++)
        {
            gameObject.transform.Rotate(Vector3.up, 360f / positionsArraySize);
            result[i] = deltaDistance * gameObject.transform.forward + gameObject.transform.position;
        }
    }

}
