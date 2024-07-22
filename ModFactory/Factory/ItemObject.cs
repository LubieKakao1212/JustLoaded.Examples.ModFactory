using Custom2d_Engine.Physics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Controllers;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;

namespace Template.Factory;

public class ItemObject : PhysicsBodyObject {

    public Item Item { get; }

    public ItemObject(World world, Vector2 position, Item item) : base(null) {
        this.PhysicsBody = world.CreateBody(position, 0f, BodyType.Dynamic);
        this.PhysicsBody.LinearDamping = 5f;
        this.PhysicsBody.Tag = this;
        PhysicsBody.FixedRotation = true;
        this.Item = item;
        
        var drawable = this.AddDrawableRectFixture(Vector2.One * 0.5f, Vector2.Zero, 0, out var fixture);
        fixture.IsSensor = false;
        fixture.Restitution = 1;
        fixture.CollisionCategories = CollisionGroups.ItemsCategories;
        fixture.CollidesWith = CollisionGroups.ItemsCollideWith;
        
        drawable.Color = Item.Color;
        drawable.Sprite = Item.Sprite.Value;
        drawable.DrawOrder = 1f;
    }
    
}