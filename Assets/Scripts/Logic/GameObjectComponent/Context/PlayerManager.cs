using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ConfigInitializer.ConstructorParams configs;
    PlayerDataService dataService;

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

    public void Init(ConfigInitializer.ConstructorParams configs, PlayerDataService playerDataSv)
    {
        this.configs = configs;
        dataService = playerDataSv;
    }

    void Start()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Init(configs, dataService.SetCurrentCoin);
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
