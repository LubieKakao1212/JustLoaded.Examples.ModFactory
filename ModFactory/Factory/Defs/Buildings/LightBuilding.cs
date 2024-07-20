using Custom2d_Engine.Rendering;
using Custom2d_Engine.Scenes;
using Custom2d_Engine.Scenes.Drawable.Lights;
using Microsoft.Xna.Framework;

namespace Template.Factory.Defs.Buildings;

public class LightBuilding : Building {

    private readonly Color _tint;
    private readonly float _intensity;
    private readonly float _size;
    private readonly float _falloff;
    
    public LightBuilding(float intensity, Color tint, float size, float falloff = 0.5f) {
        _intensity = intensity;
        _tint = tint;
        _size = size;
        _falloff = falloff;
    }
    
    protected override void OnPlace(BuildingObject obj, Hierarchy hierarchy) {
        var light = new PointLight(ModFactoryGame.RenderPipeline, _tint, 1000f);
        ConfigureLight(light);
        light.Parent = obj;
    }

    protected virtual void ConfigureLight(PointLight light) {
        light.InnerAngle = MathHelper.TwoPi;
        light.OuterAngle = MathHelper.TwoPi;
        light.Intensity = _intensity;
        light.OuterRadius = _size;
        light.InnerRadius = _size * _falloff;
        light.LightHeight = 3.5f;
    }
    
}