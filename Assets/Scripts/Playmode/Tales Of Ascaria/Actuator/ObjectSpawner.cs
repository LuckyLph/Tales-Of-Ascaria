using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class ObjectSpawner : GameScript
	{
    [Tooltip("Les gameobject qui seront instanciés")]
	  [SerializeField] private GameObject[] objectsToSpawn;

	  public void SpawnObjects()
	  {
	    if (objectsToSpawn != null && objectsToSpawn.Length > 0)
	    {
	      for (int i = 0; i < objectsToSpawn.Length; i++)
	      {
	        Instantiate(objectsToSpawn[i], transform.position, Quaternion.identity);
        }
	    }
	  }
	}
}