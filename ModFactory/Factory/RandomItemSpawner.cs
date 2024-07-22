using System;
using System.Linq;
using JustLoaded.Content;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;

namespace Template.Factory;

public class RandomItemSpawner : ItemSpawnerObject {

    private readonly DatabaseReference<Item>[] _items;
    
    public RandomItemSpawner(double cooldown, World world, params ContentKey[] items) : base(cooldown, world) {
        _items = items.Select((key) => new DatabaseReference<Item>(BoundContentKey<Item>.Make(key))).ToArray();
    }

    protected override Item GetNextItem() {
        return _items[Random.Shared.Next(_items.Length)].Value!;
    }
    
}