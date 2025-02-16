using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Community", menuName = "Scriptable Objects/Community Chests Config")]
public class CommunityChestsConfig : ScriptableObject
{
    [Serializable]
    public struct CommunityChest
    {
        public enum MoneyChangeType : byte
        {
            AllChange,
            Opposite,
            Donate
        }
        public MoneyChangeType moneyChange;
        public int value;
    }

    public CommunityChest[] chests;
}