using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    [Tooltip("identificador del logro interno")]
    public string id;
    [Header("Title")]
    public LocalizeInspectorPopup titleKey;
    [Header("Description")]
    public LocalizeInspectorPopup descriptionKey;
    public Sprite image;
    public int points;
    [Tooltip("Dificultad estimada desde Diseño del logro, entre 1 y 5. Solo como referencia.")]
    public int estimatedDifficulty;
    [Tooltip("Cantidad necesaria de lo que sea para desbloquear el logro.")]
    public int amount;
}
[CreateAssetMenu(fileName = "Achievements", menuName = "8Picaros/Config/Achievements")]
public class Achievements: ScriptableObject
{
    public bool showAchievementInGame;
    public Achievement[] achievementsList;
}
