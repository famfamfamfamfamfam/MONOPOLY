using System;
using UnityEngine;

public class BusTickerHandler : CardHandler
{
    BusTicketService busTicketService;

    RaycastHit[] _hit;
    protected override RaycastHit[] hits => _hit;

    public void Init(BusTicketService busSv)
    {
        busTicketService = busSv;
        _hit = new RaycastHit[1];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HasClickedOn())
            {
                busTicketService.TriggerACard(gamerPlayIndex);
                onFinishCardTrigger.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
