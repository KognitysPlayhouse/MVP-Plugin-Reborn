using System;
using System.Linq;
using Exiled.API.Features;
using UnityEngine;

namespace MVP_Plugin_Reborn
{

	public class MVP_Plugin_Reborn : Plugin<Config>
	{
		public static MVP_Plugin_Reborn MVP_Plugin_RebornRef { get; private set; }
		public override string Name => nameof(MVP_Plugin_Reborn);
		public override string Author => "Kognity";
		public EventHandler Handler;

		public MVP_Plugin_Reborn()
		{
			MVP_Plugin_RebornRef = this;
		}

		public override void OnEnabled()
		{
			if (!MVP_Plugin_RebornRef.Config.IsEnabled)
			{
				Log.Info("MVP Plugin Reborn Disabled");
				return;
			}

			Handler = new EventHandler(this);
			Exiled.Events.Handlers.Player.Died += Handler.OnPlayerDeath;
			Exiled.Events.Handlers.Server.EndingRound += Handler.OnRoundEnd;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension += Handler.PDEscapeFail;
		}

		public override void OnDisabled()
		{
			Exiled.Events.Handlers.Player.Died -= Handler.OnPlayerDeath;
			Exiled.Events.Handlers.Server.EndingRound -= Handler.OnRoundEnd;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= Handler.PDEscapeFail;
			Handler = null;
		}

		public override void OnReloaded() { }
	}
}