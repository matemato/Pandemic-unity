using System.Collections.Generic;
using UnityEngine;

public class InfectionCardDiscardPileController : MonoBehaviour
{
    public List<InfectionCard> _discardedCards = new List<InfectionCard>();

    public void AddToDiscardPile(InfectionCard infectionCard)
    {
        _discardedCards.Add(infectionCard);
    }

    public void RemoveFromDiscardPile(InfectionCard infectionCard)
    {
        _discardedCards.Remove(infectionCard);
    }

    // empty the discard pile 
    public void ClearDiscardPile()
    {
        _discardedCards.Clear();

        // destroy all game objects with tag InfectionCard
        var infectionCards = GameObject.FindGameObjectsWithTag("InfectionCard");
        foreach (var card in infectionCards)
        {
            Destroy(card);
        }
    }
}
