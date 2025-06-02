using UnityEngine;

public class CursorMover : MonoBehaviour
{
    [SerializeField]
    RectTransform cursor;
    [SerializeField]
    int _optionCount = 4;

    public int optionCount { get => _optionCount; } 

    float step = 480;
    float startPoint;
    float endPoint;
    float currentPosition;

    void Start()
    {
        currentPosition = cursor.position.x;
        startPoint = currentPosition;
        endPoint = startPoint + (step * (_optionCount - 1));
    }

    public void MoveToRight()
    {
        Navigate(endPoint, startPoint, step);
        Move();
    }

    public void MoveToLeft()
    {
        Navigate(startPoint, endPoint, -step);
        Move();
    }

    void Navigate(float limitPoint, float telePoint, float signedStep)
    {
        if (Mathf.Approximately(currentPosition, limitPoint))
        {
            currentPosition = telePoint;
        }
        else
        {
            currentPosition += signedStep;
        }
    }

    void Move()
    {
        cursor.position = new Vector3(currentPosition, cursor.position.y, cursor.position.z);
    }

}
