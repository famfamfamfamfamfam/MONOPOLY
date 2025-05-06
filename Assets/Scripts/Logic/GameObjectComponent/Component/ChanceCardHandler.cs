using UnityEngine;

public class ChanceCardHandler : MonoBehaviour
{
    [SerializeField]
    LayerMask chanceCardsMask;
    [SerializeField]
    float maxDistanceOfRaycast;
    ChanceService chanceService;
    int currentPlayerIndex;
    public void Init(ChanceService chanceSv)
    {
        chanceService = chanceSv;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), maxDistanceOfRaycast, chanceCardsMask))
            {
                chanceService.TriggerACard(currentPlayerIndex);
                gameObject.SetActive(false);
            }
        }
    }
}
