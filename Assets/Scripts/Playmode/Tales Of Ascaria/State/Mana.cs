using UnityEngine;

namespace TalesOfAscaria
{
    /// <summary>
    /// Représente la mana de l'entité
    /// </summary>
    public class Mana : GameScript
    {
        public delegate void ManaChangedHandler(int remainingMana);

        /// <summary>
        /// Lorsque la mana à été modifiée
        /// </summary>
        public event ManaChangedHandler OnManaChanged;

        /// <summary>
        /// Les points de mana actuel.
        /// </summary>
        public float ManaPoints
        {
            get { return manaPoints; }
            private set { manaPoints = value; }
        }

        /// <summary>
        /// Le maximum de vie
        /// </summary>
        public float MaximumManaPoints
        {
            get { return maximumManaPoints; }
            private set { maximumManaPoints = value; }
        }

        private float manaPoints;

        [Tooltip("Les points de mana maximum de l'entité")]
        [SerializeField]
        private float maximumManaPoints;

        void Awake()
        {
            RegainMana();
        }

        /// <summary>
        /// Redonne tous les points de mana
        /// </summary>
        void RegainMana()
        {
            ManaPoints = MaximumManaPoints;
            if (OnManaChanged != null) OnManaChanged((int) ManaPoints);
        }

        /// <summary>
        /// Augmente le mana maximal et regénère le mana actuelle du nombre de mana augmenté
        /// </summary>
        /// <param name="manaIncreased"></param>
        public void IncreaseMaxMana(float manaIncreased)
        {
            MaximumManaPoints += manaIncreased;
            ManaPoints += manaIncreased;
        }

        /// <summary>
        /// Réduit le mana selon le cout.
        /// </summary>
        /// <param name="cost">Le cout de l'action</param>
        public void UseMana(int cost)
        {
            ManaPoints -= cost;
        }

        /// <summary>
        /// Vérifie si la mana est suffisante selon le coût
        /// </summary>
        /// <param name="cost">Coût en mana du spell</param>
        /// <returns>true si il y a assez de mana, false sinon</returns>
        public bool HasEnoughMana(int cost)
        {
            if (cost > ManaPoints)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Redonne de la mana
        /// </summary>
        /// <param name="amount">Le montant à redonner</param>
        public void HealMana(int amount)
        {
            ManaPoints = Mathf.Min(ManaPoints + amount, MaximumManaPoints);
        }
    }
}