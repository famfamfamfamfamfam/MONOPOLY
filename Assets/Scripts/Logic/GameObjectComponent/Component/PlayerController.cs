using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int _playerIndex;
    public int playerIndex { get => _playerIndex; }
    Action<int, int> passGoSpace;
    Action<int, int> finishSteps;

    int currentSpaceIndex;
    float intervalOfSteps = 0.375f;

    ConfigInitializer.ConstructorParams configs;

    int latestStayProperty;

    public void Init(ConfigInitializer.ConstructorParams configs, Action<int, int> onPassGoSpace, Action<int, int> onFinishSteps)
    {
        this.configs = configs;

        passGoSpace = onPassGoSpace;
        finishSteps = onFinishSteps;
    }

    public void StartStep(int step)
    {
        StartCoroutine(Step(step));
    }

    IEnumerator Step(int step)
    {
        bool firstTime = true;
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(intervalOfSteps);
            currentSpaceIndex++;
            if (currentSpaceIndex == configs.gameConfig.spaceCount)
            {
                latestStayProperty = 0;
                currentSpaceIndex = 0;
                firstTime = false;
            }
            else if (currentSpaceIndex > 0 && !firstTime)
            {
                passGoSpace.Invoke(playerIndex, configs.gameConfig.passGoSpaceBonus);
            }

            if (configs.eventSpaceGroup.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.eventSpaceGroup.spaces);
            }
            else if (configs.companiesConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.companiesConfig.spaces);
            }
            else if (configs.stationsConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(configs.stationsConfig.spaces);
            }
            else
            {
                FindPositionAndMove(configs.propertySpaces, latestStayProperty, SetTheLatestPropertyIndex);
            }
        }
        yield return new WaitForSeconds(intervalOfSteps);
        finishSteps.Invoke(playerIndex, currentSpaceIndex);
    }

    void FindPositionAndMove(SpaceConfig[] spaces, int start = 0, Action<int> afterMove = null)
    {
        for (int i = start; i < spaces.Length; i++)
        {
            if (spaces[i]?.indexFromGoSpace == currentSpaceIndex)
            {
                Move(spaces[i]);
                afterMove?.Invoke(i);
                return;
            }
        }
    }

    void SetTheLatestPropertyIndex(int i)
    {
        latestStayProperty = i;
    }

    void Move(SpaceConfig space)
    {
        transform.root.position = PositionArranger.Instance.GetThePositions(space.position)[playerIndex];
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
