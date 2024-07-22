using Custom2d_Engine.Scenes;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
namespace Template.Factory.Defs.Buildings;

public class TransportBeltBuilding : Building {

    public float Speed { get; init; }

    protected override void OnPlace(BuildingObject obj, Hierarchy hierarchy, World world) {
        var belt = new BeltObject(world) {
            Controller = {
                TargetAlignmentSpeed = 10,
                TargetLinearSpeed = Speed,
                MaxForce = 10
            },
            Parent = obj,
            Transform = {
                LocalScale = new Vector2(0.5f, 1f)
            }
        };
    }
    
}