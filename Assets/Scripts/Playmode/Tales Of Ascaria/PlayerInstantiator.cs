using System;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class PlayerInstantiator : GameScript 
	{
    [Tooltip("The Guardian's prefab")]
    [SerializeField]
    private GameObject guardianPrefab;

    [Tooltip("The Ranger's prefab")]
    [SerializeField]
    private GameObject rangerPrefab;

    [Tooltip("The Mage's prefab")]
    [SerializeField]
    private GameObject magePrefab;

    [Tooltip("The Priest's prefab")]
    [SerializeField]
    private GameObject priestPrefab;

    [Tooltip("The Shotokan's prefab")]
    [SerializeField]
    private GameObject shotokanPrefab;

	  private ClassType classToInstantiate;

    public void InjectPlayerInstantiator([ApplicationScope] GameParametersFromMenu parametersFromMenu)
    {
      classToInstantiate = parametersFromMenu.InitialPlayer;
    }

	  private void Awake() 
		{
      InjectDependencies("InjectPlayerInstantiator");
		}

	  private void Start()
	  {
	    switch (classToInstantiate)
	    {
	      case ClassType.Guardian:
	        GameObject guardianClone = Instantiate(guardianPrefab, new Vector3(5.35f, 5.4f, 0), Quaternion.identity);
	        break;
	      case ClassType.Ranger:
          GameObject rangerClone = Instantiate(rangerPrefab, new Vector3(5.35f, 5.4f, 0), Quaternion.identity);
          break;
	      case ClassType.Mage:
          GameObject mageClone = Instantiate(magePrefab, new Vector3(5.35f, 5.4f, 0), Quaternion.identity);
          break;
	      case ClassType.Priest:
          GameObject priestClone = Instantiate(priestPrefab, new Vector3(5.35f, 5.4f, 0), Quaternion.identity);
          break;
	      case ClassType.Shotokan:
          GameObject shotokanClone = Instantiate(shotokanPrefab, new Vector3(5.35f, 5.4f, 0), Quaternion.identity);
          break;
	    }
      GameObject gameParameters = GameObject.FindGameObjectWithTag(R.S.GameObject.GameParameters);
      gameParameters.GetComponent<GameParametersFromMenu>().InitialPlayer = ClassType.None;
    }
	}
}