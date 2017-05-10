using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using UnityEngine;

public class EntitiesBehaviour  {

    private List<Entity> _entities = new List<Entity>();
    private List<Entity> _entitiesToRemove = new List<Entity>();
    
    #region public methods
    
    public IList<Entity> GetEntities()
    {
        return _entities;//.OfType<Actor>();
    }

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        _entitiesToRemove.Add(entity);
    }
    
    public void Update(float deltaTime)
    {
        for (var i = 0; i < _entities.Count; i++)
        {
            var each = _entities[i];
            each.Update(deltaTime);
        }

        for (var i = 0; i < _entitiesToRemove.Count; i++)
        {
            var each = _entitiesToRemove[i];
            _entities.Remove(each);
        }

        _entitiesToRemove.Clear();
    }

    #endregion
}
