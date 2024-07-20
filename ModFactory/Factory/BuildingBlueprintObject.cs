using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Drawable;
using Custom2d_Engine.Scenes.Factory;
using Custom2d_Engine.Ticking;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs.Buildings;

namespace Template.Factory;

public class BuildingBlueprintObject : HierarchyObject {
    
    private readonly DrawableObject _display;
    private readonly DrawableObject _marker;
    private Building? _building;

    private readonly World _world;

    private BuildingPlacement _placement;
    
    public BuildingBlueprintObject(World world) {
        _building = null;
        _display = this.CreateDrawableChild(Sprite.Empty, drawOrder: 0.5f);
        _marker = this.CreateDrawableChild(Sprite.Empty, color: Color.Red * 0.25f, drawOrder: 0.75f);
        SetBuilding(null);
        _world = world;
    }
    
    protected override void AddedToScene() {
        base.AddedToScene();
        
        DrawableObject dummy;
        this.AddSimpleRepeatingAction(() => {
            if (_building == null) {
                return;
            }

            dummy = _building.CanPlace(Transform.GlobalPosition, _world, this.CurrentHierarchy)
                ? _marker.SetQueueBehaviour(RenderPasses.Normals, QueueBehaviour.Skip)
                    .SetQueueBehaviour(RenderPasses.Final, QueueBehaviour.Skip)
                : _marker.SetQueueBehaviour(RenderPasses.Normals, QueueBehaviour.BatchRender)
                    .SetQueueBehaviour(RenderPasses.Final, QueueBehaviour.BatchRender);
        }, 0.5f);
    }

    public void SetBuilding(Building? building) {
        _building = building;

        if (building == null) {
            _display.Sprite = Sprite.Empty;
            _marker.Sprite = Sprite.Empty;
            return;
        }

        var size = building.Size.ToVector2();
        var half = Vector2.Zero; // Vector2.One / 2f;
        var origin = size / 2f - building.Origin.ToVector2() - half;
    
        _display.Sprite = building.Sprite.Value;
        _display.Transform.LocalPosition = origin;
        _display.Transform.LocalScale = size;
        
        _marker.Sprite = Sprite.UnlitNoNormal;
        _marker.Transform.LocalPosition = origin;
        _marker.Transform.LocalScale = size;
    }
    
    public void Place() {
        if (_building == null) {
            return;
        }
        
        var pos = Transform.GlobalPosition;
        if (_building.CanPlace(pos, _world, CurrentHierarchy)) {
            _building.Place(CurrentHierarchy, pos, _world, _placement);
        }
    }

    public void Rotate() {
        /*if ((_placement & BuildingPlacement.XAxis) == 0) {
            if ((_placement & BuildingPlacement.Negative) == 0) {
                //Up -> Right
                _placement |= BuildingPlacement.XAxis;
            }
            else {
                _placement |= BuildingPlacement.XAxis;
                //Down -> Left
            }
        }
        else {
            if ((_placement & BuildingPlacement.Negative) == 0) {
                //Right -> Down
                _placement ^= BuildingPlacement.XAxis;
                _placement ^= BuildingPlacement.Negative;
            }
            else {
                //Left -> Up
                _placement ^= BuildingPlacement.XAxis;
                _placement ^= BuildingPlacement.Negative;
            }
        }
        */

        if ((_placement & BuildingPlacement.XAxis) != 0) {
            _placement ^= BuildingPlacement.Negative;
        }
        _placement ^= BuildingPlacement.XAxis;

        Transform.LocalRotation = MathHelper.PiOver2 * _placement.ToRotation();
    }

    public void Mirror() {
        _placement ^= BuildingPlacement.Mirrored;

        Transform.LocalScale = _placement.ToScale();
    }
    
}