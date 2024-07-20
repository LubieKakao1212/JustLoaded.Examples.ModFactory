using System;
using System.Collections.Generic;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace Template.ModLoader;

/// <summary>
/// Used to execute several phases in sequence
/// </summary>
public class ChainLoadingPhase : ILoadingPhase {

    private readonly List<Action<ModLoaderSystem>> _phases = new();

    public void AddPhase(Action<ModLoaderSystem> phase) {
        _phases.Add(phase);
    }

    public void AddPhase(ILoadingPhase phase) {
        _phases.Add(phase.Load);
    }
    
    public void Load(ModLoaderSystem modLoader) {
        foreach (var phase in _phases) {
            phase(modLoader);
        }
    }
    
}