using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;
using World.SocialModule;

public class BaseWorld : IUpdateBehaviour
{
    public readonly EntityMapping EntityMapping;

    public int FreeCitizensCount => FreeCitizens.Count;

    public EntitiesController Entities { get; }
    public WorldEventsController Events { get; }
    public Stockpile Stockpile { get; }

    public int Population { get; protected set; }

    public int MaxPopulation { get; set; }

    public int Popularity { get; protected set; }

    public int MinPopulation { get; protected set; }

    public int Tax { get; set; }

    public int Gold { get; protected set; }
    
    protected Vector3 FirePlace;
    protected Queue<Actor> FreeCitizens;
    protected RelationshipMap RelationshipMap;
    
    private readonly WorldInfo _worldInfo;

    public BaseWorld(WorldInfo worldInfo,RelationshipMap relationshipMap, Vector3 firePlace)
    {
        _worldInfo = worldInfo;
        
        EntityMapping = new EntityMapping(relationshipMap);
        Entities = new EntitiesController(EntityMapping);
        FreeCitizens = new Queue<Actor>();
        Stockpile = new Stockpile();
        Events = new WorldEventsController();

        MaxPopulation = _worldInfo.MaxPopulation;
        RelationshipMap = relationshipMap;
        MinPopulation = _worldInfo.MinPopulation;
        FirePlace = firePlace;
        Popularity = _worldInfo.MaxPopularity;
    }

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

    private void UpdatePopulation()
    {
        Population = Entities.GetItems().OfType<Actor>().Count();
    }
}