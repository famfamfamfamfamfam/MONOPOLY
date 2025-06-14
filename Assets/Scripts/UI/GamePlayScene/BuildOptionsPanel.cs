using UnityEngine;
using UnityEngine.UI;

public class BuildOptionsPanel : MonoBehaviour, INeedDriver
{
    [SerializeField]
    Button addNewBuildingBtn;
    public Driver driver { get; set; }

    void Start()
    {
        addNewBuildingBtn.onClick.AddListener(driver.BuildNew);
    }

    void OnDestroy()
    {
        addNewBuildingBtn.onClick.RemoveAllListeners();

    }
}
