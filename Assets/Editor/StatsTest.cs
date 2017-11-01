using NUnit.Framework;

namespace TalesOfAscaria 
{
	public class StatsTest
	{
	  [Test]
	  public void CanIncreaseStatsAccordingToGrowth()
	  {
	    Stats stats = new Stats();
      stats.Awake();
      //Augmenter la force de 2
      stats.IncreaseBaseStatsAccordingToGrowth(2,0,0,0,0,0,0);
      Assert.AreEqual(2, stats.GetStatsSnapshot().Strength);
	  }

	  [Test]
	  public void StatsAreNeverNegative()
	  {
	    Stats stats = new Stats();
	    stats.Awake();
      stats.AddAbsoluteBonus(new AbsoluteStatBonus(-int.MaxValue,0,0,0,0,0,0,0));
      //
      Assert.IsFalse(stats.GetStatsSnapshot().Strength < 0);
    }

    [Test]
	  public void DexterityCannotExceedCap()
	  {
	    Stats stats = new Stats();
	    stats.Awake();
	    stats.AddAbsoluteBonus(new AbsoluteStatBonus(0, 0, 0, 0, 0, 66666, 0, 0));
      Assert.IsFalse(stats.GetStatsSnapshot().Dexterity> Stats.MaximumDexterity);
    }

	  [Test]
	  public void StatBonusesCanBeRemoved()
	  {
	    Stats stats = new Stats();
	    stats.Awake();
	    AbsoluteStatBonus bonus = new AbsoluteStatBonus(60, 0, 0, 0, 0, 0, 0, 0);
      stats.AddAbsoluteBonus(bonus);
	    float statAfterBonus = stats.GetStatsSnapshot().Strength;
      stats.RemoveAbsoluteBonus(bonus);
      Assert.IsFalse(stats.GetStatsSnapshot().Strength == statAfterBonus);
	  }
  }
}