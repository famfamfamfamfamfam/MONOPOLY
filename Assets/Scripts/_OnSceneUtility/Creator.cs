using UnityEngine;

public class Creator : MonoBehaviour
{
    readonly string worldPath = "Prefabs/WorldSpace";
    //readonly string playerFolderPath = "Prefabs/Players";
    private void Awake()
    {
        GameObject world = Resources.Load<GameObject>(worldPath);
        Instantiate(world);
    }
    //public void CreatePlayers(Vector3 goSpaceCenter, float spaceHeight, float spaceWidth)
    //{
    //    GameObject[] players = Resources.LoadAll<GameObject>(playerFolderPath);
    //}
}