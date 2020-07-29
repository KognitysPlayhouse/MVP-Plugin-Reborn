using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MVP_Plugin_Reborn
{
	public class EventHandler
	{
		public MVP_Plugin_Reborn plugin;
		public EventHandler(MVP_Plugin_Reborn plugin) => this.plugin = plugin;

		public List<string> playersWithKills = new List<string>();
		public List<int> playerKillNumber = new List<int>();

		public void OnPlayerDeath(DiedEventArgs ev)
		{
			var dmg = ev.HitInformations.GetDamageType();
			if (dmg == DamageTypes.Tesla || dmg == DamageTypes.Wall || dmg == DamageTypes.Contain || dmg == DamageTypes.Decont
				|| dmg == DamageTypes.Falldown || dmg == DamageTypes.Flying || dmg == DamageTypes.Nuke
				|| dmg == DamageTypes.RagdollLess || dmg == DamageTypes.Recontainment)
				return;
			if (ev.Target.Team == ev.Killer.Team)
				return;

			if (dmg == DamageTypes.Pocket)
			{
				foreach (Player Ply in Player.List)
				{
					if (Ply.Role == RoleType.Scp106)
					{
						string killer = Ply.Nickname;
						if (!playersWithKills.Contains(killer))
						{
							playersWithKills.Add(killer);
							playerKillNumber.Add(0);
						}
						playerKillNumber[playersWithKills.IndexOf(killer)] += 1;
						break;
					}
				}
			}
			else
			{

				string killer = ev.Killer.Nickname;
				if (!playersWithKills.Contains(killer))
				{
					playersWithKills.Add(killer);
					playerKillNumber.Add(0);
				}
				playerKillNumber[playersWithKills.IndexOf(killer)] += 1;
			}
			
		}

		public void PDEscapeFail(FailingEscapePocketDimensionEventArgs ev)
		{
			foreach (Player Ply in Player.List)
			{
				if (Ply.Role == RoleType.Scp106)
				{
					string killer = Ply.Nickname;
					if (!playersWithKills.Contains(killer))
					{
						playersWithKills.Add(killer);
						playerKillNumber.Add(0);
					}
					playerKillNumber[playersWithKills.IndexOf(killer)] += 1;
					break;
				}
			}
		}

		public void OnRoundEnd(EndingRoundEventArgs ev)
		{
			if (ev.IsRoundEnded)
			{
				string message = GetEndMesage();
				Map.Broadcast(10, message);
				playersWithKills = new List<string>();
				playerKillNumber = new List<int>();
			}
		}

		public int FindMVP()
		{
			if (playersWithKills.Count == 0)
				return (-1);
			int largestNumber = 0;
			for (int i = 0; i < playersWithKills.Count; i++)
			{
				if (playerKillNumber[i] > playerKillNumber[largestNumber]) { largestNumber = i; }
			}
			return (largestNumber);
		}

		public string GetEndMesage()
		{
			
			int MVP_ind = FindMVP();
			if (MVP_ind == -1)
			{
				return MVP_Plugin_Reborn.MVP_Plugin_RebornRef.Config.noKillsText;
			}

			string MVP = playersWithKills[MVP_ind];
			string message = MVP_Plugin_Reborn.MVP_Plugin_RebornRef.Config.killsText;
				
			string Kills = playerKillNumber[playersWithKills.IndexOf(MVP)].ToString();

			if (!message.Contains("%mvp") || !message.Contains("%kills"))
				return ($"{MVP} got {Kills} kills");

			message = message.Replace("%mvp", MVP);
			message = message.Replace("%kills", Kills);
			return (message);
			
		}
	}
}