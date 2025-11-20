using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum RumbleInfoType { NORMAL, INTERPOLATED, POSITIONAL}
public enum TRumbleState { RUMBLE, STOP, PAUSE}
public class RumbleInfo
{
    protected float motor1;
    protected float motor2;
    protected float time;
    protected float currentTime;
    protected RumbleInfoType rumbleInfoType;
    protected EasyFunctionType easyFunction;
    protected EasyFunctionType easyFunction2;
    protected bool inverseEasyFunction;
    protected bool inverseEasyFunction2;
    protected AnimationCurve animationCurve1;
    protected AnimationCurve animationCurve2;
    protected Transform emmisor;
    protected Transform receptor;
    protected float maxDistance;
    protected float minDistance;
    protected float pan;
    protected bool infinite;
    protected bool fadeOut;
    protected float fadeOutTime;
    protected float fadeOutTimeMax;
    protected int priority;
    protected bool background;
    protected bool growingFunction;
    protected TRumbleState state;

    public TRumbleState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }
    public bool GrowingFunction
    {
        get
        {
            return growingFunction;
        }
        set
        {
            growingFunction = value;
        }
    }

    public bool IsInfinite
    {
        get
        {
            return infinite;
        }
    }

    public int Priority
    {
        get
        {
            return priority;
        }

        set
        {
            priority = value;
        }
    }

    public bool IsBackground
    {
        get
        {
            return background;
        }
    }

    public float InterpolateFadeOutTime
    {
        get
        {
            return Mathf.Max(fadeOutTime / fadeOutTimeMax, 0f);
        }
    }

    public bool IsFadingOut
    {
        get
        {
            return fadeOut;
        }
    }

    public void SetFadingOut(float time)
    {
        fadeOut = true;
        fadeOutTime = time;
        fadeOutTimeMax = time;
    }

    public bool UpdateFadeOutTimeAndCheckFinished(float t)
    {
        fadeOutTime -= t;
        return fadeOutTime <= 0f;
    }
    public float MaxDistance
    {
        get { return maxDistance; }
    }

    public float MinDistance
    {
        get { return minDistance; }
    }

    public float Pan
    {
        get { return pan; }
    }

    public float Motor1
    {
        get { return motor1; }
    }

    public Transform Emmisor
    {
        get
        {
            return emmisor;
        }
    }


    public Transform Receptor
    {
        get
        {
            return receptor;
        }
    }

    public float Motor2
    {
        get { return motor2; }
    }



    public float Time
    {
        get { return time; }
    }

    public float CurrenTime
    {
        get { return currentTime; }
    }

    public RumbleInfoType RumbleType
    {
        get
        {
            return rumbleInfoType;
        }

        set
        {
            rumbleInfoType = value;
        }
    }


    public RumbleInfo(float m1, float m2, float t, RumbleInfoType rType, int prior, bool growing = true, EasyFunctionType ef = EasyFunctionType.Constant, EasyFunctionType ef2 = EasyFunctionType.Constant, bool ief = false, bool ief2 = false, AnimationCurve ac1=null, AnimationCurve ac2 = null, Transform em=null, Transform rec=null, float maxD=0f, float minD=0f, float p=0f, bool back = false)
    {
        motor1 = m1;
        motor2 = m2;
        time = t;
        if (time < 0f)
            infinite = true;
        currentTime = 0;
        rumbleInfoType = rType;
        easyFunction = ef;
        easyFunction2 = ef2;
        inverseEasyFunction = ief;
        inverseEasyFunction2 = ief2;
        animationCurve1 = ac1;
        animationCurve2 = ac2;
        emmisor = em;
        receptor = rec;
        maxDistance = maxD;
        minDistance = minD;
        pan = p;
        fadeOut = false;
        fadeOutTime = 0f;
        priority = prior;
        background = back;
        growingFunction = growing;
        state = TRumbleState.RUMBLE;
    }


    public void Update(float t)
    {
        currentTime += t;
        
    }

    public bool IsTimeFinished
    {
        get
        {
            return (currentTime > time) && !IsInfinite;
        }
    }

    public float InterpolateTime
    {
        get
        {
            float f= currentTime / time;
            return Mathf.Min(f, 1f);
        }
    }

    public EasyFunctionType EasyFunctionType
    {
        get
        {
            return easyFunction;
        }
    }

    public bool InverseEasyFunction
    {
        get
        {
            return inverseEasyFunction;
        }
    }

    public bool InverseEasyFunction2
    {
        get
        {
            return inverseEasyFunction2;
        }
    }

    public EasyFunctionType EasyFunctionType2
    {
        get
        {
            return easyFunction2;
        }
    }

    public AnimationCurve AnimationCurve1
    {
        get
        {
            return animationCurve1;
        }
    }

    public AnimationCurve AnimationCurve2
    {
        get
        {
            return animationCurve2;
        }
    }

    public bool IsLessPriorityThan(int p)
    {
        return priority < p;
    }
}

