using UnityEngine;

public class CardsConfig : ScriptableObject
{
    public virtual void AccessTheCard(IOnEvent currentPlayer, IChangeCoin[] players, int cardIndex)
    { }
}