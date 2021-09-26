using Sandbox;

public partial class Wall : Prop
{
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/room.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableDrawing = false;
		Tags.Add( "Wall" );
	}
}
