using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/World/Object/Stimulus/InteractionStimulus")]
  public class InteractionStimulus : GameScript
  {
    private List<InteractionSensor> sensors;
    private XInputDotNetPure.PlayerIndex index;
    private PlayerController playerController;
    
    private void InjectInteractionStimulus([EntityScope] PlayerController playerController)
    {
      this.playerController = playerController;
    }

    private void Awake()
    {
      InjectDependencies("InjectInteractionStimulus");
      sensors = new List<InteractionSensor>();
      index = playerController.Index;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
      InteractionSensor potentialSensor = collider.gameObject.GetComponentInChildren<InteractionSensor>();
      if (potentialSensor != null)
      {
        sensors.Add(potentialSensor);
      }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
      if (sensors.Count > 0)
      {
        for (int i = 0; i < sensors.Count; i++)
        {
          if (sensors[i].PlayerIndex == index)
          {
            sensors[i].HasExitedInteraction = true;
          }
        }
        sensors.Remove(collider.gameObject.GetComponentInChildren<InteractionSensor>());
      }
    }

    public void Interact()
    {
      InteractionSensor sensor;
      if (sensors.Count > 0)
      {
        sensor = sensors[0];
        if (sensors.Count > 1)
        {
          for (int i = 1; i < sensors.Count; i++)
          {
            if (Vector2.Distance(transform.parent.position, sensors[i].transform.parent.position) <
                Vector2.Distance(transform.parent.position, sensor.transform.parent.position))
            {
              sensor = sensors[i];
            }
          }
        }
        sensor.ReceiveInteraction(index);
      }
    }

    public void ResetSensors()
    {
      sensors.Clear();
    }
  }
}