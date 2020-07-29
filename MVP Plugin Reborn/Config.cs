using System;
using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace MVP_Plugin_Reborn
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public string killsText { get; set; } = "%mvp got %kills kills";
		public string noKillsText { get; set; } = "No one got any kills";
	}
}