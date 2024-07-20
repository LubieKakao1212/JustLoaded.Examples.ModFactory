using JustLoaded.Core.Reflect;
using Template.Factory.Defs;

namespace Template.Factory.Content;

[FromMod(FactoryReg.ModId)]
[ContentContainer]
public static class ItemTags {

    [RegisterContent] public static Tag<Item> RawOre = new Tag<Item>();
    [RegisterContent] public static Tag<Item> Burnable = new Tag<Item>();
    
}