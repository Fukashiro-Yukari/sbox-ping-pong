
using System;
using System.Linq;

namespace Sandbox
{
	public partial class PingPongCamera : Camera
	{
		public override void Activated()
        {
            if (Local.Pawn is PingPongPlayer player)
            {
                Pos = player.Position;
                Rot = player.Rotation;
            }

            base.Activated();
        }

        public override void Update()
		{
			if (Local.Pawn is PingPongPlayer player)
            {
				FieldOfView = 80;
				Pos = Pos.LerpTo(player.Position, Time.Delta);
				Rot = player.Rotation;
			}

			Viewer = null;
		}
	}
}
