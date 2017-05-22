using System.Collections.Generic;
using Assets.Scripts.World.SocialModule;
using UnityEngine;

public class PlayerWorld : BaseWorld {

    #region constructor

    public PlayerWorld(RelationshipMap relationshipMap, Vector3 firePlace) :
        base(relationshipMap, firePlace)
    {
        
    }

    #endregion

}
