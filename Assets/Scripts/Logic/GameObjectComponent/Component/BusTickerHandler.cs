using UnityEngine;

public class BusTickerHandler : MonoBehaviour
{
    [SerializeField]
    LayerMask busTicketsMask;
    [SerializeField]
    float maxDistanceOfRaycast;

    BusTicketService busTicketService;
    int currentPlayerIndex;

    public void Init(BusTicketService busSv)
    {
        busTicketService = busSv;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), maxDistanceOfRaycast, busTicketsMask))
            {
                busTicketService.TriggerACard(currentPlayerIndex);
                gameObject.SetActive(false);
            }
        }
    }
}
