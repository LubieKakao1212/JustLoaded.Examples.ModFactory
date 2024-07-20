using System.Collections.Generic;
using JustLoaded.Content;

namespace Template.Factory.Loading;

public interface ITmxMapGatherer {

    public IEnumerable<string> GetMaps();

}