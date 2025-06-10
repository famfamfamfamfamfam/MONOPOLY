using UnityEngine;
using UnityEngine.UI;

public class BuildOptionsPanel : MonoBehaviour, INeedDriver
{
    [SerializeField]
    Button addNewBuildingBtn;
    [SerializeField]
    Button upgradePropertyBtn;

    public Driver driver { get; set; }

    void Start()
    {
        
    }
}
