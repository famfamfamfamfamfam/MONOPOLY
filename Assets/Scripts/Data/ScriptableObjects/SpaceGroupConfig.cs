using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group", menuName = "Scriptable Objects/Space Group Config")]
public class SpaceGroupConfig : ScriptableObject
{
    public SpaceConfig[] spaces;

    HashSet<int> _spacesIndices;
    public HashSet<int> spacesIndices
    {
        get
        {
            if (_spacesIndices == null)
            {
                _spacesIndices = new HashSet<int>();
                foreach (var space in spaces)
                {
                    _spacesIndices.Add(space.indexFromGoSpace);
                }
            }
            return _spacesIndices;
        }
    }

    Dictionary<int, EventType> _eventSpaceDictionary;
    public Dictionary<int, EventType> eventDictionary
    {
        get
        {
            if (_eventSpaceDictionary == null)
            {
                _eventSpaceDictionary = new Dictionary<int, EventType>(spaces.Length * 100 / 75 + 1);
                foreach (var space in spaces)
                {
                    if (space is EventSpaceConfig eventSpace)
                    {
                        _eventSpaceDictionary.Add(eventSpace.indexFromGoSpace, eventSpace.eventType);
                    }
                    else
                    {
                        Debug.LogError("This group is not an event spaces group");
                        return null;
                    }
                }
                _eventSpaceDictionary.TrimExcess();
            }
            return _eventSpaceDictionary;
        }
    }
}