using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using UnityEngine;

public class BaseWorld : IWorld
{
    protected List<Stockpile> _stockpiles;
    private readonly Vector3 _firePlace;
    protected Queue<Actor> _freeCitizens ;

    #region constructor

    public BaseWorld(List<Stockpile> stockpiles,Vector3 firePlace)
    {
        _freeCitizens = new Queue<Actor>();
        _stockpiles = stockpiles;
        _firePlace = firePlace;
        Childs =  new List<IWorld>();
        EntitiesController = new EntitiesController();
        WorldEventsController = new WorldEventsController();
    }

    #endregion

    public int FreeCitizensCount
    {
        get
        {
            return _freeCitizens.Count;
        }
    }

    public EntitiesController EntitiesController { get; private set; }
    public WorldEventsController WorldEventsController { get; private set; }
    public Player Player { get; set; }
    public IList<IWorld> Childs { get; private set; }
    public IWorld Parent { get; set; }

    #region public methods

    public void Update(float deltaTime)
    {
        WorldEventsController.Update(deltaTime);
        EntitiesController.Update(deltaTime);
        UpdateChildWorlds(deltaTime);
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

    #region private methods

    protected void UpdateChildWorlds(float deltaTime)
    {
        for (int i = 0; i < Childs.Count; i++)
        {
            var world = Childs[i];
            world.Update(deltaTime);
        }
    }

    #endregion
}
