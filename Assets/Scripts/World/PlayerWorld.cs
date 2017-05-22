using System.Collections.Generic;
using Assets.Scripts.World.SocialModule;
using UnityEngine;

public class PlayerWorld : BaseWorld {

    #region constructor

    public PlayerWorld(List<Stockpile> stockpiles,RelationshipMap relationshipMap, Vector3 firePlace) : 
        base(stockpiles,relationshipMap, firePlace)
    {
    }

    #endregion

}
