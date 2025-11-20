using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsMgr
{
    Achievements achievements;
    Dictionary<string, Achievement> achievementDictionary;

    public AchievementsMgr(Achievements a)
    {
        achievements=a;
        achievementDictionary = new Dictionary<string, Achievement>();
        if(achievements != null && achievements.achievementsList != null)
        {
            for (int i = 0; i < achievements.achievementsList.Length; i++)
            {
                achievementDictionary.Add(achievements.achievementsList[i].id, achievements.achievementsList[i]);
            }
        }
        
    }

    public Dictionary<string, Achievement> Achievements
    {
        get
        {
            return achievementDictionary;
        }
    }
    public void ShowAchievement(string id, bool saveData)
    {
        PlayersMgrBase playerMgr = GameMgr.Instance.GetPlayersMgr;
        if (achievementDictionary.ContainsKey(id) && !playerMgr.IsAchievementCompleted(id))
        {
            Achievement achievement = achievementDictionary[id];
            ShowAchievementInPlatform(achievement);
           
            playerMgr.SetAchievementsCompleted(id);
            if (saveData)
                playerMgr.SaveAchivementOnly();
        }
    }

    public void ShowAchievementIfComplete(string id, int amount, bool saveData)
    {
        PlayersMgrBase playerMgr = GameMgr.Instance.GetPlayersMgr;
        if (achievementDictionary.ContainsKey(id) && !playerMgr.IsAchievementCompleted(id))
        {
            Achievement achievement = achievementDictionary[id];
            if (achievement.amount >= amount)
            {
                ShowAchievementInPlatform(achievement);
                playerMgr.SetAchievementsCompleted(id);
                if(saveData)
                    playerMgr.SaveAchivementOnly();
            }
        }
        //TODO
    }

#if PLATFORM_STANDALONE
    protected void ShowAchievementInPlatform(Achievement achievement)
    {
        if (achievements.showAchievementInGame)
            GameMgr.Instance.GetGUIController().ShowAchievementPanel(LocalizationMgr.Instance.Translate(achievement.titleKey.key), achievement.image);
        Debug.Log("achievement id " + achievement.id + " " + LocalizationMgr.Instance.Translate(achievement.titleKey.key) + " (" + achievement.points + " points)");
    }
#elif UNITY_PS4
    protected void ShowAchievementInPlatform(Achievement achievement)
    {
        Debug.Log("achievement id " + achievement.id + " " + LocalizationMgr.Instance.Translate(achievement.titleKey.key) + " (" + achievement.points + " points)");
    }
#elif UNITY_PS5
    protected void ShowAchievementInPlatform(Achievement achievement)
    {
        Debug.Log("achievement id " + achievement.id + " " + LocalizationMgr.Instance.Translate(achievement.titleKey.key) + " (" + achievement.points + " points)");
    }
#elif UNITY_XBOXONE
    protected void ShowAchievementInPlatform(Achievement achievement)
    {
        Debug.Log("achievement id " + achievement.id + " " + LocalizationMgr.Instance.Translate(achievement.titleKey.key) + " (" + achievement.points + " points)");
    }
#elif UNITY_SWITCH
    protected void ShowAchievementInPlatform(Achievement achievement)
    {
        Debug.Log("achievement id " + achievement.id + " " + LocalizationMgr.Instance.Translate(achievement.titleKey.key) + " (" + achievement.points + " points)");
    }
#endif

}
