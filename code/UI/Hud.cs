using Sandbox;
using Sandbox.UI;

[Library]
public partial class PingPongHud : HudEntity<RootPanel>
{
	public PingPongHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet.Load( "/UI/Hud.scss" );

		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<PingPongScoreboard<PingPongScoreboardEntry>>();
		RootPanel.AddChild<Score>();
		RootPanel.AddChild<WaitingPlayer>();
	}
}
