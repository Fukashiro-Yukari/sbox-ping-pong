using System;
using System.Linq;

namespace Sandbox
{
	public partial class PingPongCamera : Camera
	{
		public override void Activated()
		{
			if ( Local.Pawn is PingPongPlayer player )
			{
				Position = player.Position;
				Rotation = player.Rotation;
			}

			base.Activated();
		}

		public override void Update()
		{
			if ( Local.Pawn is PingPongPlayer player )
			{
				FieldOfView = 80;
				Position = Position.LerpTo( player.Position, Time.Delta );
				Rotation = player.Rotation;
			}

			Viewer = null;
		}
	}
}
