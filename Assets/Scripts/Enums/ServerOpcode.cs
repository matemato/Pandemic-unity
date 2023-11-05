using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerOpcode
{
    SERVER_MESSAGE,
    UPDATE_PLAYERS,
    BEGIN_GAME,
    UPDATE_PLAYER_CARD,
    TRIGGER_INFECTION
}
