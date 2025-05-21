using System;
using UnityEngine;

public abstract class CardHandler : MonoBehaviour
{
    public Action onFinishCardTrigger;

    [SerializeField]
    protected LayerMask cardsMask;
    [SerializeField]
    protected float maxDistanceOfRaycast;
    protected int currentPlayerIndex;

    protected abstract RaycastHit[] hits { get; }

    protected bool HasClickedOn()
    {
        return Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, maxDistanceOfRaycast, cardsMask) > 0;
    }
}
