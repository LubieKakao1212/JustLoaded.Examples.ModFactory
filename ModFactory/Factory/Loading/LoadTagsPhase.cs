using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using Template.Factory.Defs;

namespace Template.Factory.Loading;

public class LoadTagsPhase<T> : EntrypointLoadingPhase<ITagLoader<T>> where T : class {

    [NotNull] public IContentDatabase<Tag<T>>? Database { get; private set; }

    protected override void Setup(ModLoaderSystem modLoader) {
        base.Setup(modLoader);

        Database = (IContentDatabase<Tag<T>>) modLoader.MasterDb.GetByContentType<Tag<T>>()!;
    }

    protected override void HandleEntrypointFor(Mod mod, ITagLoader<T> entrypoint, ModLoaderSystem modLoader) {
        entrypoint.LoadTags(Database);
    }
    
}