using UnityEngine;

[CreateAssetMenu(fileName = "New Group", menuName = "Scriptable Objects/Space Group Config")]
public class SpaceGroupConfig : ScriptableObject
{
    public SpaceConfig[] spaces;
}