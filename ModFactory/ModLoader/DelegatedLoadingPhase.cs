using System;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace Template.ModLoader;

public class DelegatedLoadingPhase : ILoadingPhase {

    private readonly Action<ModLoaderSystem> _action;
    
    public DelegatedLoadingPhase(Action<ModLoaderSystem> action) {
        this._action = action;
    }
    
    public void Load(ModLoaderSystem modLoader) {
        _action(modLoader);
    }
    
}