namespace TalesOfAscaria
{
    /// <summary>
    /// Représente un multiplicateur de statistiques.
    /// </summary>
    public class StatMultiplierBonus
    {
        public float StrengthModifier { get; set; }
        public float WisdomModifier { get; set; }
        public float ConstitutionModifier { get; set; }
        public float SpiritModifier { get; set; }
        public float AgilityModifier { get; set; }
        public float DexterityModifier { get; set; }
        public float HealthRegenModifier { get; set; }
        public float ManaRegenModifier { get; set; }
        public float FinalDamageDealingModifier { get; set; }
        public float FinalDamageRecievedModifier { get; set; }

        public StatMultiplierBonus(float strengthModifier = 1,
                                   float wisdomModifier = 1,
                                   float constitutionModifier = 1,
                                   float spiritModifier = 1,
                                   float agilityModifier = 1,
                                   float dexterityModifier = 1,
                                   float healthRegenModifier = 1,
                                   float manaRegenModifier = 1,
                                   float damageDealingModifier = 1,
                                   float damageReductionModifier = 1)
        {
            StrengthModifier = strengthModifier;
            WisdomModifier = wisdomModifier;
            ConstitutionModifier = constitutionModifier;
            SpiritModifier = spiritModifier;
            AgilityModifier = agilityModifier;
            DexterityModifier = dexterityModifier;
            HealthRegenModifier = healthRegenModifier;
            ManaRegenModifier = manaRegenModifier;
            FinalDamageDealingModifier = damageDealingModifier;
            FinalDamageRecievedModifier = damageReductionModifier;
        }
    }
}