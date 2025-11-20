using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public enum TickType {  UPDATE, FIXED_UPDATE, LATE_UPDATE }

    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string PlayerCameraTag = "PlayerCamera";
    public const string BasePlayerLogic = "BasePlayerLogic";
    public const string SpawnerRoot = "SpawnerRoot";


    //LAYERS
    public const int GROUND_LAYER = 8;
    public const int ENEMY_LAYER = 6;
    public const int DEFAULT_LAYER = 0;
    public const int CAMERA_LAYER = 3;
    public const int PLAYER_LAYER = 9;

    public enum PLATFORMS { DEFAULT, SWITCH, XBOX, PS, DECK}

    public enum BUILD_TYPE { DEBUG, RELEASE, PRODUCTION }

    public const string SCENE_SUFIX = "_Art";
}

