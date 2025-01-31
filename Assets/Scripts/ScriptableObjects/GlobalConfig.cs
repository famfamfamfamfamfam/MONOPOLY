using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Scriptable Objects/Global Config")]
public class GlobalConfig : ScriptableObject
{
    public int playerCount = 4;
    public int purchasableSpaceCount = 37;
    public int eventSpaceCount = 15;
}