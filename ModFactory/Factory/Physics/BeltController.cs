using System;
using System.Collections.Generic;
using JustLoaded.Content;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Controllers;
using nkast.Aether.Physics2D.Dynamics;

namespace Template.Factory.Physics;

public class BeltController : Controller {

    public AABB Area { get; set; }
    public Vector2 Anchor { get; set; }
    public Vector2 Direction { get; set; }

    public float Width { get; set; } = 0.01f;
    
    public float TargetLinearSpeed { get; set; }
    public float TargetAlignmentSpeed { get; set; }
    public float MaxForce { get; set; }

    public Func<Body, bool> Filter { get; set; } = (_) => true;

    private HashSet<Body> _uniqueueBodies = new();

    public BeltController(Vector2 axisFrom, Vector2 axisTo, AABB area, float targetSpeed = 1f, float maxForce = 10f) {
        this.Anchor = axisFrom;
        this.Direction = (axisTo - axisFrom);
        this.Direction.Normalize();
        this.Area = area;
        this.TargetLinearSpeed = targetSpeed;
        this.TargetAlignmentSpeed = TargetLinearSpeed;
        this.MaxForce = maxForce;
    }
    
    public override void Update(float dt) {
        _uniqueueBodies.Clear();
        
        World.QueryAABB(fixture => {
            if (fixture.Body.BodyType == BodyType.Static || !fixture.Body.Awake)
                return true;
            _uniqueueBodies.Add(fixture.Body);
            return true;
        }, Area);

        foreach (var body in _uniqueueBodies) {
            if (!IsActiveOn(body) || !Filter(body)) {
                continue;
            }

            var pos = body.Position;
            var p0 = pos - Anchor;
            var posProj = Vector2.Dot(p0, Direction) * Direction;
            posProj += Anchor;

            var deltaProj = (posProj - pos);
            //var a = Direction.X * deltaProj.Y - Direction.Y * deltaProj.X;
            var d = deltaProj.Length();

            var targetVelocity = Direction * TargetLinearSpeed;
            
            if (d > Width) {
                var dV = deltaProj;
                dV.Normalize();
                dV *= Math.Min(deltaProj.Length(), TargetAlignmentSpeed);
                targetVelocity += dV;
            }

            var deltaV = targetVelocity - body.LinearVelocity;
            
            var forceStr = Math.Min(deltaV.Length(), MaxForce);
            deltaV.Normalize();
            deltaV *= forceStr;
            body.ApplyForce(deltaV);
        }
    }
    
}