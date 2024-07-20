using System;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Factory;
using Custom2d_Engine.Util;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;
using Template.Factory.Physics;

namespace Template.Factory;

public class BeltObject : HierarchyObject {
    
    public BeltController Controller { get; }
    private readonly World _world;

    public BeltObject(World world, Vector2 direction, Color color) {
        this._world = world;
        Controller = new BeltController(
            Transform.GlobalPosition, 
            Transform.GlobalPosition + Transform.Up, 
            new AABB(Transform.GlobalPosition, 1f, 1f))
            {
                Direction = direction
            };

        this.CreateDrawableChild(Sprite.Unlit, color: color);
        
        Transform.Changed += () => {
            var size = Transform.GlobalScale * 2f;
            var pos = Transform.GlobalPosition;
            Controller.Area = new AABB(pos, size.X, size.Y);
            Controller.Anchor = pos;
        };
    }

    protected override void AddedToScene() {
        base.AddedToScene();
        
        _world.Add(Controller);
    }

    protected override void RemovedFromScene() {
        base.RemovedFromScene();
        
        _world.Remove(Controller);
    }

    public static Func<Body, bool> ItemFilter(bool fallback, Func<Item, bool> itemFilter) {
        return body => {
            if (body.Tag is ItemObject itemObj) {
                return itemFilter(itemObj.Item);
            }

            return fallback;
        };
    }
}