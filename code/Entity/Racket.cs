using Sandbox;

public partial class Racket : Prop
{
    private PingPong game;
    public int PlayerID;
    private Ball Ball;

    public override void Spawn()
    {
        base.Spawn();

        SetModel("models/citizen_props/concreteroaddivider01.vmdl");
        SetupPhysicsFromModel(PhysicsMotionType.Static);
        Scale = 0.4f;

        game = Game.Current as PingPong;
    }

    [Event.Tick.Server]
    public void OnTick()
    {
        if (!game.IsStart && PlayerID == game.StartPlayer)
            CreateBall();

        if (!game.IsStart)
            Health = 100f;
    }

    public void CreateBall()
    {
        if (Ball != null) return;

        var ryaw = 60;
        var ang = Rotation.From(Rotation.Angles() + new Angles(0, Rand.Float(-ryaw, ryaw), 0));

        Ball = new Ball()
        {
            Position = Position + Rotation.Forward * 20 + Rotation.Up * 6,
            Parent = this,
            EnableAllCollisions = false,
            Forward = ang.Forward,
        };
    }

    public void Start()
    {
        if (Ball == null) return;

        Ball.Parent = null;
        Ball.EnableAllCollisions = true;
        Ball = null;

        game.IsStart = true;
    }

    public override void TakeDamage(DamageInfo info)
    {
        if (info.Flags != DamageFlags.Bullet || !game.IsStart) return;

        base.TakeDamage(info);
    }
}
