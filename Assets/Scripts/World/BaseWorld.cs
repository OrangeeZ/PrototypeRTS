using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;
using World.SocialModule;

public class BaseWorld : IUpdateBehaviour
{
    protected Vector3 FirePlace;
    protected Queue<Actor> FreeCitizens;
    private WorldInfo _worldInfo;
    protected RelationshipMap RelationshipMap;
    
    public readonly EntityMapping EntityMapping;

    public int FreeCitizensCount => FreeCitizens.Count;

    public EntitiesController Entities { get; private set; }
    public WorldEventsController Events { get; private set; }
    public Stockpile Stockpile { get; private set; }

    /// <summary>
    /// current population
    /// </summary>
    public int Population { get; protected set; }

    /// <summary>
    /// max citizen count
    /// </summary>
    public int PopulationLimit { get; set; }


    public int Popularity { get; protected set; }

    /// <summary>
    /// debt for related to PopulationLimit
    /// </summary>
    public int Tax { get; set; }

    /// <summary>
    /// world coins
    /// </summary>
    public int Gold { get; protected set; }
    

    public BaseWorld(WorldInfo worldInfo,RelationshipMap relationshipMap, Vector3 firePlace)
    {
        EntityMapping = new EntityMapping(relationshipMap);
        Entities = new EntitiesController(EntityMapping);
        
        FreeCitizens = new Queue<Actor>();
        _worldInfo = worldInfo;
        RelationshipMap = relationshipMap;
        FirePlace = firePlace;
        Popularity = _worldInfo.MaxPopularity;

        Stockpile = new Stockpile();
        Events = new WorldEventsController();
    }
    
    #region public methods

    public virtual void Update(float deltaTime)
    {
        Events.Update(deltaTime);
        Entities.Update(deltaTime);
        
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

    public int ChangePopularity(int popularity)
    {
        var result = Popularity + popularity;
        Popularity = Mathf.Clamp(result, _worldInfo.MinPopularity, _worldInfo.MaxPopularity);
        return Popularity;
    }

    #endregion

    #region private methods

    private void UpdatePopulation()
    {
        Population = Entities.GetItems().OfType<Actor>().Count();
    }

    #endregion
}