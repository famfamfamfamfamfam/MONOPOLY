using UnityEngine;

public class ChanceCardHandler : CardHandler
{
    ChanceService chanceService;

    RaycastHit[] _hit;
    protected override RaycastHit[] hits => _hit;

    public void Init(ChanceService chanceSv)
    {
        chanceService = chanceSv;
        _hit = new RaycastHit[1];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HasClickedOn())
            {
                chanceService.TriggerACard(gamerPlayIndex);
                onFinishCardTrigger.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
