using System;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using nkast.Aether.Physics2D.Common;
using Path = System.IO.Path;

namespace Template.Factory.Loading;

public class LoadSpritesLoadingPhase : EntrypointLoadingPhase<ISpriteGatherer> {

    private SpriteLoadingContext _ctx;

    public LoadSpritesLoadingPhase(SpriteLoadingContext ctx) {
        _ctx = ctx;
    }
    
    protected override void HandleEntrypointFor(Mod mod, ISpriteGatherer entrypoint, ModLoaderSystem modLoader) {
        var db = modLoader.MasterDb.GetByContentType<Sprite>();
        
        if(db == null) {
            throw new ApplicationException("Something went very wrong");
        }
        
        foreach (var spritePath in entrypoint.GetSprites()) {
            var key = new ContentKey(ModMetadata.ToModId(mod.Metadata.ModKey), spritePath);
            var sprite = _ctx.LoadSprite($"mods/{key.source}/{key.path}");

            db.AddContent(key, sprite);
        }
    }
    
    
    protected override void Setup(ModLoaderSystem modLoader) {
        //TODO use logger
        Console.Out.WriteLine("Loading sprites...");
    }
}