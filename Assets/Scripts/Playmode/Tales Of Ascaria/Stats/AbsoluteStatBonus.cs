namespace TalesOfAscaria
{
    /// <summary>
    /// Représente un bonus en valeur absolu des statistiques
    /// </summary>
    public class AbsoluteStatBonus
    {
        public int BonusStrength { get; private set; }
        public int BonusWisdom { get; private set; }
        public int BonusConstitution { get; private set; }
        public int BonusSpirit { get; private set; }
        public int BonusAgility { get; private set; }
        public int BonusDexterity { get; private set; }
        public int BonusHealthRegen { get; private set; }
        public int BonusManaRegen { get; private set; }

        public AbsoluteStatBonus(int bonusStrength,
                                 int bonusWisdom,
                                 int bonusConstitution,
                                 int bonusSpirit,
                                 int bonusAgility,
                                 int bonusDexterity,
                                 int bonusHealthRegen,
                                 int bonusManaRegen)
        {
            BonusStrength = bonusStrength;
            BonusWisdom = bonusWisdom;
            BonusConstitution = bonusConstitution;
            BonusSpirit = bonusSpirit;
            BonusAgility = bonusAgility;
            BonusDexterity = bonusDexterity;
            BonusHealthRegen = bonusHealthRegen;
            BonusManaRegen = bonusManaRegen;
        }
    }
}