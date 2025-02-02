using UnityEngine;

public class Creator : MonoBehaviour
{
    readonly string worldPath = "WorldSpace";
    private void Awake()
    {
        GameObject world = Resources.Load<GameObject>(worldPath);
        Instantiate(world);
    }
}