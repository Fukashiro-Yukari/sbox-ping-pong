
using Sandbox;
using System.Collections.Generic;
using System.Linq;

partial class PingPong : Game
{
	[Net] public bool IsStart { get; set; }
	[Net] public PingPongTable Table { get; set; }

	public PingPong()
	{
		if ( IsServer )
		{
			_ = new PingPongHud();
		}
	}

	[Net] public PingPongPlayer PlayerOne { get; set; }
	[Net] public PingPongPlayer PlayerTwe { get; set; }

	private int playerid;
	public int StartPlayer = -1;

	[Event.Tick.Server]
	public void OnTick()
	{
		if ( Table == null )
			Table = All.OfType<PingPongTable>().FirstOrDefault();

		if ( PlayerOne != null && PlayerTwe != null && !IsStart )
		{
			PlayerOne.CreateRacket();
			PlayerTwe.CreateRacket();
		}

		if ( StartPlayer == -1 )
		{
			StartPlayer = Rand.Int( 1, 2 );
		}

		if ( playerid < 2 )
		{
			if ( PlayerOne?.Racket != null )
				PlayerOne?.RemoveRacket();

			if ( PlayerTwe?.Racket != null )
				PlayerTwe?.RemoveRacket();

			IsStart = false;
			StartPlayer = -1;
		}

		if ( playerid > 1 )
		{
			if ( PlayerOne == null )
			{
				PlayerOne = All.OfType<PingPongPlayer>().Where( x => x != PlayerTwe ).FirstOrDefault();
				PlayerOne.PlayerID = 1;

				PlayerTwe?.RemoveRacket();

				IsStart = false;
				StartPlayer = -1;
			}

			if ( PlayerTwe == null )
			{
				PlayerTwe = All.OfType<PingPongPlayer>().Where( x => x != PlayerOne ).FirstOrDefault();
				PlayerTwe.PlayerID = 2;

				PlayerOne?.RemoveRacket();

				IsStart = false;
				StartPlayer = -1;
			}
		}
	}

	[ServerCmd( "pp_restart" )]
	public static void ReStart()
	{
		var game = Current as PingPong;

		game.PlayerOne?.RemoveRacket();
		game.PlayerTwe?.RemoveRacket();

		game.IsStart = false;
		game.StartPlayer = -1;
	}

	public override void ClientJoined( Client client )
	{
		var player = new PingPongPlayer();
		client.Pawn = player;

		playerid++;

		player.PlayerID = playerid;

		if ( playerid == 1 )
			PlayerOne = player;
		else if ( playerid == 2 )
			PlayerTwe = player;

		base.ClientJoined( client );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );

		playerid--;
	}
}
