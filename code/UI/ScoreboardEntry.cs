
using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Sandbox.UI
{
	public partial class PingPongScoreboardEntry : Panel
	{
		public PlayerScore.Entry Entry;

		public Label PlayerName;
		public Label Ping;

		public PingPongScoreboardEntry()
		{
			AddClass( "entry" );

			PlayerName = Add.Label( "PlayerName", "name" );
			Ping = Add.Label( "", "ping" );
		}

		public virtual void UpdateFrom( PlayerScore.Entry entry )
		{
			Entry = entry;

			PlayerName.Text = entry.GetString( "name" );
			Ping.Text = entry.Get<int>( "ping", 0 ).ToString();

			SetClass( "me", Local.Client != null && entry.Get<ulong>( "steamid", 0 ) == Local.Client.SteamId );
		}
	}
}
