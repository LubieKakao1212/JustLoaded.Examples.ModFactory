using System;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.TMX;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Common;
using Path = System.IO.Path;

namespace Template.Factory.Loading;

public class LoadMapsLoadingPhase : EntrypointLoadingPhase<ITmxMapGatherer> {

    private SpriteLoadingContext _ctx;
    
    public LoadMapsLoadingPhase(SpriteLoadingContext ctx) {
        _ctx = ctx;
    }
    
    protected override void HandleEntrypointFor(Mod mod, ITmxMapGatherer entrypoint, ModLoaderSystem modLoader) {
        var db = modLoader.MasterDb.GetByContentType<LoadedMap<Color>>();
        
        if(db == null) {
            throw new ApplicationException("Something went very wrong");
        }
        
        foreach (var spritePath in entrypoint.GetMaps()) {
            var key = new ContentKey(ModMetadata.ToModId(mod.Metadata.ModKey), spritePath);
            var sprite = _ctx.LoadTmxMap(Path.Combine("mods", key.source, key.path));
            
            db.AddContent(key, sprite);
        }
    }
}