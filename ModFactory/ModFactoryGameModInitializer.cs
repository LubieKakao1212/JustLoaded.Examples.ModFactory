using System;
using System.Collections.Generic;
using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;
using JustLoaded.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Template.Factory;
using Template.Factory.Content;
using Template.Factory.Defs;
using Template.Factory.Defs.Buildings;
using Template.Factory.Loading;
using Template.ModLoader;

namespace Template;

[FromMod(FactoryReg.ModId)]
public class ModFactoryGameModInitializer : IModInitializer, IDatabaseRegisterer, ISpriteGatherer {

    private readonly ContentManager _content;
    private readonly RenderPipeline _renderPipeline;
    private readonly SpriteAtlas<Color> _atlas;
    
    public ModFactoryGameModInitializer(ContentManager content, SpriteAtlas<Color> atlas, RenderPipeline renderPipeline) {
        this._content = content;
        this._renderPipeline = renderPipeline;
        this._atlas = atlas;
    }
    
    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(FactoryReg.LoadingPhase.RegisterDb, new DefaultDatabaseRegistrationEntrypointLoadingPhase())
            .Register();
        
        phases.New(FactoryReg.LoadingPhase.AutoRegContent, new RegisterContentLoadingPhase())
            .WithOrder(FactoryReg.LoadingPhase.RegisterDb, Order.After)
            .Register();

        var spriteCtx = new SpriteLoadingContext(_content, _atlas, "albedo", "normal", "emit");

        phases.New(FactoryReg.LoadingPhase.RegisterSprites, new LoadSpritesLoadingPhase(spriteCtx))
            .WithOrder(FactoryReg.LoadingPhase.RegisterDb, Order.After)
            .Register();
        phases.New(FactoryReg.LoadingPhase.RegisterSpritesTmx, new LoadMapsLoadingPhase(spriteCtx))
            .WithOrder(FactoryReg.LoadingPhase.RegisterSprites, Order.After)
            .Register();
        phases.New(FactoryReg.LoadingPhase.RegisterSpritesPost, new DelegatedLoadingPhase(_ => {
                spriteCtx.Finish(_renderPipeline);
                //TODO use logger
                Console.Out.WriteLine("Loaded sprites");
            }))
            .WithOrder(FactoryReg.LoadingPhase.RegisterSpritesTmx, Order.After)
            .Register();
    }

    public void RegisterDatabases(IDatabaseRegistrationContext context) {
        FactoryReg.Database.Items = context.CreateDatabase<Item>(FactoryReg.Database.ItemsKey, DBRegistrationType.Main);
        FactoryReg.Database.Sprites = context.CreateDatabase<Sprite>(FactoryReg.Database.SpritesKey, DBRegistrationType.Main);
        FactoryReg.Database.Buildings = context.CreateDatabase<Building>(FactoryReg.Database.BuildingsKey, DBRegistrationType.Main);
        
        FactoryReg.Database.ItemTags = context.CreateDatabase<Tag<Item>>(FactoryReg.Database.ItemTagsKey, DBRegistrationType.Main);
    }

    public IEnumerable<string> GetSprites() {
        yield return "raw-iron";
        yield return "gear";
        yield return "light";
        yield return "grid";
    }
}