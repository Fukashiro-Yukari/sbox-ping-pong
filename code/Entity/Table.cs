using Sandbox;

[Library("ent_table", Title = "Ping Pong Table")]
[Hammer.EditorModel("models/room.vmdl")]
public partial class PingPongTable : Prop
{
    public Wall Wall1;
    public Wall Wall2;

    bool IsCreateWall;

    public override void Spawn()
    {
        base.Spawn();

        SetModel("models/room.vmdl");
        SetupPhysicsFromModel(PhysicsMotionType.Static);
    }

    [Event.Tick.Server]
    public void OnTick()
    {
        if (!IsCreateWall)
            CreateWall();
    }

    public void CreateWall()
    {
        if (IsCreateWall) return;

        IsCreateWall = true;

        var thisang = Rotation.Angles();

        Wall1 = new Wall()
        {
            Position = Position + Rotation.Left * 200 + Rotation.Up * 200,
            Rotation = Rotation.From(thisang + new Angles(90, 90, 0))
        };

        Wall2 = new Wall()
        {
            Position = Position + Rotation.Left * -200 + Rotation.Up * 200,
            Rotation = Rotation.From(thisang + new Angles(90, 90, 0))
        };
    }
}