using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MusicResourceInfo
{
    [Tooltip("Fichero a cargar desde resources")]
    public string fileName;
    [Tooltip("Nombre que utilizaremos para invocarlo")]
    public string name;
    //pensar si poner el clip de audio o no.
    //Ventaja de poner el clip de audio, las build no incluyen la musica que no necesitamos el el proyecto
    //Desventaja de poner el clip de audio, que el clip de audio está en memoria todo el tiempo.
    [Tooltip("Nombre de la escena donde se reproducirá el sonido")]
    public string sceneName;
    [Tooltip("Se reproduce en loop o no")]
    public bool loop;
    [Tooltip("Volumen del clip")]
    public float volume = 1f;
}
[CreateAssetMenu(fileName = "SoundResources", menuName = "8Picaros/Config/SoundResources")]
public class SoundResources : ScriptableObject
{
    public MusicResourceInfo[] musicResourceInfo;

    public int maxSounds = 2;
    public int maxLoopFX = 2;
    public int maxFX = 2;
}
