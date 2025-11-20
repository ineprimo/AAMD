using UnityEngine;
using UnityEngine.UI;

public class GUIControllerBase : MonoBehaviour
{
    public GameObject _guiParent;
    public SavingInfo _saving;
    public Image _fusedImage;
    public AchievementDisplay achievementDisplay;

    public void ShowGUI(bool b)
    {
        _guiParent.SetActive(b);
    }

    public bool IsFuseActive
    {
        get
        {
            return _fusedImage.gameObject.activeInHierarchy;
        }

    }
    public void ShowSaving()
    {
        if (!IsFuseActive)
            _saving.Show();
    }

    public void ShowAchievementPanel(string t, Sprite i)
    {
        if (achievementDisplay != null)
            achievementDisplay.Show(t, i);
    }
}