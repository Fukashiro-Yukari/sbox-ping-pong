using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Linq;

public class WaitingPlayer : Panel
{
    public Label Label;

    public WaitingPlayer()
    {
        Label = Add.Label("Waiting for the player (0 / 2)", "value");
    }

    public override void Tick()
    {
        var game = Game.Current as PingPong;

        if (game.Table == null)
        {
            Label.Text = "This map is not a ping pong game map !!";

            return;
        }

        var plyc = Entity.All.OfType<PingPongPlayer>().Count();

        if (plyc > 1)
        {
            Label.Text = "";

            return;
        }

        Label.Text = $"Waiting for the player ({plyc} / 2)";
    }
}