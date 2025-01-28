using UnityEngine;

public class DiePointProperty : MonoBehaviour, IPointOnSide
{
    [SerializeField]
    int pointOnThis;

    public int point => pointOnThis;
}
