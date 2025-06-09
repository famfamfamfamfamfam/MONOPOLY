using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ConfigInitializer.ConstructorParams configs;
    PlayerDataService playerService;
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
        playerService = playerDataSv;
        triggerSpaceService = triggerSv;
    }

    void Start()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Init(configs, playerService.SetCurrentCoin, triggerSpaceService.TriggerSpace, playerService.SetCurrentStaySpaceIndex);
        }
    }

    public void OperateThePlayer(int playerIndex, int step)
    {
        players[playerIndex].StartStep(step);
    }

    public void MovePlayerTo(int playerIndex, Vector3 targetPosition)
    {
        players[playerIndex].MoveTo(targetPosition);
    }

}
