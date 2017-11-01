using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DevastatingLungeController : GameScript
  {
    [Tooltip("Liste des effets du high guard")]
    [SerializeField] List<Effect> devastatingLungeEffects = new List<Effect>();

    [Tooltip("Temps avant que le devastating lunge commence à exister")]
    [SerializeField] private float spellChargeTime;

    [Tooltip("Pourcentage de force de l'entité appliqué au devastating lunge pour les dommages")]
    [SerializeField] private float devastatingLungePower;

    [Tooltip("Vitesse de déplacement du lanceur de sort lors d'un devastating lunge")]
    [SerializeField] private Dash devastatingLungeDash;

    [Tooltip("Temps de déplacement du lanceur de sort lors d'un devastating lunge")]
    [SerializeField] private float devastatingLungeDuration;

    private Transform casterTransform;
    private LivingEntity casterLivingEntity;

    private HitStimulus hitStimulus;
    private new Collider2D collider2D;

    private void Awake()
    {
      InjectDependencies("InjectDevastatingLunge");
    }

    private void InjectDevastatingLunge([EntityScope] HitStimulus hitStimulus, [EntityScope] PolygonCollider2D polygonCollider2D)
    {
      collider2D = polygonCollider2D;
      this.hitStimulus = hitStimulus;
    }

    private void Start()
    {
      StartCoroutine(DoDevastatingLunge());
    }

    public IEnumerator DoDevastatingLunge()
    {
      yield return new WaitForSeconds(spellChargeTime);
      devastatingLungeDash.ApplyOn(casterLivingEntity);
      collider2D.enabled = true;
      float currentTime = Time.time;
      while (Time.time < currentTime + devastatingLungeDuration)
      {
        MoveCasterAndHitboxAsSpellIsCast();
        yield return new WaitForEndOfFrame();
      }
      Destroy(gameObject.transform.root.gameObject);
    }

    public void SetDevastatingLungeParameters(StatsSnapshot statsSnapshot, Transform casterTransform,
                                              Vector2 playerDirection, LivingEntity casterLivingEntity)
    {
      for (int i = 0; i < devastatingLungeEffects.Count; i++)
      {
        if (devastatingLungeEffects[i] is InstantDamage)
        {
          InstantDamage devastatingLungeInstantDamage = devastatingLungeEffects[i] as InstantDamage;
          devastatingLungeInstantDamage.BonusDamage = statsSnapshot.Strength * devastatingLungePower;
          devastatingLungeEffects[i] = devastatingLungeInstantDamage;
        }
        if (devastatingLungeEffects[i] is Knockback)
        {
          Knockback knockback = devastatingLungeEffects[i] as Knockback;
          knockback.KnockbackSourceDirection = casterTransform;
          devastatingLungeEffects[i] = knockback;
        }
      }
      devastatingLungeDash.DashDirection = playerDirection;
      this.casterTransform = casterTransform;
      this.casterLivingEntity = casterLivingEntity;
      transform.root.Rotate(new Vector3(0, 0, 1), Vector3.SignedAngle(playerDirection, Vector3.right, Vector3.back));
      ActorAction actorAction = new ActorAction(devastatingLungeEffects);
      hitStimulus.ActorAction = actorAction;
    }

    public void MoveCasterAndHitboxAsSpellIsCast()
    {
      transform.root.position = casterTransform.root.position;
    }
  }
}