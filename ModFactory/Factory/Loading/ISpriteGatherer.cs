using System.Collections.Generic;
using JustLoaded.Content;

namespace Template.Factory.Loading;

public interface ISpriteGatherer {

    public IEnumerable<string> GetSprites();

}