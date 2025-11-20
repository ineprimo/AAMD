using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMgrBase
{
    private List<string> _achievementsCompleted;
    private int _slotLoaded = -1;
    public bool IsBindingGameToAnySlot { get { return _slotLoaded >= 0; } }
    public string[] _slots;

    public PlayersMgrBase(string[] slots)
    {
        _slots = slots;
    }
    public List<string> AchievementsCompleted
    {
        get
        {
            return _achievementsCompleted;
        }
    }

    public virtual string[] SLOTS
    {
        get
        {
            return _slots;
        }
    }

    public void SetAchievementsCompleted(string id)
    {
        _achievementsCompleted.Add(id);
    }

    public bool IsAchievementCompleted(string id)
    {
        if (_achievementsCompleted == null)
            return true;
        return _achievementsCompleted.Contains(id);
    }

    public void SaveAchivementOnly()
    {
        if (IsBindingGameToAnySlot) //no guardamos datos de partidas de prueba.
            SaveAchievementOnly(_slotLoaded);
    }

    public void SaveAchievementOnly(int slot)
    {
        GUIControllerBase guiController = GameMgr.Instance.GetGUIController();
        if (guiController != null)
            guiController.ShowSaving();

        StorageMgr sm = GameMgr.Instance.GetStorageMgr();
        Debug.Assert(slot >= 0 && slot < SLOTS.Length, "El ínidce del slot no es correcto " + slot);
        SaveAchievents(GetAchievementFileName(slot));

    }

    public static string PlayerMgrSection
    {
        get
        {
            return typeof(PlayersMgrBase).ToString();
        }
    }

    protected void SaveAchievents(string fileName)
    {
        _SaveAchievents();
        StorageMgr sm = GameMgr.Instance.GetStorageMgr();
        sm.SetInSerialized("achievements", PlayerMgrSection, "_achievementsCompleted", Utils.ConvertStringArrayToString(_achievementsCompleted.ToArray()));
        sm.StoreSerialized("achievements", fileName);
    }

    protected virtual void _SaveAchievents()
    {

    }

    public string GetAchievementFileName(int slot)
    {
        return GetUserID(slot) + "_achievement";
    }
    public string GetUserID(int slot)
    {
#if PLATFORM_STANDALONE
        return SLOTS[slot];
#elif UNITY_PS4
#elif UNITY_PS5
#elif UNITY_XBOXONE
#elif UNITY_SWITCH
#endif
    }
}
