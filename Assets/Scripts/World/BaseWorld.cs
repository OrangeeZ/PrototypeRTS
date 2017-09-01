using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;
using World.SocialModule;

public class BaseWorld : IUpdateBehaviour
{
    public int FreeCitizensCount => FreeCitizens.Count;

    public BaseWorld Parent { get; set; }
    
    public EntitiesController Entities { get; private set; }
    public WorldEventsController Events { get; private set; }
    public WorldsController Children { get; private set; }
    public Stockpile Stockpile { get; private set; }
    public byte FactionId { get; private set; }

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
    
    protected Vector3 FirePlace;
    protected Queue<Actor> FreeCitizens;
    protected RelationshipMap RelationshipMap;

    public BaseWorld(RelationshipMap relationshipMap, Vector3 firePlace)
    {
        FreeCitizens = new Queue<Actor>();
        RelationshipMap = relationshipMap;
        FirePlace = firePlace;

        Stockpile = new Stockpile();
        Entities = new EntitiesController();
        Events = new WorldEventsController();
        Children = new WorldsController();
    }
    
    #region public methods

    public void SetFaction(byte factionId)
    {
        FactionId = factionId;
    }

    public RelationshipMap.RelationshipType GetRelationshipType(byte factionId)
    {
        return RelationshipMap.GetRelationshipType(FactionId, factionId);
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
        FreeCitizens.Enqueue(actor);
    }

    public void SetGold(int amount)
    {
        Gold = amount;
    }

    public Actor HireCitizen()
    {
        return FreeCitizens.Count > 0 ? FreeCitizens.Dequeue() : null;
    }

    public virtual Vector3 GetFireplace()
    {
        return FirePlace;
    }

    #endregion

    #region private methods

    private void UpdatePopulation()
    {
        Population = Entities.GetItems().OfType<Actor>().Count();
    }

    #endregion
}