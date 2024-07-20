using System;
using System.Linq;
using System.Runtime.CompilerServices;
using JustLoaded.Content;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;

namespace Template.Factory;

public class RandomItemSpawner : ItemSpawnerObject {

    private DatabaseReference<Item>[] _items;
    
    public RandomItemSpawner(double cooldown, World world, params ContentKey[] items) : base(cooldown, world) {
        this._items = items.Select((key) => new DatabaseReference<Item>(key)).ToArray();
    }

    protected override Item GetNextItem() {
        return _items[Random.Shared.Next(_items.Length)].Value!;
    }
    
}