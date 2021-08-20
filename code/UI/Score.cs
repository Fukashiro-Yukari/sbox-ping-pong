using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Score : Panel
{
    public Label Label;

    public Score()
    {
        Label = Add.Label("0 : 0", "value");
    }

    public PlayerName CreatePlayerName(PingPongPlayer player)
    {
        if (player.GetClientOwner() == null) return null;

        var plyn = new PlayerName(player);
        plyn.Parent = this;
        return plyn;
    }

    private PlayerName PlayerOneName;
    private PlayerName PlayerTweName;

    public override void Tick()
    {
        var game = Game.Current as PingPong;

        if (game.PlayerOne != null && game.PlayerTwe != null)
        {
            Label.Text = $"{game.PlayerOne.Score} : {game.PlayerTwe.Score}";
        }

        if (game.PlayerOne != null && PlayerOneName == null)
        {
            PlayerOneName = CreatePlayerName(game.PlayerOne);
        }

        if (game.PlayerTwe != null && PlayerTweName == null)
        {
            PlayerTweName = CreatePlayerName(game.PlayerTwe);
        }

        if (PlayerOneName != null)
        {
            if (PlayerOneName.IsDelete)
                PlayerOneName = null;
            else
                PlayerOneName.Tick();
        }

        if (PlayerTweName != null)
        {
            if (PlayerTweName.IsDelete)
                PlayerTweName = null;
            else
                PlayerTweName.Tick();
        }
    }
}