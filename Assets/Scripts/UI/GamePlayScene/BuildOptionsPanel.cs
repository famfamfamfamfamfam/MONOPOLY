using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildOptionsPanel : MonoBehaviour, INeedDriver
{
    [SerializeField]
    Button addNewBuildingBtn;
    public Driver driver { get; set; }

    public void Init(UnityAction onAddNewBuilding)
    {
        addNewBuildingBtn.onClick.AddListener(driver.BuildNew);
        addNewBuildingBtn.onClick.AddListener(onAddNewBuilding);
    }

    void OnDestroy()
    {
        addNewBuildingBtn.onClick.RemoveAllListeners();

    }
}