[System.Serializable]
public class RumbleAnimationTime
{
    public AnimationCurve curve1;
    public AnimationCurve curve2;
    public float timeScale;

    private float currentTime;

    public void Reset()
    {
        currentTime = 0f;
    }

    public void SpendTime(float t)
    {
        currentTime += t;
        if (currentTime >= timeScale)
        {
            currentTime = currentTime - timeScale;
        }
    }

    public Vector2 CalculateRumbleIntensity(Vector2 max)
    {
        float interpolate = currentTime / timeScale;
        float f1 = curve1.Evaluate(interpolate);
        float f2 = curve2.Evaluate(interpolate);
        return new Vector2(f1 * max.x, f2 * max.y);
    }
}
public class RumbleMgr : MonoBehaviour
{
    private Dictionary<Gamepad, RumbleInfo> _activeRumbles;
    private Dictionary<Gamepad, RumbleInfo> _backgroundRumbles;
    private Dictionary<Gamepad, RumbleAnimationTime> _backgroundRumbleAnimations;
    private List<Gamepad> _marketToDelete;
    private List<Gamepad> _marketToDeleteBackground;
    // Start is called before the first frame update
    void Start()
    {
        _activeRumbles = new Dictionary<Gamepad, RumbleInfo>();
        _backgroundRumbles = new Dictionary<Gamepad, RumbleInfo>();
        _backgroundRumbleAnimations = new Dictionary<Gamepad, RumbleAnimationTime>();
        _marketToDelete = new List<Gamepad>();
        _marketToDeleteBackground = new List<Gamepad>();
    }


    public bool IsGamePadRumble(Gamepad gamepad)
    {
        if (gamepad == null)
            return false;
        return _activeRumbles.ContainsKey(gamepad) || _backgroundRumbles.ContainsKey(gamepad);
    }

    public void PauseAllActiveRumbles()
    {
        if (_activeRumbles != null)
        {
            foreach (var pair in _activeRumbles)
            {
                pair.Key.PauseHaptics();
                pair.Value.State = TRumbleState.PAUSE;
            }
        }
    }

    public void PauseAllBackgroundRumbles()
    {
        if (_backgroundRumbles != null)
        {
            foreach (var pair in _backgroundRumbles)
            {
                if (!_activeRumbles.ContainsKey(pair.Key) && pair.Value.State == TRumbleState.RUMBLE)
                    pair.Key.PauseHaptics();
                pair.Value.State = TRumbleState.PAUSE;
            }
        }
    }

    public void PauseAllRumbles()
    {
        PauseAllActiveRumbles();

        PauseAllBackgroundRumbles();
    }

    public void ResumeActiveRumbles()
    {
        if (_activeRumbles != null)
        {
            foreach (var pair in _activeRumbles)
            {
                pair.Key.ResumeHaptics();
                pair.Value.State = TRumbleState.RUMBLE;
            }
        }
    }

    public void ResumenBackgroundRumbles()
    {
        if (_backgroundRumbles != null)
        {
            foreach (var pair in _backgroundRumbles)
            {
                pair.Key.ResumeHaptics();
                pair.Value.State = TRumbleState.RUMBLE;
            }
        }
    }

    public void ResumenAllRumbles()
    {
        ResumeActiveRumbles();

        ResumenBackgroundRumbles();
    }

