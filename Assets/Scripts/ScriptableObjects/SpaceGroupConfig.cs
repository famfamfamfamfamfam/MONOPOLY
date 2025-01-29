using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Group", menuName = "Scriptable Objects/Space Group Config")]
public class SpaceGroupConfig : ScriptableObject
{
    [SerializeField]
    SpaceConfig[] spaces;
    HashSet<uint> _spacesIndices;
    public HashSet<uint> spacesIndices
    {
        get
        {
            if (_spacesIndices == null)
            {
                _spacesIndices = new HashSet<uint>();
                foreach (var space in spaces)
                {
                    _spacesIndices.Add(space.indexFromGoSpace);
                }
            }
            return _spacesIndices;
        }
    }
}