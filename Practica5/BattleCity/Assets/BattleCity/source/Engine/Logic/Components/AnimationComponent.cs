using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Animation component.Gestor de animaciones Legacy asociado a un GameObject que nos permite manipularlas de forma sencilla.
/// Su principal virtud es que nos avisa cuando temrina una animacion, para poder encadenar una accion que este sincronizada con la misma.
/// </summary>
public class AnimationComponent : MonoBehaviour {
	public enum TAnimationState {PLAYING, PAUSED, STOP};
	//Algunas constantes utiles para la velocidad de reproduccion de una animacion.
	public const float NORMAL = 1f;
	public const float X2 = 2f;
	public const float BACKWARD = -1f;
	public const float BACKWARD_X2 = -2f;
	public const float SLOW_MOTION_X2 = 0.5f;
	public const float BACKWARD_SLOW_MOTION_X2 = -0.5f;
	
	protected struct TAnimationData
	{
		public string AnimationName;
		public bool EndFeedback;
	}
	//PlayAnim: ejecutamos una animacion. Podemos configurar su velocidad de reproduccion y si queremos o no que nos avisen...
	//Nos avisaran con el mensaje "OnAnimationFinish" y el nombre de la ainmacion que ha fionalizado.
	public void PlayAnim(string animationName, float animationSpeed = 1f, bool endFeedback = false)
	{
		if(!GetComponent<Animation>().IsPlaying(animationName))
		{
			GetComponent<Animation>()[animationName].speed = animationSpeed;
			GetComponent<Animation>().Play(animationName);
			TAnimationData data = new TAnimationData();
			data.AnimationName = animationName;
			data.EndFeedback = endFeedback;
			StartCoroutine("CheckingAnimationFinish",data);
		}
	}
	//paramos una animacion.
	public void StopAnimation(string animationName)
	{
		GetComponent<Animation>().Stop(animationName);
	}
	//Dejamos en pausa una animacion
	public void PauseAnimation(string animationName)
	{
		GetComponent<Animation>()[animationName].enabled = false;
	}
	//volvemos a ejecutar una animacion.
	public void ResumeAnimation(string animationName)
	{
		GetComponent<Animation>()[animationName].enabled = true;
	}
	//GetState, Testeamos el estado de de una animacion.
	public TAnimationState GetState(string animationName)
	{
		if (GetComponent<Animation>()[animationName].enabled && GetComponent<Animation>().IsPlaying(animationName))
		{
			//Playing
			return TAnimationState.PLAYING;
		}
		else if (!GetComponent<Animation>()[animationName].enabled)
		{
			//pause
			return TAnimationState.PAUSED;
		}
		else
		{
			//stop
			return TAnimationState.STOP;
		}
	}
	//Comprobamos el tiempo que le queda a la animacion para que termine.
	public float GetTimeToFinish(string animationName)
	{
		return GetComponent<Animation>()[animationName].length -  GetComponent<Animation>()[animationName].time;
	}
	//Obtenermos el que lleva en reproduccion.
	public float GetTimePlaying(string animationName)
	{
		return GetComponent<Animation>()[animationName].time;
	}
	//Obtiene la duracion de una animacion
	public float GetDuration(string animationName)
	{
		return GetComponent<Animation>()[animationName].length;
	}
	
	//Corutina que gestiona la animaciones que han sido lanzadas para que al temrinar nos avisen.
	IEnumerator CheckingAnimationFinish(TAnimationData data)
	{
		while(this.GetComponent<Animation>().IsPlaying(data.AnimationName))
		{
			yield return new WaitForSeconds(this.GetComponent<Animation>()[data.AnimationName].length - this.GetComponent<Animation>()[data.AnimationName].time);
		}
		if(data.EndFeedback)
		{
			SendMessage("OnAnimationFinish",data.AnimationName,SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
