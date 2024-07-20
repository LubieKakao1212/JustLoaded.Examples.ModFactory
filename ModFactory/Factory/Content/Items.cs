using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Core.Reflect;
using Microsoft.Xna.Framework;
using Template.Factory.Defs;

namespace Template.Factory.Content;

[FromMod(FactoryReg.ModId)]
[ContentContainer]
public static class Items {

    [RegisterContent] public static readonly Item RawIron = new Item(Sprites.RawIron);
    [RegisterContent] public static readonly Item DarkIron = new Item(Sprites.RawIron, Color.Gray);
    
    public static class Keys {
        public static readonly ContentKey RawIron = new ContentKey(FactoryReg.ModId, "raw-iron");
        
        public static readonly ContentKey DarkIron = new ContentKey(FactoryReg.ModId, "dark-iron");
    }
}