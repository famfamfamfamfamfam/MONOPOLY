using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ConfigInitializer.ConstructorParams configs;
    PlayerDataService dataService;
    TriggerSpaceService triggerSpaceService;

    List<PlayerController> players;

    public void AddRef(int playerIndex, PlayerController controller)
    {
        if (players == null)
        {
            players = new List<PlayerController>();
        }
        if (playerIndex > players.Count - 1)
        {
            for (int i = 0; i <= playerIndex - players.Count; i++)
            {
                players.Add(null);
            }
        }
        players[playerIndex] = controller;
    }

    public void Init(ConfigInitializer.ConstructorParams configs, PlayerDataService playerDataSv, TriggerSpaceService triggerSv)
    {
        this.configs = configs;
        dataService = playerDataSv;
        triggerSpaceService = triggerSv;
    }

    void Start()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Init(configs, dataService.SetCurrentCoin, triggerSpaceService.TriggerSpace);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            players[0].StartStep(4);
        }
    }
}
