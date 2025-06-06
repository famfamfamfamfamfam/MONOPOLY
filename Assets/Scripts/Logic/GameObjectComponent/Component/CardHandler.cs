using System;
using UnityEngine;

public abstract class CardHandler : MonoBehaviour
{
    public Action onFinishCardTrigger { get; set; }

    [SerializeField]
    protected LayerMask cardsMask;
    [SerializeField]
    protected float maxDistanceOfRaycast;
    protected int gamerPlayIndex;

    protected abstract RaycastHit[] hits { get; }

    protected bool HasClickedOn()
    {
        return Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, maxDistanceOfRaycast, cardsMask) > 0;
    }

    public void SetGamerIndex(int index)
    {
        gamerPlayIndex = index;
    }
}
