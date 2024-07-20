using System;
using JustLoaded.Content;
using nkast.Aether.Physics2D.Dynamics;
using Template.Factory.Defs;

namespace Template.Factory;

public class ConstantItemSpawnerObject : ItemSpawnerObject {

    private DatabaseReference<Item> _itemRef;
    
    public ConstantItemSpawnerObject(ContentKey itemKey, double cooldown, World world) : base(cooldown, world) {
        this._itemRef = new DatabaseReference<Item>(itemKey);
    }
    
    protected override Item GetNextItem() {
        return _itemRef.Value ?? throw new ArgumentException($"Item { _itemRef } does not exist");
    }

}