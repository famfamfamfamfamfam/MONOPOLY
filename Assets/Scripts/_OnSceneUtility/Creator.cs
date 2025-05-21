using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField]
    GameObject[] objsNeedRefRuntime;

    readonly string worldPath = "Prefabs/WorldSpace";
    //readonly string playerFolderPath = "Prefabs/Players";
    private void Awake()
    {
        GameObject world = Resources.Load<GameObject>(worldPath);
        RuntimeRefWrapper wrapper = Instantiate(world).GetComponent<RuntimeRefWrapper>();
        for (int i = 0; i < objsNeedRefRuntime.Length; i++)
        {
            objsNeedRefRuntime[i].GetComponent<INeedRefRuntime>().Init(wrapper);
        }
    }
    //public void CreatePlayers(Vector3 goSpaceCenter, float spaceHeight, float spaceWidth)
    //{
    //    GameObject[] players = Resources.LoadAll<GameObject>(playerFolderPath);
    //}
}