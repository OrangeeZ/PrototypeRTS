using System.Collections.Generic;
using UnityEngine;

public class PlayerWorld : BaseWorld {

    #region constructor

    public PlayerWorld(List<Stockpile> stockpiles, Vector3 firePlace) : 
        base(stockpiles, firePlace)
    {
    }

    #endregion

}
