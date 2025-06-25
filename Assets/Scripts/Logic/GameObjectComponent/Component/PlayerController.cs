using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int _playerIndex;
    public int playerIndex { get => _playerIndex; }
    Action<int, int> bonusAction;
    Action<int, int> finishSteps;
    Action<int, int> changingCurrentSpace;

    int currentSpaceIndex;
    float intervalOfSteps = 0.375f;
    bool canGiveBonus;

    ConfigInitializer.ConstructorParams configs;

    public void Init(ConfigInitializer.ConstructorParams configs,
        Action<int, int> onPassGoSpace, Action<int, int> onFinishSteps, Action<int, int> onChangeCurrentSpaceIndex)
    {
        this.configs = configs;

        bonusAction = onPassGoSpace;
        finishSteps = onFinishSteps;
        changingCurrentSpace = onChangeCurrentSpaceIndex;
    }

    public void StartStep(int step)
    {
        StartCoroutine(Step(step));
    }

    IEnumerator Step(int step)
    {
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(intervalOfSteps);
            currentSpaceIndex++;
            if (currentSpaceIndex == configs.gameConfig.spaceCount)
            {
                currentSpaceIndex = 0;
                canGiveBonus = true;
            }
            if (currentSpaceIndex > 0 && canGiveBonus)
            {
                bonusAction.Invoke(_playerIndex, configs.gameConfig.passGoSpaceBonus);
                canGiveBonus = false;
            }

            if (configs.eventSpaceGroup.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.eventSpaceGroup.spaces, 0);
            }
            else if (configs.companiesConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.companiesConfig.spaces, 0);
            }
            else if (configs.stationsConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.stationsConfig.spaces, 0);
            }
            else
            {
                FindPositionAndMove(configs.propertySpaces, currentSpaceIndex);
            }
        }
        changingCurrentSpace.Invoke(_playerIndex, currentSpaceIndex);
        yield return new WaitForSeconds(intervalOfSteps);
        finishSteps.Invoke(_playerIndex, currentSpaceIndex);
    }

    void FindPositionAndMove(SpaceConfig[] spaces, int start)
    {
        if (spaces is PropertyConfig[])
        {
            Move(spaces[currentSpaceIndex]);
            return;
        }
        for (int i = start; i < spaces.Length; i++)
        {
            if (spaces[i].indexFromGoSpace == currentSpaceIndex)
            {
                Move(spaces[i]);
                return;
            }
        }
    }

    void Move(SpaceConfig space)
    {
        transform.root.position = PositionArranger.Instance.GetCircularPositions(space.position)[playerIndex];
    }

    public void MoveTo(Vector3 targetPosition)
    {
        transform.root.position = targetPosition;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}