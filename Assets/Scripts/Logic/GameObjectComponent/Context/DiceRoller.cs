using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour, INeedRefRuntime
{
    [SerializeField]
    LayerMask normalDieMask;
    [SerializeField]
    LayerMask thirdDieMask;

    List<DieController> normalDice;
    DieController thirdDie;

    int currentDicePoint;
    public Action<int> onFinishRoll { get; set; }

    public void Init(RuntimeRefWrapper refProvider)
    {
        if (refProvider.GetReference(out List<DieController> allDice))
        {
            AssignTheActions(allDice);
            normalDice = new List<DieController>();
            SeparateDice(allDice);
        }
    }

    void AssignTheActions(List<DieController> dice)
    {
        for (int i = 0; i < dice.Count; i++)
        {
            dice[i].onBeginRoll = () => currentDicePoint = 0;
            dice[i].onFinishRoll = OnFinishRoll;
        }
    }

    void OnFinishRoll(int point, LayerMask dieType)
    {
        currentDicePoint += point;
        if (dieType == normalDieMask)
        {
            if (NormalDiceHasFinishRoll())
            {
                onFinishRoll.Invoke(currentDicePoint);
            }
        }
        else
        {
            onFinishRoll.Invoke(currentDicePoint);
        }
    }

    bool NormalDiceHasFinishRoll()//check trước khi tắt
    {
        for (int i = 0; i < normalDice.Count; i++)
        {
            if (!normalDice[i].enabled)
            {
                return true;
            }
        }
        return false;
    }

    void SeparateDice(List<DieController> dice)
    {
        for (int i = 0; i < dice.Count; i++)
        {
            if (dice[i].dieType == thirdDieMask)
            {
                thirdDie = dice[i];
                continue;
            }
            normalDice.Add(dice[i]);
        }
    }

    public void ActiveRoll(bool normalRolling)
    {
        if (normalRolling)
        {
            for (int i = 0; i < normalDice.Count; i++)
            {
                normalDice[i].enabled = true;
            }
        }
        else
        {
            thirdDie.enabled = true;
        }
    }

}
