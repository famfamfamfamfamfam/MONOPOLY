using UnityEngine;

public class CommunityChestHandler : MonoBehaviour
{
    [SerializeField]
    LayerMask communityChestsMask;
    [SerializeField]
    float maxDistanceOfRaycast;

    CommunityChestService communityChestService;
    int currentPlayerIndex;
    public void Init(CommunityChestService communitySv)
    {
        communityChestService = communitySv;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), maxDistanceOfRaycast, communityChestsMask))
            {
                communityChestService.TriggerACard(currentPlayerIndex);
                gameObject.SetActive(false);
            }
        }
    }
}
