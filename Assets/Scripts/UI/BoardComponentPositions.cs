using UnityEngine;

public static class BoardComponentPositions
{
    public static readonly Vector3 PlayerCardDeckPosition = new (-740, 250, -1);
    public static readonly Vector3 PlayerCardDiscardPilePosition = new (-740, 520, -1);
    
    public static readonly Vector3 InfectionCardDeckPosition = new (-100, 20, -1);
    public static readonly Vector3 InfectionCardDiscardPilePosition = new (165, 20, -1);

    public static readonly Vector3 PlayerHandStartingPosition = new (-770, 0, -1);
    public static readonly Vector3 PlayerHandCardOffset = new (160, 0, 0);
}
