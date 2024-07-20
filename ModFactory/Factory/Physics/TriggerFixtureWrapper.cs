using System;
using System.Collections.Generic;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Template.Factory.Physics;

public class TriggerFixtureWrapper {

    public event Action OnFirstEnter = delegate { };
    public event Action<Fixture, Contact> OnEnter = delegate { };
    public event Action<Fixture, Contact> OnExit = delegate { };
    public event Action OnLastExit = delegate { };
    public bool HasCollisions => _collisions.Count > 0;
    public int CollisionCount => _collisions.Count;
        
    private readonly HashSet<Contact> _collisions = new();
    
    public TriggerFixtureWrapper(Fixture fixture) {
        fixture.IsSensor = true;
        fixture.OnCollision = (sender, other, contact) => {
            if (_collisions.Count == 0) {
                OnFirstEnter();
            }

            if (!_collisions.Add(contact) && false) {
                throw new ApplicationException("Impossible?");
            }
            OnEnter(other, contact);
            return true;
        };
        
        fixture.OnSeparation = (sender, other, contact) => {
            if (!_collisions.Remove(contact)) {
                throw new ApplicationException("Impossible?");
            }
            
            OnExit(other, contact);
            if (_collisions.Count == 0) {
                OnLastExit();
            }
        };
    }
}