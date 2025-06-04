using System;
using UnityEngine;
using MessagePack;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
            //nếu không return thì dòng sau vẫn được thực thi vì lệnh Destroy chỉ đánh dấu để hủy,
            //không hủy ngay mà hủy vào cuối frame (gọi OnDestroy() sau LateUpdate() và trước khi render)
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }



    public GameData gameData { get; private set; }
    int gamerPlayIndex;

    public bool newGameSelected { get; private set; }

    public bool shuffleMusicsPlay { get; private set; }
    float backSoundsVolume;
    float SFX_volume;

    ConfigInitializer.ConstructorParams configs;
    DataInitializer.ConstructorOuputs data;
    TriggerSpaceService triggerSpaceService;

    readonly string commonDataKey = "CommonData";
    readonly string playersDataKey = "PlayersData";
    readonly string assetsDataKey = "AssetsData";
    readonly string boardDataKey = "BoardData";

    public void Init(ConfigInitializer.ConstructorParams configs, DataInitializer.ConstructorOuputs dataOutputs, TriggerSpaceService triggerSpaceService)
    {
        gameData = dataOutputs.commonData;
        data = dataOutputs;
        gameData.gamerPlayIndex = gamerPlayIndex;
        this.configs = configs;
        this.triggerSpaceService = triggerSpaceService;
    }

    public void SetGamerPlayIndex(ref int index, int optionCount)
    {
        if (index >= optionCount)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = optionCount - 1;
        }
        gamerPlayIndex = index;
    }

    #region Inspector UnityEvents
    public void OnChooseNewGameOption()
    {
        newGameSelected = true;
    }
    public void OnChooseContinueOption()
    {
        newGameSelected = false;
    }

    public void ShuffleMusicsPlaying(bool value)
    {
        shuffleMusicsPlay = value;
    }
    public void SetBackSoundsVolume(float value)
    {
        backSoundsVolume = value;
    }
    public void SetSoundEffectsVolume(float value)
    {
        SFX_volume = value;
    }
    #endregion
    #region Save/Load Data
    public void LoadData(out DataInitializer.ConstructorOuputs data)
    {
        data = new DataInitializer.ConstructorOuputs();
        DeserializeFromPlayerPrefs(data.commonData, commonDataKey);
        DeserializeFromPlayerPrefs(data.playersData, playersDataKey);
        DeserializeFromPlayerPrefs(data.assetsData, assetsDataKey);
        DeserializeFromPlayerPrefs(data.boardData, boardDataKey);
    }
    void DeserializeFromPlayerPrefs<T>(T outData, string playerPrefsKey)
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(playerPrefsKey));
            outData = MessagePackSerializer.Deserialize<T>(bytes);
        }
    }

    public void SaveData()
    {
        SerializeToPlayerPrefs(data.commonData, commonDataKey);
        SerializeToPlayerPrefs(data.playersData, playersDataKey);
        SerializeToPlayerPrefs(data.assetsData, assetsDataKey);
        SerializeToPlayerPrefs(data.boardData, boardDataKey);
        PlayerPrefs.Save();
    }
    void SerializeToPlayerPrefs(object serializedData, string playerPrefsKey)
    {
        byte[] bytes = MessagePackSerializer.Serialize(serializedData);
        PlayerPrefs.SetString(playerPrefsKey, Convert.ToBase64String(bytes));
    }
    #endregion



    void TriggerSpace(int playerIndex, int spaceIndex)
    {
        triggerSpaceService.TriggerSpace(playerIndex, spaceIndex);
    }

    public void Init(Transform tempPlayer)
    {
        this.tempPlayer = tempPlayer;
    }
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
        if (Input.GetKeyDown(KeyCode.E))
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
