using NUnit.Framework;

namespace TalesOfAscaria 
{
  public class StatsSnapshotTest
  {
    [Test]
    public void CanAddStatModifier()
	  {
	    StatsSnapshot statsSnapshot = new StatsSnapshot(1,1,1,1,1,1,1,1);
      //Multiplicateur de force de 2
      StatMultiplierBonus bonus = new StatMultiplierBonus(2);
	    statsSnapshot *= bonus;
      Assert.AreEqual(2,statsSnapshot.Strength);
	  }

	  [Test]
	  public void CanAddAbsoluteStatBonus()
	  {
	    StatsSnapshot statsSnapshot = new StatsSnapshot(1, 1, 1, 1, 1, 1, 1, 1);
	    //Ajouter 2 force
      AbsoluteStatBonus bonus = new AbsoluteStatBonus(2,0,0,0,0,0,0,0);
	    statsSnapshot += bonus;
      Assert.AreEqual(3,statsSnapshot.Strength);
	  }

	  [Test]
	  public void CanAddStatSnapshot()
	  {
	    StatsSnapshot statsSnapshot = new StatsSnapshot(1, 1, 1, 1, 1, 1, 1, 1);
	    //Statsnapshot avec 2 force ajouté
      StatsSnapshot statsSnapshot2 = new StatsSnapshot(2,0,0,0,0,0,0,0);
	    statsSnapshot += statsSnapshot2;
      Assert.AreEqual(3,statsSnapshot.Strength);
	  }
  }
}