using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content;

namespace Template.Factory.Defs;

/// <summary>
/// Represents a tag for objects of type <typeparamref name="TContent"/>
/// </summary>
public class Tag<TContent> where TContent : class {

    private readonly HashSet<BoundContentKey<TContent>> _content = new();

    private readonly HashSet<Tag<TContent>> _dependants = new();

    public void AddDependency(Tag<TContent> dependency) {
        //Need to make sure this is correct
        if (IsDependant(dependency)) {
            //TODO exception
            throw new ApplicationException("Attempting to create tag dependency loop");
        }

        if (dependency.IsDependant(this)) {
            //TODO use logger
            Console.Error.WriteLine("Redundant tag relation");
            return;
        }

        dependency._dependants.Add(this);
    }
    public void Add(BoundContentKey<TContent> contentKey) {
        _content.Add(contentKey);
    }
    public bool Contains(BoundContentKey<TContent> key) {
        var flag = _content.Contains(key);
        foreach (var dependant in _dependants) {
            if (flag) {
                break;
            }
            
            flag |= dependant.Contains(key);
        }

        return flag;
    }
    
    private bool IsDependant(Tag<TContent> other) {
        var flag = _dependants.Contains(other);

        foreach(var dependant in _dependants) {
            if (flag) {
                break;
            }

            flag |= dependant.IsDependant(other);
        }

        return flag;
    }
}