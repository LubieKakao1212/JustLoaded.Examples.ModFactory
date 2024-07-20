using System.Diagnostics.CodeAnalysis;
using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;
using Template.Factory;
using Template.Factory.Defs;
using Template.Factory.Defs.Buildings;

namespace Template;

public static class FactoryReg {

    public const string ModId = "game";

    public static class LoadingPhase {
        public static readonly ContentKey RegisterDb = new ContentKey(ModId, "register-db");
        public static readonly ContentKey AutoRegContent = new ContentKey(ModId, "register-content-auto");
        
        public static readonly ContentKey RegisterSprites = new ContentKey(ModId, "register-sprites");
        public static readonly ContentKey RegisterSpritesTmx = new ContentKey(ModId, "register-tmx");
        public static readonly ContentKey RegisterSpritesPost = new ContentKey(ModId, "register-sprites-post");
    }

    public static class Database {
        public static readonly ContentKey ItemsKey = new ContentKey(ModId, "items");
        [NotNull] public static IContentDatabase<Item>? Items { get; internal set; }

        
        public static readonly ContentKey SpritesKey = new ContentKey(ModId, "sprites");
        [NotNull] public static IContentDatabase<Sprite>? Sprites { get; internal set; }
        
        
        public static readonly ContentKey ItemTagsKey = new ContentKey(ModId, "tags/item");
        [NotNull] public static IContentDatabase<Tag<Item>>? ItemTags { get; internal set; }
        
        
        public static readonly ContentKey BuildingsKey = new ContentKey(ModId, "buildings");
        [NotNull] public static IContentDatabase<Building>? Buildings { get; internal set; }
    }
}