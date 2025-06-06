using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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


}
