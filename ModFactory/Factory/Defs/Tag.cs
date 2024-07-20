using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Template.Factory.Defs;

/// <summary>
/// Represents a tag for objects of type <typeparamref name="T"/>
/// </summary>
public class Tag<T> : HashSet<T> where T : class { }