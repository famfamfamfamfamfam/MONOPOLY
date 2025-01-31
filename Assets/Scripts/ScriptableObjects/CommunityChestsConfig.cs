using UnityEngine;

[CreateAssetMenu(fileName = "New Community", menuName = "Scriptable Objects/Community Chests Config")]
public class CommunityChestsConfig : CardsConfig
{
    [SerializeField]
    CommunityChestAction action;
}