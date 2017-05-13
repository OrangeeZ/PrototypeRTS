using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using UnityEngine;

public class Building : Entity {

    public Building(BaseWorld world) : base(world)
    {
    }


    public BuildingInfo Info { get; private set; }

    #region public methods

    public override void Update(float deltaTime)
    {
        
    }
    
    public void SetInfo(BuildingInfo info)
    {
        Info = info;
    }

    public override EntityDisplayPanel GetDisplayPanelPrefab()
    {
        return Info.DisplayPanelPrefab;
    }

    #endregion
}
