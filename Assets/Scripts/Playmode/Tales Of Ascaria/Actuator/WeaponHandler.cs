using UnityEngine;

namespace TalesOfAscaria
{
    public class WeaponHandler : MonoBehaviour
    {
        private IWeapon weapon;


        void Awake()
        {
            //TODO: s'abonner a l'evenement OnChangeWeapon
        }

        /// <summary>
        /// Utilise l'action principale de l'arme
        /// </summary>
        public void UsePrimary()
        {
            weapon.DoPrimary();
        }

        /// <summary>
        /// Utilise l'action secondaire de l'arme
        /// </summary>
        public void UseSecondary()
        {
            weapon.DoSecondary();
        }
    }

    public interface IWeapon
    {
        /// <summary>
        /// Utilise l'action primaire
        /// </summary>
        void DoPrimary();

        /// <summary>
        /// Utilise l'action secondaire
        /// </summary>
        void DoSecondary();
    }
}