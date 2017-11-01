using UnityEngine;

namespace TalesOfAscaria
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void ApplyOn(LivingEntity entity);
    }
}