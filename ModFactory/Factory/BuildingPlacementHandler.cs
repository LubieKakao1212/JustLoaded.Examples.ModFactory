using Custom2d_Engine.Input;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Tilemap;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Content;

namespace Template.Factory;

public class BuildingPlacementHandler {

    private readonly BuildingBlueprintObject _blueprint;
    private readonly Grid _grid;
    
    public BuildingPlacementHandler(World world, Grid grid) {
        _blueprint = new BuildingBlueprintObject(world);
        _blueprint.SetBuilding(Buildings.Assembler);
        _grid = grid;
    }

    public void Spawn(Hierarchy hierarchy) {
        hierarchy.AddObject(_blueprint);
    }
    
    public void BindInput(InputManager inputManager) {
        var left = inputManager.GetMouse(MouseButton.Left);
        left.Started += _ => {
            _blueprint.Place();
            _blueprint.SetBuilding(null);
        };
    }

    public void Update(Vector2 mousePosWorld) {
        _blueprint.Transform.LocalPosition = _grid.GridToCellCenterWorld(_grid.WorldToCell(mousePosWorld));
    }
    
}