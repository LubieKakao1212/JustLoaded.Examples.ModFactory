using JustLoaded.Content.Database;
using Template.Factory.Defs;

namespace Template.Factory.Loading;

public interface ITagLoader<T> where T: class {

    public void LoadTags(IContentDatabase<Tag<T>> tags);

}