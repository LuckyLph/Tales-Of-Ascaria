using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Harmony;

namespace TalesOfAscaria
{
  /// <summary>
  /// Joue un son au Start
  /// </summary>
  public class PlaySoundOnAwake : GameScript
  {
    private AudioManager audioManager;

    public enum SoundType
    {
      Local,
      Global
    }

    [Tooltip("Le clip audio qui sera joué")] [SerializeField] public AudioClip audioClip;

    [Tooltip("Le type de son")] [SerializeField] public SoundType soundType;

    [Tooltip("La portée du son")] [SerializeField] public float range;

    [Tooltip("Le volume du son")] [SerializeField] public float volume;

    [Tooltip("Le son va-t-il loop?")] [SerializeField] public bool looping;

    [Tooltip("Le son va-t-il suivre l'objet?")] [SerializeField] public bool followObject;


    private void Awake()
    {
      InjectDependencies("InjectPlaySoundOnAwake");     
    }

    private void Start()
    {
      SpawnSound();
    }

    /// <summary>
    /// Créer le son lui-même
    /// </summary>
    private void SpawnSound()
    {
      if (soundType == SoundType.Global)
      {
        audioManager.PlayGlobal(audioClip,volume,looping);
      }
      else
      {
        if (followObject)
        {
          audioManager.PlayLocal(audioClip,transform,range,volume,looping);
        }
        else
        {
          audioManager.PlayLocal(audioClip, transform.position, range, volume, looping);
        }
      }
    }

    private void InjectPlaySoundOnAwake([ApplicationScope] AudioManager audioManager)
    {
      this.audioManager = audioManager;
    }

    private void OnDrawGizmosSelected()
    {
      if (soundType == SoundType.Local)
      {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
      }
    }
  }

  #region CustomInspector

  [CustomEditor(typeof(PlaySoundOnAwake))]
  public class SimpleAudioPlayerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      PlaySoundOnAwake playSoundOnAwake = target as PlaySoundOnAwake;

      playSoundOnAwake.audioClip = EditorGUILayout.ObjectField(new GUIContent("Audio Clip", "Le clip qui sera joué"),
        playSoundOnAwake.audioClip, typeof(AudioClip), false,
        null) as AudioClip;

      playSoundOnAwake.soundType =
        (PlaySoundOnAwake.SoundType) EditorGUILayout.EnumPopup(
          new GUIContent("Sound Type", "Le type de source instanciée"),
          playSoundOnAwake.soundType);

      if (playSoundOnAwake.soundType == PlaySoundOnAwake.SoundType.Local)
      {
        playSoundOnAwake.range = EditorGUILayout.FloatField(new GUIContent("Range", "La portée du son"),
          playSoundOnAwake.range);
      }

      playSoundOnAwake.volume =
        EditorGUILayout.FloatField(new GUIContent("Volume", "Le volume du son, entre 0f et 1f"),
          playSoundOnAwake.volume);


      playSoundOnAwake.looping = EditorGUILayout.Toggle(new GUIContent("Looping", "Détermine si le clip loopera"),
        playSoundOnAwake.looping);

      playSoundOnAwake.followObject = EditorGUILayout.Toggle(new GUIContent("Follow Object", "Détermine si le clip suivra l'objet"),
        playSoundOnAwake.followObject);
    }
  }

  #endregion
}