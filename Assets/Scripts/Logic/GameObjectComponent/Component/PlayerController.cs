using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int _playerIndex;
    public int playerIndex { get => _playerIndex; }
    Action<int, int> passGoSpace;

    int currentSpaceIndex;
    float intervalOfSteps = 0.375f;

    ConfigInitializer.ConstructorParams configs;

    List<SpaceConfig> eventSpaces;
    List<SpaceConfig> sub_eventSpaces;
    List<SpaceConfig> companies;
    List<SpaceConfig> sub_companies;
    List<SpaceConfig> stations;
    List<SpaceConfig> sub_stations;
    List<PropertyConfig> properties;
    List<PropertyConfig> sub_properties;

    bool firtsTime = true;

    public void Init(ConfigInitializer.ConstructorParams configs, Action<int, int> onPassGoSpace)
    {
        eventSpaces = configs.eventSpaceGroup.spaces.ToList();
        sub_eventSpaces = new List<SpaceConfig>();
        companies = configs.companiesConfig.spaces.ToList();
        sub_companies = new List<SpaceConfig>();
        stations = configs.stationsConfig.spaces.ToList();
        sub_stations = new List<SpaceConfig>();
        properties = configs.propertySpaces.ToList();
        sub_properties = new List<PropertyConfig>();
        this.configs = configs;

        passGoSpace = onPassGoSpace;
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
            if (currentSpaceIndex == 0 && !firtsTime)
            {
                passGoSpace.Invoke(playerIndex, configs.gameConfig.passGoSpaceBonus);
            }
            currentSpaceIndex++;
            firtsTime = false;
            if (currentSpaceIndex == configs.gameConfig.spaceCount)
            {
                TransferElement(eventSpaces, sub_eventSpaces, 0);
                SwitchTheList(ref eventSpaces, ref sub_eventSpaces);
                SwitchTheList(ref companies, ref sub_companies);
                SwitchTheList(ref stations, ref sub_stations);
                SwitchTheList(ref properties, ref sub_properties);
                currentSpaceIndex = 0;
            }

            if (configs.eventSpaceGroup.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(eventSpaces, sub_eventSpaces);
            }
            else if (configs.companiesConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(companies, sub_companies);
            }
            else if (configs.stationsConfig.spacesIndices.Contains(currentSpaceIndex))
            {
                FindPositionAndMove(stations, sub_stations);
            }
            else
            {
                FindPositionAndMove(properties, sub_properties);
            }
        }

    }

    void TransferElement<T>(List<T> origin, List<T> target, int elementIndex)
    {
        if (origin.Count == 0)
        {
            return;
        }
        target.Add(origin[elementIndex]);
        origin[elementIndex] = origin[origin.Count - 1];
        origin.RemoveAt(origin.Count - 1);
    }

    private void SwitchTheList<T>(ref List<T> spaces, ref List<T> subSpaces)
    {
        List<T> tmp;
        tmp = spaces;
        spaces = subSpaces;
        subSpaces = tmp;
    }

    void FindPositionAndMove<T>(List<T> spaces, List<T> subSpaces) where T : SpaceConfig
    {
        for (int i = 0; i < spaces.Count; i++)
        {
            if (spaces[i]?.indexFromGoSpace == currentSpaceIndex)
            {
                transform.root.position = PositionArranger.Instance.GetThePositions(spaces[i].position)[playerIndex];
                TransferElement(spaces, subSpaces, i);
                return;
            }
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
