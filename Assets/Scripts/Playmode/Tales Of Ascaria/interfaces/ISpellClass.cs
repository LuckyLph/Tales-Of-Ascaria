using UnityEngine;

namespace TalesOfAscaria
{
  public interface ISpellClass
  {
    void SpellA(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
    void SpellB(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
    void SpellX(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
    void SpellY(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
  }

  /// <summary>
  /// Inclut les méthodes requises pour utiliser une arme in-game
  /// </summary>
  public interface IWeaponHandler
  {
    void PrimaryAction(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
    void SecondaryAction(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition);
  }
}
