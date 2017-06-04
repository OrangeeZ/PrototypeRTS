using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebtEvent : WorldEvent
{
    private readonly Player _player;
    private float _timeLeft;

    public DebtEvent(BaseWorld gameWorld,Player player, float updatePeriod) :
        base(gameWorld)
    {
        _player = player;
        Period = updatePeriod;
    }

    #region public properties

    public float Period { get; set; }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        _timeLeft += deltaTime;
        if (_timeLeft < Period)
            return;
        _timeLeft = 0;
        ApplyDebt();
    }

    #endregion

    #region

    private void ApplyDebt()
    {
        var income = _gameWorld.PopulationLimit * _gameWorld.PublicDebt;
        _player.ChangePopularity(-_gameWorld.PublicDebt);
        _gameWorld.SetGold(_gameWorld.Gold + income);
    }

    #endregion
}
