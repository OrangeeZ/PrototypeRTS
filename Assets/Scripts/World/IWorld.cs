using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using UnityEngine;

public interface IWorld : IWorldUpdateBehaviour
{
    #region public properties

    int FreeCitizensCount { get; }

    EntitiesController EntitiesController { get;}

    WorldEventsController WorldEventsController { get; }

    Player Player { get; set; }

    IList<IWorld> Childs { get; }

    IWorld Parent { get; set; }

    #endregion

    #region public methods

    void RegisterFreeCitizen(Actor actor);
    void GetClosestStockpileWithResource(ResourceType resourceType);
    Stockpile GetClosestStockpile(Vector3 position);
    Actor HireCitizen();
    Vector3 GetFireplace();

    #endregion
}
