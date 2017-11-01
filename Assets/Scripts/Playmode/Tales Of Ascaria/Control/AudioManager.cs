using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
  /// <summary>
  /// Représente un controleur de son. Singleton.
  /// </summary>
  [AddComponentMenu("Audio/AudioManager")]
	public sealed class AudioManager : GameScript
  {
    private static AudioManager instance;

    [SerializeField]
    [Tooltip("Le gameobject qui sera instancié pour un son local (3D)")]
    private GameObject localGameObject;
    [SerializeField]
    [Tooltip("Le gameobject qui sera instancié pour un son global (2D)")]
    private GameObject globalGameObject;

    private List<AudioSource> sources;

    private void Awake()
    {
      instance = this;
      sources = new List<AudioSource>();
    }

    /// <summary>
    /// Retourne l'instance active du audiomanager
    /// </summary>
    /// <returns>L'instance</returns>
    public static AudioManager GetInstance()
    {
      if (instance == null)
      {
        Debug.LogError("L'instance est nulle");
      }
      return instance;
    }

    #region Methods

    /// <summary>
    /// Joue un son globalement
    /// </summary>
    /// <param name="clip">Le clip audio</param>
    public void PlayGlobal(AudioClip clip, float volume = 1f, bool loop = false)
    {
      GameObject clone = Instantiate(globalGameObject, Vector3.zero, Quaternion.identity);
      AudioSource source = clone.GetComponent<AudioSource>();
      source.clip = clip;
      source.volume = volume;
      source.loop = loop;
      source.Play();
      sources.Add(source);
      source.GetComponent<DestroyOnSoundEnd>().OnSoundDestroyed += HandleSoundEnd;
    }

    /// <summary>
    /// Joue un son localement à l'endroit spécifié
    /// </summary>
    /// <param name="clip">Le clip audio</param>
    /// <param name="position">La position de la source audio</param>
    /// <param name="range">La portée du son jouée</param>
    public void PlayLocal(AudioClip clip, Vector2 position, float range, float volume = 1f, bool loop = false)
    {
      GameObject clone = Instantiate(localGameObject, position, Quaternion.identity);
      AudioSource source = clone.GetComponent<AudioSource>();
      source.clip = clip;
      source.volume = volume;
      source.loop = loop;
      source.minDistance = 0.25f;
      source.maxDistance = range;
      source.Play();
      sources.Add(source);
      source.GetComponent<DestroyOnSoundEnd>().OnSoundDestroyed += HandleSoundEnd;
    }
    /// <summary>
    /// Joue un son localement à l'endroit spécifié. Le son suivra le transform, sans y être dépendant
    /// </summary>
    /// <param name="clip">Le clip audio</param>
    /// <param name="parent">Le parent à suivre</param>
    /// <param name="range">La portée du son jouée</param>
    public void PlayLocal(AudioClip clip, Transform parent, float range, float volume = 1f, bool loop = false)
    {
      GameObject clone = Instantiate(localGameObject, parent.position, Quaternion.identity);
      AudioSource source = clone.GetComponent<AudioSource>();
      source.clip = clip;
      source.volume = volume;
      source.loop = loop;
      source.minDistance = 0.25f;
      source.maxDistance = range;
      source.GetComponent<FollowTransform>().enabled = true;
      source.GetComponent<FollowTransform>().target = parent;
      source.Play();
      sources.Add(source);
      source.GetComponent<DestroyOnSoundEnd>().OnSoundDestroyed += HandleSoundEnd;
    }

    /// <summary>
    /// Retourne tous les sources audio existantes, actives ou en pause
    /// </summary>
    /// <returns>Toutes les sources</returns>
    public AudioSource[] GetAllSounds()
    {
      return sources.ToArray();
    }

    /// <summary>
    /// Retourne les sources audio qui jouent le clip passé en paramètre.
    /// Retournera un tableau de taille 0 si rien n'est trouvé
    /// </summary>
    /// <param name="clip">Le clip audio</param>
    /// <returns>La source jouant le clip. Peut être nul</returns>
    public AudioSource[] GetSoundplayersForClip(AudioClip clip)
    {
      List<AudioSource> valeurRetour = new List<AudioSource>();
      foreach (AudioSource audioSource in sources)
      {
        if (audioSource.clip == clip)
        {
          valeurRetour.Add(audioSource);
        }
      }
      if(valeurRetour.Count == 0) return new AudioSource[0];
      return valeurRetour.ToArray();
    }

    #endregion

    /// <summary>
    /// Retire la source lorsqu'elle se termine
    /// </summary>
    /// <param name="audioSource">La source terminée</param>
    private void HandleSoundEnd(AudioSource audioSource)
    {
      sources.Remove(audioSource);
    }


  }
}