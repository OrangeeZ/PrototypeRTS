using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;

public class MultiDictionary<TKey, TValue>
{
    private static readonly TValue[] EmptyCollection = new TValue[0];
    private Dictionary<TKey, List<TValue>> _dictionary;

    public void Add(TKey key, TValue value)
    {
        var collection = default(List<TValue>);
        if (!_dictionary.TryGetValue(key, out collection))
        {
            collection = new List<TValue>();
        }

        collection.Add(value);
    }

    public void Remove(TKey key, TValue value)
    {
        var collection = default(List<TValue>);
        if (_dictionary.TryGetValue(key, out collection))
        {
            collection.Remove(value);
        }
    }

    public IList<TValue> Get(TKey key)
    {
        return _dictionary.ContainsKey(key) ? (IList<TValue>) _dictionary[key] : EmptyCollection;
    }
}

public class EntityMapping
{
    private readonly MultiDictionary<UnitInfo, Actor> _infoToActorMapping = new MultiDictionary<UnitInfo, Actor>();
    private readonly MultiDictionary<System.Type, Entity> _typeToEntityMapping = new MultiDictionary<System.Type, Entity>();

    public void AddActor(Actor actor)
    {
        _infoToActorMapping.Add(actor.Info, actor);
        _typeToEntityMapping.Add(actor.GetType(), actor);
    }

    public void RemoveActor(Actor actor)
    {
        _infoToActorMapping.Remove(actor.Info, actor);
        _typeToEntityMapping.Remove(actor.GetType(), actor);
    }

    public IList<Actor> GetActorsByInfo(UnitInfo info)
    {
        return _infoToActorMapping.Get(info);
    }

    public IEnumerable<TEntity> GetEntitiesByType<TEntity>() where TEntity : Entity
    {
        return _typeToEntityMapping.Get(typeof(TEntity)).OfType<TEntity>();
    }
}