    public void AllPlayerRumble(float motor1, float motor2, float time, int priority = 0)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad gp = Gamepad.all[i];
            if(gp != null)
            {
                InitRumble(gp, motor1, motor2, time, priority);
            }
        }
    }

    public void AllPlayerRumbleInterpolated(float motor1Min, float motor2Min, float time, EasyFunctionType easyFunctionTypeLeft, EasyFunctionType easyFunctionTypeRight, bool ief, bool ief2, AnimationCurve curve1, AnimationCurve curve2, int priority = 0)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad gp = Gamepad.all[i];
            if (gp != null)
            {
                InitRumbleInterpolate(gp, motor1Min, motor2Min, time, easyFunctionTypeLeft, easyFunctionTypeRight, ief, ief2, curve1, curve2, priority);
            }
        }
    }



    public void AllPlayerRumblePositional(Transform emmisor, PlayerInputControllerBase[] receptors, float motorMax, float motorPan, float maxDistance, float minDistance, float time, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, int priority = 0)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        for (int i = 0; i < receptors.Length; i++)
        {
            Gamepad gp = receptors[i].GetGamePad();
            if (gp != null)
            {
                InitPositionalRumble(gp, emmisor, receptors[i].transform, motorMax, motorPan, maxDistance, minDistance, time, easyFunctionType, curve1, priority);
            }
        }
    }

    public void StopAllActiveRumbles()
    {
        if (_activeRumbles != null)
        {
            foreach (var pair in _activeRumbles)
            {
                if (pair.Key != null)
                {
                    Stop(pair.Key);
                    pair.Value.State = TRumbleState.STOP;
                }
            }
            _activeRumbles.Clear();
        }
    }

    public void StopAllRumbles()
    {
        StopAllActiveRumbles();
        StopAllBackgroundRumbles();
    }

    protected void Stop(Gamepad gp)
    {
        if (gp == null)
            return;
        gp.PauseHaptics();
        gp.ResetHaptics();
    }

    public void InitRumble(Gamepad gamepad, float motor1, float motor2, float time, int priority=0, bool growing = true)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (gamepad == null)
            return;
        if (_activeRumbles == null)
            return;
        if (_activeRumbles.ContainsKey(gamepad))
        {
            RumbleInfo currentRI = _activeRumbles[gamepad];
            if (currentRI.IsLessPriorityThan(priority))
                StopPreviousAndDelete(gamepad);
            else
                return; // si el rumble tiene menos o igual prioridad que el que está puesto se descarta.
        }
        _activeRumbles.Add(gamepad, new RumbleInfo(motor1, motor2, time,RumbleInfoType.NORMAL, priority, growing));
        gamepad.SetMotorSpeeds(motor1, motor2);
    }


    protected void StopPreviousAndDelete(Gamepad gamepad)
    {
        if (gamepad == null)
            return;
        if (_activeRumbles == null)
            return;
        Stop(gamepad);
        _activeRumbles.Remove(gamepad);
    }

    protected void StopPreviousAndDeleteBackground(Gamepad gamepad)
    {
        if (gamepad == null)
            return;
        if (_backgroundRumbles == null)
            return;
        if(!_backgroundRumbles.ContainsKey(gamepad))
        {
            Stop(gamepad);
        }
        _backgroundRumbles.Remove(gamepad);
        if (_backgroundRumbleAnimations.ContainsKey(gamepad))
            _backgroundRumbleAnimations.Remove(gamepad);
    }


    public void InitRumbleInterpolate(Gamepad gamepad, float motor1Min, float motor2Min, float time, EasyFunctionType easyFunctionTypeLeft, EasyFunctionType easyFunctionTypeRight, bool ief, bool ief2, AnimationCurve curve1, AnimationCurve curve2, int priority = 0, bool growing = true)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (gamepad == null)
            return;
        if (time <0f)
        {
            Debug.LogError("Los rumbles interpolados no pueden durar de forma indefinida ");
            return;
        }

        if (_activeRumbles == null)
            return;

        if (_activeRumbles.ContainsKey(gamepad))
        {
            RumbleInfo currentRI = _activeRumbles[gamepad];
            if (currentRI.IsLessPriorityThan(priority))
                StopPreviousAndDelete(gamepad);
            else
                return; // si el rumble tiene menos o igual prioridad que el que está puesto se descarta.
        }
        _activeRumbles.Add(gamepad, new RumbleInfo(motor1Min, motor2Min, time, RumbleInfoType.INTERPOLATED, priority, growing, easyFunctionTypeLeft, easyFunctionTypeRight, ief, ief2, curve1, curve2));
        gamepad.SetMotorSpeeds(EasyFunctions.CalculateInterpolation(easyFunctionTypeLeft,0f), EasyFunctions.CalculateInterpolation(easyFunctionTypeRight,0f));
    }

    /// <summary>
    /// Inicia los motores de los rumble con un objeto como fuente. LA intensidad del rumble variara en función de la distancia
    /// a la que nos encontremos del origen dle rumble.
    /// </summary>
    /// <param name="gamepad">El game pad que debemos accionar</param>
    /// <param name="emmisor">el origen de la vibración</param>
    /// <param name="receptor">en teoria lap osición del que siente la vibración</param>
    /// <param name="motorMax">la intensidad máxima que alcanza la vibración</param>
    /// <param name="motorPan">La diferencia entre los dos motores</param>
    /// <param name="maxDistance">La distancia máxima a la que se siente</param>
    /// <param name="minDistance">La distancia mínima donde no hay decaimiento de la señal</param>
    /// <param name="time">el tiempo que dura, -1 infinito</param>
    /// <param name="easyFunctionType">el tipo de decaimiento de la vibración</param>
    /// <param name="curve">En caso de tipo custom, la curva de decaimiento</param>
    public void InitPositionalRumble(Gamepad gamepad, Transform emmisor, Transform receptor, float motorMax, float motorPan, float maxDistance, float minDistance, float time, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, int priority = 0, bool growing = true)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (gamepad == null)
            return;
        if (_activeRumbles == null)
            return;
        if (_activeRumbles.ContainsKey(gamepad))
        {
            RumbleInfo currentRI = _activeRumbles[gamepad];
            if (currentRI.IsLessPriorityThan(priority))
                StopPreviousAndDelete(gamepad);
            else
                return; // si el rumble tiene menos o igual prioridad que el que está puesto se descarta.
        }

        RumbleInfo ri = new RumbleInfo(motorMax, motorMax, time, RumbleInfoType.POSITIONAL, priority, growing, easyFunctionType, easyFunctionType, false, false,curve1, curve1, emmisor, receptor, maxDistance,minDistance, motorPan,false);
        _activeRumbles.Add(gamepad, ri);
        Vector2 motorIntensity = CalculatePositionalMotorIntensity(ri);
        gamepad.SetMotorSpeeds(motorIntensity.x, motorIntensity.y);
    }

    public void InitAllOneShotRumblePositional(PlayerInputControllerBase[] receptors, Vector3 emmisor, float motorMax, float motorPan, float maxDistance, float minDistance, float time, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, int priority = 0)
    {
        for(int i = 0; i < receptors.Length; i++)
        {
            InitOneShotRumblePositional(receptors[i].GetGamePad(), emmisor, receptors[i].transform.position, motorMax, motorPan, maxDistance, minDistance, time, easyFunctionType, curve1, priority);
        }
    }

    public void InitOneShotRumblePositional(Gamepad gamepad, Vector3 emmisor, Vector3 receptor, float motorMax, float motorPan, float maxDistance, float minDistance, float time, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, int priority = 0, bool growing = true)
    {
        if (gamepad == null)
            return;        

        float distanced = Vector3.Distance(receptor,emmisor);
        if (distanced > maxDistance)
            return;

        if (distanced < minDistance)
        {
            InitRumble(gamepad, motorMax, motorMax, time, priority);
        }
        else
        {
            Vector2 motorIntensity = CalculatePositionalMotorIntensity(motorMax, motorPan, minDistance, maxDistance, easyFunctionType, curve1, emmisor, receptor, growing);
            InitRumble(gamepad, motorIntensity.x, motorIntensity.y, time, priority);
        }
    }

    public void StopRumble(Gamepad pad)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (pad == null)
            return;
        if (_activeRumbles == null)
            return;
        if (_activeRumbles.ContainsKey(pad))
        {
            _activeRumbles.Remove(pad);
            Stop(pad);
        }
    }
    public void FadeOutRumble(Gamepad pad, float timeToFadeOut)
    {
        if (pad == null)
            return;
        if (_activeRumbles == null)
            return;
        if (_activeRumbles.ContainsKey(pad))
        {
            RumbleInfo rumble = _activeRumbles[pad];
            if(rumble.IsInfinite && rumble.RumbleType != RumbleInfoType.INTERPOLATED)
            {
                rumble.SetFadingOut(timeToFadeOut);
            }
            else
            {
                Debug.LogError("No se puede hacer fadeOut de un rumble no infinito o a un rumble interpolado");
            }
        }
    }

    public void InitAllBackgroundRumble(PlayerInputControllerBase[] players, Transform emmisor, float motorMax, float motorPan, float maxDistance, float minDistance, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, RumbleAnimationTime rat = null)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (players == null)
            return;
        for (int i = 0; i < players.Length; i++)
        {
            Gamepad gp = players[i].GetGamePad();
            if(gp != null)
            {
                InitBackgroundRumble(gp, emmisor, players[i].transform, motorMax, motorPan, maxDistance, minDistance, easyFunctionType, curve1, true, rat);
            }
        }
    }

    public void InitBackgroundRumble(Gamepad gamepad, Transform emmisor, Transform receptor, float motorMax, float motorPan, float maxDistance, float minDistance, EasyFunctionType easyFunctionType, AnimationCurve curve1 = null, bool growing = true, RumbleAnimationTime rat = null)
    {
        if (!GameMgr.Instance.GetServer<BaseQualityMgr>().RumbleEnable)
            return;
        if (gamepad == null)
            return;

        if (_backgroundRumbles == null)
            return;
        if (_backgroundRumbles.ContainsKey(gamepad))
        {
            StopPreviousAndDeleteBackground(gamepad); // en el background siempre descartamos.
        }

        RumbleInfo ri = new RumbleInfo(motorMax, motorMax, -1, RumbleInfoType.POSITIONAL, 0, growing, easyFunctionType, easyFunctionType, false, false, curve1, curve1, emmisor, receptor, maxDistance, minDistance, motorPan,true);
        _backgroundRumbles.Add(gamepad, ri);
        if(rat != null)
        {
            _backgroundRumbleAnimations.Add(gamepad,rat);
        }
        Vector2 motorIntensity = CalculatePositionalMotorIntensity(ri);
        if (!_activeRumbles.ContainsKey(gamepad)) // no ponemos este porque hay otro sonando en primer plano
        {
            if(_backgroundRumbleAnimations.ContainsKey(gamepad))
            {
                RumbleAnimationTime rmi = _backgroundRumbleAnimations[gamepad];
                rmi.Reset();
                motorIntensity = rmi.CalculateRumbleIntensity(motorIntensity);
            }

            gamepad.SetMotorSpeeds(motorIntensity.x, motorIntensity.y);
        }
        
    }

    public void StopBackgroundRumble(Gamepad gamepad)
    {
        if (_backgroundRumbles == null)
            return;
        if (_backgroundRumbles.ContainsKey(gamepad))
        {
            StopPreviousAndDeleteBackground(gamepad); // en el background siempre descartamos.
        }
    }

    public void StopAllBackgroundRumbles()
    {
        if (_backgroundRumbles == null)
        {
            foreach (var pair in _backgroundRumbles)
            {
                if (pair.Key != null)
                {
                    if (!_activeRumbles.ContainsKey(pair.Key) && pair.Value.State == TRumbleState.RUMBLE)
                        Stop(pair.Key);
                    pair.Value.State = TRumbleState.STOP;
                }
            }
            _backgroundRumbles.Clear();
            _backgroundRumbleAnimations.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_activeRumbles != null)
        {
            foreach (var pair in _activeRumbles)
            {
                if (pair.Key == null)
                    _marketToDelete.Add(pair.Key);
                else
                {
                    if(pair.Value.State == TRumbleState.RUMBLE)
                    {
                        if (pair.Value.RumbleType == RumbleInfoType.NORMAL)
                        {
                            RumbleNormal(pair.Key, pair.Value);
                        }
                        else if (pair.Value.RumbleType == RumbleInfoType.INTERPOLATED)
                        {
                            RumbleInterpolated(pair.Key, pair.Value);
                        }
                        else
                        {
                            if (pair.Value.Emmisor == null || pair.Value.Receptor == null)
                            {
                                _marketToDelete.Add(pair.Key);
                                Stop(pair.Key);
                                pair.Value.State = TRumbleState.STOP;
                            }
                            else
                                RumblePositional(pair.Key, pair.Value, false);
                        }
                        FadeOutUpdate(pair.Key, pair.Value);
                    }
                }
            }
        }

        if(_backgroundRumbles != null)
        {
            foreach (var pair in _backgroundRumbles)
            {
                if (pair.Key == null)
                    _marketToDeleteBackground.Add(pair.Key);
                else
                {
                    if (!_activeRumbles.ContainsKey(pair.Key))
                    {
                        if (pair.Value.Emmisor == null || pair.Value.Receptor == null)
                        {
                            _marketToDeleteBackground.Add(pair.Key);
                            Stop(pair.Key);
                            pair.Value.State = TRumbleState.STOP;
                        }  
                        else if (pair.Value.State == TRumbleState.RUMBLE)
                            RumblePositional(pair.Key, pair.Value,true);
                    }
                }
            }
        }
        

        while (_marketToDelete.Count > 0)
        {
            Gamepad pad = _marketToDelete[0];
            _activeRumbles.Remove(pad);
            _marketToDelete.RemoveAt(0);
        }

        while (_marketToDeleteBackground.Count > 0)
        {
            Gamepad pad = _marketToDeleteBackground[0];
            _backgroundRumbles.Remove(pad);
            if(_backgroundRumbleAnimations.ContainsKey(pad))
                _backgroundRumbleAnimations.Remove(pad);
            _marketToDeleteBackground.RemoveAt(0);
        }

        
    }

    protected void RumbleNormal(Gamepad gamepad, RumbleInfo rumbleInfo)
    {
        if (gamepad == null)
            return;
        UpdateTimeAndCheckFinish(gamepad, rumbleInfo);
        if (rumbleInfo.IsInfinite && rumbleInfo.IsFadingOut)
        {
            float m1 = Mathf.Lerp(rumbleInfo.Motor1, 0f, rumbleInfo.InterpolateFadeOutTime);
            float m2 = Mathf.Lerp(rumbleInfo.Motor2, 0f, rumbleInfo.InterpolateFadeOutTime);
            gamepad.SetMotorSpeeds(m1, m2);
        }
    }

    protected void FadeOutUpdate(Gamepad gamepad, RumbleInfo rumbleInfo)
    {
        if (gamepad == null)
            return;
        if (rumbleInfo.IsFadingOut)
        {
            if(rumbleInfo.UpdateFadeOutTimeAndCheckFinished(Time.deltaTime))
            {
                _marketToDelete.Add(gamepad);
                gamepad.PauseHaptics();
            }
        }
    }
    protected bool UpdateTimeAndCheckFinish(Gamepad gamepad, RumbleInfo rumbleInfo)
    {
        if (gamepad == null)
            return false;
        if (!rumbleInfo.IsInfinite)
            rumbleInfo.Update(Time.deltaTime);
        if (rumbleInfo.IsTimeFinished)
        {
            _marketToDelete.Add(gamepad);
            gamepad.PauseHaptics();
            return true;
        }
        else
            return false;
    }

    protected Vector2 CalculatePositionalMotorIntensity(RumbleInfo rumbleInfo)
    {
        return CalculatePositionalMotorIntensity(rumbleInfo.Motor1, rumbleInfo.Pan, rumbleInfo.MinDistance, 
            rumbleInfo.MaxDistance, rumbleInfo.EasyFunctionType, rumbleInfo.AnimationCurve1, rumbleInfo.Emmisor.position, rumbleInfo.Receptor.position, rumbleInfo.GrowingFunction);
    }

    protected Vector2 CalculatePositionalMotorIntensity(float motor1, float pan, float minDistance, float maxDistance, EasyFunctionType easyFunction, AnimationCurve curve, Vector3 emmisor, Vector3 receptor, bool growingFunction)
    {
        float distance = Vector3.Distance(emmisor, receptor);
        if (distance < minDistance) // la vibración es máxima, pero mantenemos el pan.
        {
            return CalculatePan(motor1,pan, emmisor,receptor, minDistance);
        }
        else if (distance > maxDistance) // no vibra
        {
            return Vector2.zero;
        }
        else
        {
            // entre medias, interpolamos entre el max y 0 en función de la distancia
            float range = maxDistance - minDistance;
            float relativeD = distance - minDistance;
            float interpolate = growingFunction ? 1f - (relativeD / range)  : (relativeD / range);
            interpolate = Mathf.Clamp(interpolate, 0f, 1f);
            float motorIntensity = 0f;
            if (easyFunction == EasyFunctionType.CustomFunction)
                motorIntensity = curve.Evaluate(interpolate);
            else
                motorIntensity = EasyFunctions.CalculateInterpolation(easyFunction, interpolate);

            return CalculatePan(motor1 * motorIntensity, pan, emmisor, receptor, minDistance);
        }
    }

    public static Vector2 CalculatePan(float rumbleMax, float pan, Vector3 emmisor, Vector3 receptor, float minDifferenceToPan)
    {
        float xd = emmisor.x - receptor.x;
        if (xd < minDifferenceToPan)
            return new Vector2(rumbleMax, rumbleMax);
        else if (emmisor.x > receptor.x)
            return new Vector2(rumbleMax * pan, rumbleMax);
        else
            return new Vector2(rumbleMax, rumbleMax * pan);
    }

    public static Vector2 CalculatePan(float rumbleMax, RumbleInfo rumbleInfo,float minDifferenceToPan)
    {
        return CalculatePan(rumbleMax, rumbleInfo.Pan, rumbleInfo.Emmisor.position, rumbleInfo.Receptor.position, minDifferenceToPan);
    }


    protected void RumbleInterpolated(Gamepad gamepad, RumbleInfo rumbleInfo)
    {
        if (gamepad == null)
            return;
        if (!UpdateTimeAndCheckFinish(gamepad, rumbleInfo))
        {
            float interpolation = rumbleInfo.InterpolateTime;
            float motor1Intensity = 0f;
            float motor2Intensity = 0f;
            if (rumbleInfo.EasyFunctionType == EasyFunctionType.CustomFunction)

                motor1Intensity = rumbleInfo.AnimationCurve1.Evaluate(interpolation);
            else
                motor1Intensity = EasyFunctions.CalculateInterpolation(rumbleInfo.EasyFunctionType, interpolation);

            if (rumbleInfo.EasyFunctionType2 == EasyFunctionType.CustomFunction)
                motor2Intensity = rumbleInfo.AnimationCurve2.Evaluate(interpolation);
            else
                motor2Intensity = EasyFunctions.CalculateInterpolation(rumbleInfo.EasyFunctionType2, interpolation);
            if (rumbleInfo.InverseEasyFunction)
                motor1Intensity = 1f - motor1Intensity;
            if (rumbleInfo.InverseEasyFunction2)
                motor2Intensity = 1f - motor2Intensity;
            //Debug.Log("Interpolated " + rumbleInfo.Motor1 * motor1Intensity + " " + rumbleInfo.Motor2 * motor2Intensity);
            gamepad.SetMotorSpeeds(rumbleInfo.Motor1*motor1Intensity, rumbleInfo.Motor2*motor2Intensity);
        }
    }

    private void OnDestroy()
    {
        StopAllRumbles();
    }

    protected void RumblePositional(Gamepad gamepad, RumbleInfo rumbleInfo, bool background)
    {
        if (gamepad == null)
            return;
        if (!UpdateTimeAndCheckFinish(gamepad, rumbleInfo))
        {
            Vector2 motorIntensity = CalculatePositionalMotorIntensity(rumbleInfo);
            if (rumbleInfo.IsInfinite && rumbleInfo.IsFadingOut)
            {
                motorIntensity.x = Mathf.Lerp(motorIntensity.x, 0f, rumbleInfo.InterpolateFadeOutTime);
                motorIntensity.y = Mathf.Lerp(motorIntensity.y, 0f, rumbleInfo.InterpolateFadeOutTime);
            }
            if(background)
            {
                if (_backgroundRumbleAnimations.ContainsKey(gamepad))
                {
                    RumbleAnimationTime rmi = _backgroundRumbleAnimations[gamepad];
                    rmi.SpendTime(Time.deltaTime);
                    motorIntensity = rmi.CalculateRumbleIntensity(motorIntensity);
                }
            }
            gamepad.SetMotorSpeeds(motorIntensity.x, motorIntensity.y);
        }
    }
}
