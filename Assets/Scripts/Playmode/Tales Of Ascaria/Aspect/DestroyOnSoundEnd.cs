using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
  /// <summary>
  /// Détruit le gameobject lorsque le son est terminé
  /// </summary>
  [AddComponentMenu("Aspect/DestroyOnSoundEnd")]
	public sealed class DestroyOnSoundEnd : GameScript
  {
    public delegate void DestroyedEventHandler(AudioSource audioSource);

    public event DestroyedEventHandler OnSoundDestroyed;


	  private AudioSource audioSource;

		private void Awake() 
		{
		    InjectDependencies("InjectDestroySoundOnEnd");
		}

	  private void Start()
	  {
	    if (!audioSource.loop)
	    {
	      Destroy(transform.root.gameObject, audioSource.clip.length - audioSource.time);
	    }
	  }

	  private void InjectDestroySoundOnEnd([EntityScope] AudioSource audioSource)
	  {
	    this.audioSource = audioSource;
	  }

    private void OnDestroy()
    {
      if (OnSoundDestroyed != null) OnSoundDestroyed(audioSource);
    }


  }
}