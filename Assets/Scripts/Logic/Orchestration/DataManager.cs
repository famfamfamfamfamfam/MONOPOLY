using UnityEngine;

public class DataManager : MonoBehaviour
{
    ConfigInitializer.ConstructorParams configs;
    TriggerSpaceService triggerSpaceService;

    public void Init(ConfigInitializer.ConstructorParams configs, TriggerSpaceService triggerSpaceService)
    {
        this.configs = configs;
        this.triggerSpaceService = triggerSpaceService;
    }

    void TriggerSpace(int playerIndex, int spaceIndex)
    {
        triggerSpaceService.TriggerSpace(playerIndex, spaceIndex);
    }

    [SerializeField]
    Transform tempPlayer;
    public int curPosIndex;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            curPosIndex++;

            if (configs.eventSpaceGroup.spacesIndices.Contains(curPosIndex % 52))
            {
                FindPosAndMove(curPosIndex % 52, configs.eventSpaceGroup.spaces);
            }
            else if (configs.companiesConfig.spacesIndices.Contains(curPosIndex % 52))
            {
                FindPosAndMove(curPosIndex % 52, configs.companiesConfig.spaces);
            }
            else if (configs.stationsConfig.spacesIndices.Contains(curPosIndex % 52))
            {
                FindPosAndMove(curPosIndex % 52, configs.stationsConfig.spaces);
            }
            else
            {
                tempPlayer.position = configs.propertySpaces[curPosIndex % 52].position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerSpace(0, curPosIndex % 52);
        }
    }
    void FindPosAndMove(int index, SpaceConfig[] spaces)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaces[i].indexFromGoSpace == index)
            {
                tempPlayer.position = spaces[i].position;
                return;
            }
        }
    }
}
