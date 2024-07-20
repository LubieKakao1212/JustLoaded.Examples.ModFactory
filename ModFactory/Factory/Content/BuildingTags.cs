using JustLoaded.Core.Reflect;
using Template.Factory.Defs;
using Template.Factory.Defs.Buildings;

namespace Template.Factory.Content;

[ContentContainer]
[FromMod(FactoryReg.ModId)]
public class BuildingTags {

    [RegisterContent] public static readonly Tag<Building> Buildable = new();

}