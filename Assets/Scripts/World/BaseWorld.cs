using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;
using World.SocialModule;

public class PerCitizenResourceController
{
    public int Amount => _values[_valueIndex];
    public int Popularity => _popularityValues[_valueIndex];

    private readonly int[] _values;
    private readonly int[] _popularityValues;

    private int _valueIndex;

    public PerCitizenResourceController(int[] values, int[] popularityValues)
    {
        _values = values;
        _popularityValues = popularityValues;
    }

    public int GetValuesCount()
    {
        return _values.Length;
    }

    public void SetValueIndex(int index)
    {
        _valueIndex = index.Clamped(0, _values.Length - 1);
    }

    public void OffsetValueIndex(int offset)
    {
        SetValueIndex(_valueIndex + offset);
    }
}

public class BaseWorld : IUpdateBehaviour
{
    public readonly EntityMapping EntityMapping;

    public int FreeCitizensCount => FreeCitizens.Count;

    public EntitiesController Entities { get; }
    public WorldEventsController Events { get; }
    public Stockpile Stockpile { get; }

    public int Population { get; protected set; }

    public int MaxPopulation { get; set; }

    public int Popularity => _popularityInternal.FloorToInt();

    public int MinPopulation { get; protected set; }

    public int Gold { get; protected set; }

    public readonly PerCitizenResourceController TaxController;
    public readonly PerCitizenResourceController FoodController;

    protected Vector3 FirePlace;
    protected Queue<Actor> FreeCitizens;
    protected RelationshipMap RelationshipMap;

    private readonly WorldInfo _worldInfo;
    private float _popularityInternal;

    private float _updateTimer = 0;

    public BaseWorld(WorldInfo worldInfo, RelationshipMap relationshipMap, Vector3 firePlace)
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

        TaxController = new PerCitizenResourceController(new[] {-2, -1, 0, 1, 2}, new[] {2, 1, 0, -1, -2});
        FoodController = new PerCitizenResourceController(new[] {0, 1, 2, 3}, new[] {-1, 0, 1, 2});
        
        TaxController.SetValueIndex(2);
        FoodController.SetValueIndex(1);

        _popularityInternal = _worldInfo.MaxPopularity;
    }

    public virtual void Update(float deltaTime)
    {
        Events.Update(deltaTime);
        Entities.Update(deltaTime);
        
        var popularity = TaxController.Popularity + FoodController.Popularity;
        _popularityInternal += popularity * Time.deltaTime;
        _popularityInternal = _popularityInternal.Clamped(_worldInfo.MinPopularity, _worldInfo.MaxPopularity);

        if (_updateTimer.FloorToInt() != (_updateTimer + deltaTime).FloorToInt())
        {
            Gold += TaxController.Amount * Population;
        }

        _updateTimer += deltaTime;

        UpdatePopulation();
    }

    public void RegisterFreeCitizen(Actor actor)
    {
        FreeCitizens.Enqueue(actor);
    }

    public Actor HireCitizen()
    {
        return FreeCitizens.Count > 0 ? FreeCitizens.Dequeue() : null;
    }

    public virtual Vector3 GetFireplace()
    {
        return FirePlace;
    }

    private void UpdatePopulation()
    {
        Population = Entities.GetItems().OfType<Actor>().Count();
    }
}