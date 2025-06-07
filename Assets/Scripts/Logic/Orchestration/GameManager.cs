using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    DiceRoller diceRoller;
    [SerializeField]
    PlayerManager playersManager;

    int currentPlayerIndexInTurn;
    void Start()
    {
        diceRoller.onFinishRoll = step => playersManager.OperateThePlayer(currentPlayerIndexInTurn, step);
    }

}
