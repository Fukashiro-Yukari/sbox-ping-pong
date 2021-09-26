using Sandbox;

public partial class Goal : AnimEntity
{
	private PingPong game;
	public PingPongPlayer Player;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/room.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		EnableAllCollisions = false;
		EnableDrawing = false;
		EnableTouch = true;
		EnableTouchPersists = true;

		game = Game.Current as PingPong;
	}

	public override void StartTouch( Entity ent )
	{
		if ( Player == null || !(ent is Ball ball) || !game.IsStart ) return;
		if ( ball.IsDeath ) return;

		ball.DeleteAsync( 5 );
		ball.IsDeath = true;

		Player.Score++;

		game.StartPlayer = Player.PlayerID == 1 ? 2 : 1;
		game.IsStart = false;
	}
}
