using System.Linq;
using UnityEngine;

public class PopularityEvent : WorldEvent
{
    private readonly Player _player;
    private readonly TestUnitFactory _unitFactory;
    private float _updatePeriod = 3f;
    private int _popularityStep = 2;
    private float _lastUpdateTime;
    private string _testFood = "Bread";

    #region constructors

    public PopularityEvent(BaseWorld gameWorld, Player player,
        TestUnitFactory unitFactory, float period) :
        base(gameWorld)
    {
        _updatePeriod = period;
        _player = player;
        _unitFactory = unitFactory;
    }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        _lastUpdateTime += deltaTime;
        if (_lastUpdateTime > _updatePeriod)
        {
            UpdatePopularity();
            _lastUpdateTime = 0;
        }
    }

    #endregion

    private void UpdatePopularity()
    {
        var citizensCount = _gameWorld.FreeCitizensCount;
        var foodAmount = _gameWorld.Stockpile.GetTotalResourceAmount(_testFood);
        
        if (foodAmount < citizensCount)
        {
            _player.ChangePopularity(-_popularityStep);
        }
        else
        {
            _player.ChangePopularity(_popularityStep);
        }
        
        if (_player.Popularity <= 0)
        {
            RemoveCitizen(_gameWorld);
        }
        else if (_player.Popularity > 50 && _gameWorld.Population < _gameWorld.PopulationLimit)
        {
            _unitFactory.CreateUnit(_unitFactory.UnitInfos.First(x => x.Name == "Peasant"));
        }

        _gameWorld.Stockpile.ChangeTotalResourceAmount(_testFood, -citizensCount);
    }

    private void RemoveCitizen(BaseWorld world)
    {
        var citizen = world.HireCitizen();
        if (citizen == null) return;
        _gameWorld.Entities.Remove(citizen);
    }
}