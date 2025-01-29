using UnityEngine;

[CreateAssetMenu(fileName = "New Space", menuName = "Scriptable Objects/Space Config")]
public class SpaceConfig : ScriptableObject
{
    public uint indexFromGoSpace;
}

//Gemeinschaftsfeld -> CommunityChest
//Einkommensteuer -> Tax
//Zusatzsteuer -> Tax
//Ereignisfeld -> Chance
//Busfahrkarte -> BusTicket *keep to use
//Station : 1 -> 25$, 2 -> 50$, 3 -> 100$, 4 -> 200$
//Werke : 1 -> 4x, 2 -> 10x