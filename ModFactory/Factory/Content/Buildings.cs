using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Core.Reflect;
using Microsoft.Xna.Framework;
using Template.Factory.Defs.Buildings;

namespace Template.Factory.Content;

[FromMod(FactoryReg.ModId)]
[ContentContainer]
public static class Buildings {

    [RegisterContent] public static readonly Building Assembler;

    [RegisterContent] public static readonly Building BasicLight;
    [RegisterContent] public static readonly Building WhiteLight;
    
    static Buildings() {
        Assembler = new DecorationBuilding();
        Assembler.Sprite = new DatabaseReference<Sprite>(Sprites.Gear);
        Assembler.Size = new Point(2, 2);
        Assembler.Origin = new Point(0, 0);
        
        //Default size and origin
        BasicLight = new LightBuilding(1f, new Color(200, 200, 255), 20f, 0.75f);
        BasicLight.Sprite = new DatabaseReference<Sprite>(Sprites.Light);
        
        //Default size and origin
        WhiteLight = new LightBuilding(1f, new Color(255, 255, 255), 20f, 0.75f);
        WhiteLight.Sprite = new DatabaseReference<Sprite>(Sprites.Light);
        
    }

}