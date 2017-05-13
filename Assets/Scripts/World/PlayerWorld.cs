using System.Collections.Generic;
using UnityEngine;

public class PlayerWorld : BaseWorld {

    private ConstructionModule _constructionModule;
    private readonly UnitCommandModule _commandModule;

    #region constructor

    public PlayerWorld(ConstructionModule constructionModule,
        UnitCommandModule commandModule,
        List<Stockpile> stockpiles, Vector3 firePlace) : 
        base(stockpiles, firePlace)
    {
        _constructionModule = constructionModule;
        _commandModule = commandModule;
    }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _constructionModule.Update(deltaTime);
        _commandModule.Update();

    }

    #endregion
}
