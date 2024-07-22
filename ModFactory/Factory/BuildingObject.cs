using System;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Drawable;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs.Buildings;

namespace Template.Factory;

public class BuildingObject : PhysicsBodyObject {

    public Building Building { get; private set; }
    
    public DrawableObject Display { get; set; }

    internal BuildingObject(Vector2 position, World world, Building building, float rotation) : base(null) {
        this.Building = building;
        this.PhysicsBody = world.CreateBody(position, rotation, BodyType.Static);
        var size = building.Size.ToVector2();
        //var half = Vector2.One / 2f;
        //size / 2f - building.Origin.ToVector2() - half
        this.Display = AddDrawableRectFixture(size,  Vector2.Zero, 0f, out var fixture);
        this.Display.Sprite = building.Sprite.Value;
    }
}

[Flags]
public enum BuildingPlacement : byte {
    Normal = 0,
    XAxis = 1,
    Negative = 2,
    Mirrored = 4,
    
    Up = Normal,
    Down = Negative,
    Right = XAxis,
    Left = XAxis | Negative,
}

public static class BuildingPlacementExtensions {

    public static int ToRotation(this BuildingPlacement placement) {
        var rot = 0;
        if ((placement & BuildingPlacement.XAxis) > 0) {
            rot += 1;
        }
        if ((placement & BuildingPlacement.Negative) > 0) {
            rot += 2;
        }
        return rot;
    }

    public static Vector2 ToScale(this BuildingPlacement placement) {
        var result = Vector2.One;
        if ((placement & BuildingPlacement.Mirrored) != 0) {
            if ((placement & BuildingPlacement.XAxis) != 0) {
                return result with { Y = -1 };
            }
            else {
                return result with { X = -1 };
            }
        }
        return result;
    }
    
}