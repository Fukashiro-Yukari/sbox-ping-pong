using Sandbox;
using System;
using System.Linq;

public partial class PingPongPlayer : Entity
{
	[Net] public Racket Racket { get; set; }
	[Net] public int Score { get; set; }
	[Net] public int PlayerID { get; set; } = -1;

	private PingPong game;

	public PingPongPlayer()
	{
		Camera = new PingPongCamera();
		Transmit = TransmitType.Always;
		game = Game.Current as PingPong;
	}

	private Goal goal1;
	private Goal goal2;
	private Goal goal3;

	public void CreateRacket()
	{
		if ( game.Table == null || Racket != null || PlayerID > 2 ) return;

		Racket = new Racket()
		{
			Position = game.Table.Position + game.Table.Rotation.Forward * (PlayerID == 1 ? 180 : -180),
			Rotation = Rotation.From( game.Table.Rotation.Angles() + new Angles( 0, PlayerID == 1 ? 180 : 0, 0 ) ),
			PlayerID = PlayerID
		};

		goal1 = new Goal()
		{
			Position = game.Table.Position + game.Table.Rotation.Forward * (PlayerID == 1 ? -300 : 300) + game.Table.Rotation.Up * 200,
			Rotation = Rotation.From( game.Table.Rotation.Angles() + new Angles( 90, 0, 0 ) ),
			Player = this
		};

		goal2 = new Goal()
		{
			Position = game.Table.Position + game.Table.Rotation.Left * 200 + game.Table.Rotation.Forward * (PlayerID == 1 ? -400 : 400) + game.Table.Rotation.Up * 200,
			Rotation = Rotation.From( game.Table.Rotation.Angles() + new Angles( 90, 90, 0 ) ),
			Player = this
		};

		goal3 = new Goal()
		{
			Position = game.Table.Position + game.Table.Rotation.Left * -200 + game.Table.Rotation.Forward * (PlayerID == 1 ? -400 : 400) + game.Table.Rotation.Up * 200,
			Rotation = Rotation.From( game.Table.Rotation.Angles() + new Angles( 90, 90, 0 ) ),
			Player = this
		};
	}

	public void RemoveRacket()
	{
		Racket?.Delete();
		Racket = null;

		goal1?.Delete();
		goal1 = null;

		goal2?.Delete();
		goal2 = null;

		goal3?.Delete();
		goal3 = null;

		ClearScore();
	}

	public void ClearScore()
	{
		Score = 0;
	}

	private bool isinitspawn;

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( IsClient ) return;
		if ( game.Table != null && !isinitspawn )
		{
			isinitspawn = true;

			Position = game.Table.Rotation.Up * 450;
			Rotation = Rotation.From( game.Table.Rotation.Angles() + new Angles( 90, 0, 90 ) );
		}

		if ( game.Table == null || Racket == null ) return;

		var startpos = Racket.Position + Racket.Rotation.Up * 10;
		var dir = PlayerID == 1 ? Racket.Rotation.Right : Racket.Rotation.Left;

		var tr = Trace.Ray( startpos, startpos + dir * 200 )
			.WithTag( "Wall" )
			.Radius( 8 )
			.Run();

		var tr2 = Trace.Ray( startpos, startpos + dir * -200 )
			.WithTag( "Wall" )
			.Radius( 8 )
			.Run();

		var add = Input.MouseDelta.y;
		var speed = 40f;
		var mindist = 4f;

		add = Math.Clamp( add, -speed, speed );

		var abs = Math.Abs( add ) / 3;

		if ( tr.EndPos.Distance( startpos ) < mindist * abs )
		{
			add = Math.Min( add, -0.1f );
		}

		if ( tr2.EndPos.Distance( startpos ) < mindist * abs )
		{
			add = Math.Max( add, 0.1f );
		}

		Racket.Position += dir * add;

		if ( !game.IsStart )
		{
			if ( Input.Pressed( InputButton.Attack1 ) )
			{
				Racket.Start();
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if ( IsClient ) return;

		RemoveRacket();
	}
}
