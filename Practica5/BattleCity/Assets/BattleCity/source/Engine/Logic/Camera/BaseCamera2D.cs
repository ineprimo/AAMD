using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCamera2D : MonoBehaviour
{
    private static BaseCamera2D _camera2DInstance;

    public static BaseCamera2D Get()
    {
        return Utils.GetSecureStaticGet<BaseCamera2D>(ref _camera2DInstance, Globals.PlayerCameraTag);
    }
}
