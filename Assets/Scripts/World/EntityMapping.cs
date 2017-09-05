using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;
using World.SocialModule;

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
    private readonly MultiDictionary<byte, Entity> _factionToEntityMapping = new MultiDictionary<byte, Entity>();

    private readonly RelationshipMap _relationshipMap;

    public EntityMapping(RelationshipMap relationshipMap)
    {
        _relationshipMap = relationshipMap;
    }

    public void AddEntity(Entity entity)
    {
        if (entity is Actor)
        {
            var actor = entity as Actor;
            _infoToActorMapping.Add(actor.Info, actor);
        }

        _typeToEntityMapping.Add(entity.GetType(), entity);
        _factionToEntityMapping.Add(entity.FactionId, entity);
    }

    public void RemoveEntity(Entity entity)
    {
        if (entity is Actor)
        {
            var actor = entity as Actor;
            _infoToActorMapping.Remove(actor.Info, actor);
        }

        _typeToEntityMapping.Remove(entity.GetType(), entity);
        _factionToEntityMapping.Add(entity.FactionId, entity);
    }

    public IList<Actor> GetActorsByInfo(UnitInfo info)
    {
        return _infoToActorMapping.Get(info);
    }

    public IEnumerable<TEntity> GetEntitiesByType<TEntity>() where TEntity : Entity
    {
        return _typeToEntityMapping.Get(typeof(TEntity)).OfType<TEntity>();
    }

    public IEnumerable<Entity> GetEntitiesByRelationship(byte factionId, RelationshipMap.RelationshipType relationshipType)
    {
        var factions = _relationshipMap.GetFactionsWithRelationship(factionId, relationshipType);

        return factions.SelectMany(_factionToEntityMapping.Get);
    }
}