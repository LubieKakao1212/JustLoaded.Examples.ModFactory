using System.Collections.Generic;
using Custom2d_Engine.Physics;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Drawable;
using Custom2d_Engine.Scenes.Events;
using Custom2d_Engine.Scenes.Factory;
using Custom2d_Engine.Ticking;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;
using Template.Factory.Physics;

namespace Template.Factory;

public abstract class ItemSpawnerObject : PhysicsBodyObject, IUpdatable {

    private readonly double _cooldown;
    private readonly TimeMachine _timer = new();
    private readonly DrawableObject _errorDisplay;
    private readonly TriggerFixtureWrapper _trigger;

    public ItemSpawnerObject(double cooldown, World world) : base(null) {
        this.PhysicsBody = world.CreateBody(bodyType: BodyType.Static);
        this._cooldown = cooldown;
        this._errorDisplay = AddDrawableRectFixture(Vector2.One, Vector2.Zero, 0, out var fixture);
        this._errorDisplay.Color = Color.Green * 0.25f;
        this._errorDisplay.Sprite = Sprite.UnlitNoNormal;
        
        _trigger = new TriggerFixtureWrapper(fixture);
    }

    public override void Update(GameTime time) {
        base.Update(time);

        if (IsBlocked()) {
            _timer.Retrieve(double.PositiveInfinity);
            _errorDisplay.Color = Color.Red * 0.25f;
        }
        else {
            _timer.Accumulate(time.ElapsedGameTime.TotalSeconds);
            _errorDisplay.Color = Color.Green * 0.25f;

            if (_timer.TryRetrieve(_cooldown)) {
                var item = GetNextItem();
                var itemObj = new ItemObject(PhysicsBody.World, Transform.GlobalPosition, item);
                CurrentHierarchy.AddObject(itemObj);
            }
        }
    }
    
    public virtual bool IsBlocked() {
        return _trigger.CollisionCount >= 3;
    }
    
    protected abstract Item GetNextItem();

}