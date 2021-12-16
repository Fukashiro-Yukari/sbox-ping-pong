using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class PlayerName : Panel
{
	public Label Label;
	public Label Health;
	public Image Avatar;
	public PingPongPlayer Player;
	public bool IsDelete;

	public PlayerName( PingPongPlayer player )
	{
		var client = player.Client;

		Health = Add.Label( "", "Health" );
		Label = Add.Label( client.Name, "Name" );
		Avatar = Add.Image( $"avatar:{client.PlayerId}" );
		Player = player;
	}

	public override void Tick()
	{
		if ( Player == null ) return;
		if ( Player.Client == null )
		{
			Delete( true );
			IsDelete = true;
			return;
		}

		var game = Game.Current as PingPong;

		if ( Player.Racket != null )
			Health.Text = $"HP : {Player.Racket.Health:0}";
		else
			Health.Text = game.IsStart ? "HP: 0" : "";

		if ( Player.PlayerID == 1 )
			SetClass( "playerone", true );
		else if ( Player.PlayerID == 2 )
			SetClass( "playertwe", true );
	}
}
