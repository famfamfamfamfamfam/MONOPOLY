using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField]
    GameObject objNeedJailPosition;
    [SerializeField]
    GameObject objNeedDiceRef;
    [SerializeField]
    GameObject[] objsNeedPlayerRef;
    [SerializeField]
    Transform playersParent;
    [SerializeField]
    Vector3 startPositionCenter;
    [SerializeField]
    float deltaForPositions = 0.025f;

    readonly string worldPath = "Prefabs/WorldSpace";
    readonly string playerFolderPath = "Prefabs/Players";
    private void Awake()
    {
        GameObject world = Resources.Load<GameObject>(worldPath);
        RuntimeRefWrapper wrapper = Instantiate(world).GetComponent<RuntimeRefWrapper>();
        objNeedDiceRef.GetComponent<INeedRefRuntime>().Init(wrapper);
        objNeedJailPosition.GetComponent<INeedRefRuntime>().Init(wrapper);
        CreatePlayers();
    }
    void CreatePlayers()
    {
        GameObject[] players = Resources.LoadAll<GameObject>(playerFolderPath);
        Vector3[] playersPositions = PositionArranger.Instance
            .GetCircularPositions(startPositionCenter, players.Length, deltaForPositions);
        for (int i = 0; i < players.Length; i++)
        {
            RuntimeRefWrapper wrapper = Instantiate(players[i], playersPositions[i], Quaternion.identity, playersParent)
                .GetComponent<RuntimeRefWrapper>();
            objsNeedPlayerRef[i].GetComponent<INeedRefRuntime>().Init(wrapper);
        }
    }

}