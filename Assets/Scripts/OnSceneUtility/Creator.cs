using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField]
    GameObject[] objsNeedRefRuntime;

    [SerializeField]
    Vector3 startPositionCenter;
    [SerializeField]
    float deltaFromStartPositionCenter = 0.025f;

    readonly string worldPath = "Prefabs/WorldSpace";
    readonly string playerFolderPath = "Prefabs/Players";
    private void Awake()
    {
        GameObject world = Resources.Load<GameObject>(worldPath);
        RuntimeRefWrapper wrapper = Instantiate(world).GetComponent<RuntimeRefWrapper>();
        for (int i = 0; i < objsNeedRefRuntime.Length; i++)
        {
            objsNeedRefRuntime[i].GetComponent<INeedRefRuntime>().Init(wrapper);
        }
        CreatePlayers();
    }
    void CreatePlayers()
    {
        GameObject[] players = Resources.LoadAll<GameObject>(playerFolderPath);
        Vector3[] playersPositions = PositionArranger.Instance
            .GetThePositions(startPositionCenter, players.Length, deltaFromStartPositionCenter);
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(players[i], playersPositions[i], Quaternion.identity);
        }
    }

}