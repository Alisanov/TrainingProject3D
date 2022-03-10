using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public const string ENEMY_HIT = "ENEMY_HIT";
    public const string SPEED_CHANGED = "SPEED_CHANGED";
    public const string HEALTH_UPDATED = "HEALTH_UPDATED";
    public const string LEVEL_COMPLETE = "LEVEL_COMPLETE";
    public const string LEVEL_FAILED = "LEVEL_FAILED";
    public const string GAME_COMPLETE = "GAME_COMPLETE";
    public const string CHANGE_POSITION = "CHANGE_POSITION";
}
