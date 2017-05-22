using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Assets.Scripts.World.SocialModule;
using UnityEngine;

public class BaseWorld : IUpdateBehaviour
{
    protected List<Stockpile> _stockpiles;
    protected Vector3 _firePlace;
    protected Queue<Actor> _freeCitizens ;
    protected RelationshipMap _relationshipMap;

    #region constructor

    public BaseWorld(List<Stockpile> stockpiles,
        RelationshipMap relationshipMap,
        Vector3 firePlace)
    {
        _freeCitizens = new Queue<Actor>();
        _stockpiles = stockpiles;
        _relationshipMap = relationshipMap;
        _firePlace = firePlace;
        Entities = new EntitiesController();
        Events = new WorldEventsController();
        Children =  new WorldsController();

    }

    #endregion

    public int FreeCitizensCount
    {
        get
        {
            return _freeCitizens.Count;
        }
    }

    public EntitiesController Entities { get; private set; }
    public WorldEventsController Events { get; private set; }
    public WorldsController Children { get; private set; }
    public BaseWorld Parent { get; set; }
    public int Faction { get { return _relationshipMap.Faction; } }

    #region public methods

    public int GetRelationship(int faction)
    {
        return _relationshipMap.GetRelation(faction);
    }

    public virtual void Update(float deltaTime)
    {
        Events.Update(deltaTime);
        Entities.Update(deltaTime);
        Children.Update(deltaTime);
    }

    public void RegisterFreeCitizen(Actor actor)
    {
        _freeCitizens.Enqueue(actor);
    }

    public void GetClosestStockpileWithResource(ResourceType resourceType)
    {
        
    }

    public virtual Stockpile GetClosestStockpile(Vector3 position)
    {
        return _stockpiles.Count > 0 ? _stockpiles[0] : null;
    }

    public Actor HireCitizen()
    {
        return _freeCitizens.Count > 0 ? _freeCitizens.Dequeue() : null;
    }

    public virtual Vector3 GetFireplace()
    {
        return _firePlace;
    }

    #endregion
}
