using UnityEngine;

[CreateAssetMenu(fileName = "New Player Config", menuName = "Scriptable Objects/Player General Config")]
public class PlayerGeneralConfig : ScriptableObject
{
    public int initialCoin = 1500;
    public int timeToBeInJail = 3;
}