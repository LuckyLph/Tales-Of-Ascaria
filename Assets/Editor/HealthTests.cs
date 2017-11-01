using NUnit.Framework;

namespace TalesOfAscaria
{
  public class HealthTests
  {
    [Test]
    public void ConstructorMockTest()
    {
      HealthMock health = new HealthMock(10, 10);
      Assert.IsTrue(health.HealthPoints == 10);
      Assert.IsTrue(health.MaximumHealthPoints == 10);
    }

    [Test]
    public void Awake_NormalHealthParameters_ShouldHaveCorrectValues()
    {
      HealthMock health = new HealthMock(10, 0);
      Assert.IsTrue(health.HealthPoints == 0);
      health.Awake();
      Assert.IsTrue(health.HealthPoints == 10);
    }

    [Test]
    public void Awake_ZeroMaximumHealth_ShouldHaveCorrectValues()
    {
      HealthMock health = new HealthMock(0, 0);
      Assert.IsTrue(health.HealthPoints == 0);
      health.Awake();
      Assert.IsTrue(health.HealthPoints == 0);
    }

    [Test]
    public void Awake_NegativeHealth_ShouldHaveZeroHealth()
    {
      HealthMock health = new HealthMock(-10, 0);
      health.Awake();
      Assert.IsTrue(health.HealthPoints == 0);
      Assert.IsTrue(health.MaximumHealthPoints == 0);
    }

    [Test]
    public void RegainHealth_NormalValues_ShouldMaxOutHealthPoints()
    {
      HealthMock health = new HealthMock(10, 0);
      health.RegainHealth();
      Assert.IsTrue(health.HealthPoints == 10);
    }

    [Test]
    public void RegainHealth_HigherHealthThanMaxHealth_ShouldLowerHealthToMaxHealth()
    {
      HealthMock health = new HealthMock(5, 10);
      health.RegainHealth();
      Assert.IsTrue(health.HealthPoints == 5);
    }

    [Test]
    public void ChangeMaxHealth_PositiveValue_ShouldIncreaseMaxHealthAndHealth()
    {
      HealthMock health = new HealthMock(10, 5);
      health.ChangeMaxHealth(5);
      Assert.IsTrue(health.HealthPoints == 10);
      Assert.IsTrue(health.MaximumHealthPoints == 15);
    }

    [Test]
    public void ChangeMaxHealth_NegativeValueNotLowerThanCurrentHealth_ShouldReduceMaxHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.ChangeMaxHealth(-5);
      Assert.IsTrue(health.HealthPoints == 5);
      Assert.IsTrue(health.MaximumHealthPoints == 10);
    }

    [Test]
    public void ChangeMaxHealth_NegativeValueLowerThanCurrentHealth_ShouldReduceMaxHealthAndResetHealth()
    {
      HealthMock health = new HealthMock(15, 15);
      health.ChangeMaxHealth(-5);
      Assert.IsTrue(health.HealthPoints == 10);
      Assert.IsTrue(health.MaximumHealthPoints == 10);
    }

    [Test]
    public void ChangeMaxHealth_ValueZero_ShouldRemainTheSame()
    {
      HealthMock health = new HealthMock(15, 15);
      health.ChangeMaxHealth(0);
      Assert.IsTrue(health.HealthPoints == 15);
      Assert.IsTrue(health.MaximumHealthPoints == 15);
    }


    [Test]
    public void Heal_SumOfHealthAndValueLowerThanMaxHealth_ShouldIncreaseHealthByValue()
    {
      HealthMock health = new HealthMock(15, 10);
      health.Heal(3.0f);
      Assert.IsTrue(health.HealthPoints == 13);
    }


    [Test]
    public void Heal_SumOfHealthAndValueGreaterThanMaxHealth_ShouldIncreaseHealthToMaxHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.Heal(10);
      Assert.IsTrue(health.HealthPoints == 15);
    }

    [Test]
    public void Heal_ValueIsZero_ShouldNotChangeHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.Heal(0);
      Assert.IsTrue(health.HealthPoints == 10);
    }

    [Test]
    public void Heal_NegativeValue_ShouldNotChangeHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.Heal(-10);
      Assert.IsTrue(health.HealthPoints == 10);
    }

    [Test]
    public void TakeDamage_ValueIsLowerThanHealth_ShouldReduceHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.TakeDamage(5);
      Assert.IsTrue(health.HealthPoints == 5);
    }

    [Test]
    public void TakeDamage_ValueIsHigherThanHealth_ShouldCapAtZero()
    {
      HealthMock health = new HealthMock(15, 10);
      health.TakeDamage(15);
      Assert.IsTrue(health.HealthPoints == 0);
    }

    [Test]
    public void TakeDamage_NegativeValue_ShouldNotChangeHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.TakeDamage(-15);
      Assert.IsTrue(health.HealthPoints == 10);
    }

    [Test]
    public void TakeDamage_ValueIsZero_ShouldNotChangeHealth()
    {
      HealthMock health = new HealthMock(15, 10);
      health.TakeDamage(0);
      Assert.IsTrue(health.HealthPoints == 10);
    }
  }
}