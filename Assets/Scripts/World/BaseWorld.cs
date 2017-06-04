using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Assets.Scripts.World.SocialModule;
using UnityEngine;

public class BaseWorld : IUpdateBehaviour
{
    protected Vector3 _firePlace;
    protected Queue<Actor> _freeCitizens ;
    protected RelationshipMap _relationshipMap;

    #region constructor
    
    public BaseWorld(RelationshipMap relationshipMap,
        Vector3 firePlace)
    {
        _freeCitizens = new Queue<Actor>();
        _relationshipMap = relationshipMap;
        _firePlace = firePlace;
        Stockpile = new Stockpile();
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
    public Stockpile Stockpile { get; private set; }

    /// <summary>
    /// World faction type
    /// </summary>
    public int Faction { get { return _relationshipMap.Faction; } }

    /// <summary>
    /// current population
    /// </summary>
    public int Population { get; protected set; }

    /// <summary>
    /// max citizen count
    /// </summary>
    public int PopulationLimit { get; set; }

    /// <summary>
    /// debt for related to PopulationLimit
    /// </summary>
    public int PublicDebt { get; set; }
    
    /// <summary>
    /// world coins
    /// </summary>
    public int Gold { get; protected set; }

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
        UpdatePopulation();
    }

    public void RegisterFreeCitizen(Actor actor)
    {
        _freeCitizens.Enqueue(actor);
    }

    public void SetGold(int amount)
    {
        Gold = amount;
    }

    public void GetClosestStockpileWithResource(ResourceType resourceType)
    {
        
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

    #region private methods

    private void UpdatePopulation()
    {
        Population = Entities.Count;
    }

    #endregion
}
