using System;
using System.Transactions;
using Custom2d_Engine.Input;
using Custom2d_Engine.Physics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;

namespace Template.Factory;

public class Dragger : PhysicsBodyObject {

    private RopeJoint? _currentJoint = null;
    
    public Dragger(World world) : base(null) {
        PhysicsBody = world.CreateBody(bodyType: BodyType.Kinematic);
    }

    public override void Update(GameTime time) {
        var delta = Transform.GlobalPosition - PhysicsBody.Position;
        PhysicsBody.LinearVelocity = delta / (float)time.ElapsedGameTime.TotalSeconds;
    }

    public void Attach() {
        if (_currentJoint != null) {
            Console.Out.WriteLine("Already attached");
            return;
        }
        
        var world = PhysicsBody.World;
        var pos = Transform.GlobalPosition;
        Body? closestBody = null;
        float closestDistance = float.PositiveInfinity;
        world.QueryAABB(fixture => {
                if (fixture.CollisionCategories.HasFlag(CollisionGroups.Draggable)) {
                    var body = fixture.Body;
                    var distance = (pos - body.Position).LengthSquared();
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestBody = body;
                    }
                }
                return true;
            },
        new AABB(Transform.GlobalPosition, 0.5f, 0.5f));

        if (closestBody == null) {
            return;
        }
        _currentJoint = new RopeJoint(PhysicsBody, closestBody, Vector2.Zero, Vector2.Zero) {
            MaxLength = 0.5f
        };
        world.Add(_currentJoint);
    }

    public void Detach() {
        if (_currentJoint == null) {
            Console.Out.WriteLine("Not attached");
            return;
        }
        
        PhysicsBody.World.Remove(_currentJoint);
        _currentJoint = null;
    }
    
}