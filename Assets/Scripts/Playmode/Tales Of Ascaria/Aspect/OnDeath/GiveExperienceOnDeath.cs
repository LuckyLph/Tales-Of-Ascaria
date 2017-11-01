using Harmony;
using UnityEngine;

namespace TalesOfAscaria 
{
	public class GiveExperienceOnDeath : GameScript
	{
    [Tooltip("Nombre d'expérience donné au joueur lorsque l'entité meurt")]
	  [SerializeField] private int experienceValue;

	  private PlayersList playersList;

	  private void InjectGiveExperienceOnDeath([ApplicationScope] PlayersList playersList)
	  {
	    this.playersList = playersList;
	  }

		private void Awake() 
		{
      InjectDependencies("InjectGiveExperienceOnDeath");
		}

	  private void OnDestroy()
	  {
	    for (int i = 0; i < playersList.PlayersAlive.ToArray().Length; i++)
	    {
	      if (playersList.PlayersAlive.ToArray()[i] != null)
	      {
	        playersList.PlayersAlive.ToArray()[i].GetComponentInChildren<LivingEntity>().AddExperience(experienceValue);
        } 
	    }
	  }
	}
}