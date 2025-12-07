using UnityEngine;

public static class BoardComponentPositions
{
    // z is at -100 because of disease cubes.. probably should be handled with layers instead
    public static readonly Vector3 BoardCenterPosition = new (0, 0, 0);
    public static readonly Vector3 PlayerHandPosition = new (-105, -390, 100);
    public static readonly Vector3 InfectionPilePosition = new (660, 400, 100);

    public static readonly Vector3 PlayerCardDeckPosition = new (-740, 250, -100);
    public static readonly Vector3 PlayerCardDiscardPilePosition = new (-740, 520, -1);
    
    public static readonly Vector3 InfectionCardDeckPosition = new (-100, 20, -1);
    public static readonly Vector3 InfectionCardDiscardPilePosition = new (165, 20, -1);

    public static readonly Vector3 InfectionCardScale = new (80, 80, 0);
    public static readonly Vector3 InfectionCardEnlargedScale = new (170, 170, 0);

    public static readonly Vector3 PlayerHandStartingPosition = new (-770, 0, -1);
    public static readonly Vector3 PlayerHandCardOffset = new (160, 0, 0);

    public static readonly Vector3 PlayerCardScale = new (77, 77, 1);
    public static readonly Vector3 PlayerCardEnlargedScale = new (200, 200, 1);
}
