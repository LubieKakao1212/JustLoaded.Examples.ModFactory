using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;

namespace Template.Factory;

public static class CollisionGroups {

    public static readonly Category Default = Category.Cat1;
    public static readonly Category Items = Category.Cat2;
    public static readonly Category Enviro = Category.Cat3;
    public static readonly Category Buildings = Category.Cat4;
    public static readonly Category BuildingTriggers = Category.Cat5;
    public static readonly Category Draggable = Category.Cat6;

    public static readonly Category ItemsCategories = Items | Draggable;
    public static readonly Category ItemsCollideWith = Items | Enviro | Default | BuildingTriggers;

    public static readonly Category BuildingPlacementQuery = Buildings | Enviro;
    
    
}