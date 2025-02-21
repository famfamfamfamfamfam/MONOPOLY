using UnityEngine;

public class DieSidePointProperty : MonoBehaviour
{
    [SerializeField]
    int pointOnThis;

    public int point { get => pointOnThis; }
}