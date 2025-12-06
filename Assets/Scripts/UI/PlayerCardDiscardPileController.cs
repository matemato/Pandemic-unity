using System.Collections.Generic;
using UnityEngine;

public class PlayerCardDiscardPileController : MonoBehaviour
{
    public List<PlayerCard> _discardedCards = new List<PlayerCard>();

    public void AddToDiscardPile(PlayerCard playerCard)
    {
        _discardedCards.Add(playerCard);
    }

    public void RemoveFromDiscardPile(PlayerCard playerCard)
    {
        _discardedCards.Remove(playerCard);
    }
}
