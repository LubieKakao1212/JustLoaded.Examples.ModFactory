using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Drawable;
using JustLoaded.Content;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;

namespace Template.Factory.Defs.Buildings;

public abstract class Building {

    public Point Size { get; set; } = new Point(1, 1);
    public Point Origin { get; set; } = new Point(0, 0);
    
    public DatabaseReference<Sprite> Sprite { get; set; }

    public BuildingObject Place(Hierarchy hierarchy, Point position, World world, BuildingPlacement placement) {
        return Place(hierarchy, position.ToVector2() * 2f - Vector2.One / 2f, world, placement);
    }
    
    public BuildingObject Place(Hierarchy hierarchy, Vector2 position, World world, BuildingPlacement placement) {
        //var pos = position.ToVector2() * 2f;
        var pos = position + Size.ToVector2() / 2f - Origin.ToVector2() * 2f;
        var buildingObj = new BuildingObject(pos, world, this, placement.ToRotation() * MathHelper.PiOver2);
        OnPlace(buildingObj, hierarchy, world);
        hierarchy.AddObject(buildingObj);
        return buildingObj;
    }
    
    protected abstract void OnPlace(BuildingObject obj, Hierarchy hierarchy, World world);

    public virtual bool CanPlace(Vector2 position, World world, Hierarchy hierarchy) {
        //
        var half = Vector2.One / 2f;
        var a = -Size.ToVector2() / 2f - half;
        
        var pos = position + Size.ToVector2() / 2f + Origin.ToVector2() * 2f;
        var aabb = new AABB(pos, Size.X * 2f, Size.Y * 2f);
        var flag = true;
        world.QueryAABB(fixture => flag = false, aabb);
        
        //Debug display
        /*hierarchy.AddObject(new DrawableObject(Color.White, 0f) {
            Sprite = Custom2d_Engine.Rendering.Sprites.Sprite.Unlit,
            Transform = {
                GlobalPosition = aabb.Center,
                LocalScale = aabb.Extents
            }
        });*/
        
        return flag;
    }

    public virtual void Update() { }
    
}
