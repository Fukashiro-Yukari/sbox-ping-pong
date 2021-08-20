using Sandbox;
using System.Linq;

public partial class Ball : Prop
{
    public float Speed = 100f;
    public float AddSpeed = 30f;
    public Vector3 Forward;
    public bool IsDeath;

    private PingPong game;

    public override void Spawn()
    {
        base.Spawn();

        SetModel("models/ball/ball.vmdl");
        SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);
        Scale = 0.4f;
        game = Game.Current as PingPong;
    }

    [Event.Tick.Server]
    public void OnTick()
    {
        if (Forward.IsNaN || Parent != null) return;

        Velocity = Forward * Speed;

        if (!game.IsStart && !IsDeath)
            Delete();

        var table = All.OfType<PingPongTable>().FirstOrDefault();

        if (table.Position.Distance(Position) > 500f)
        {
            if (!IsDeath)
                game.IsStart = false;

            Delete();
        }
    }

    protected override void OnPhysicsCollision(CollisionEventData eventData)
    {
        var direction = Vector3.Reflect(eventData.PreVelocity.Normal, eventData.Normal.Normal).Normal;

        direction = new Vector3(direction.x, direction.y, 0);
        Forward = direction;

        if (eventData.Entity is Racket)
        {
            Speed += AddSpeed;

            if (Speed > 400f)
            {
                var dmg = new DamageInfo()
                {
                    Flags = DamageFlags.Bullet,
                    Damage = Speed / 50,
                    Force = Speed * 2,
                    Attacker = this,
                    Position = eventData.Pos
                };

                eventData.Entity.TakeDamage(dmg);
            }
        }

        if (eventData.Entity is Wall)
        {
            Speed += AddSpeed / 10;
        }
    }

    [Event.Physics.PostStep]
    public void OnPostPhysicsStep()
    {
        if (!this.IsValid())
            return;

        var body = PhysicsBody;
        if (!body.IsValid())
            return;

        body.GravityScale = 0f;
    }
}