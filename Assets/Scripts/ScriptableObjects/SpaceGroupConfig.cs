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
                    _spacesIndices.Add((int)space.indexFromGoSpace);
                }
            }
            return _spacesIndices;
        }
    }
}