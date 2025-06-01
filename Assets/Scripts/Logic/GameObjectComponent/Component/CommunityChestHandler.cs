using UnityEngine;

public class CommunityChestHandler : CardHandler
{
    CommunityChestService communityChestService;

    RaycastHit[] _hit;
    protected override RaycastHit[] hits => _hit;

    public void Init(CommunityChestService communitySv)
    {
        communityChestService = communitySv;
        _hit = new RaycastHit[1];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HasClickedOn())
            {
                communityChestService.TriggerACard(gamerPlayIndex);
                onFinishCardTrigger.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
