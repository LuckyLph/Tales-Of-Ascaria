using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class SceneSwitchedEventPublisher : GameScript 
	{
    private SceneSwitchedEventChannel channel;

    private void InjectSceneSwitchedEventPublisher([EventChannelScope] SceneSwitchedEventChannel channel)
    {
      this.channel = channel;
    }

    private void Awake() 
		{
      InjectDependencies("InjectSceneSwitchedEventPublisher");
		}

    public void Publish()
    {
      channel.Publish(new SceneSwitchedEvent());
    }
	}
}