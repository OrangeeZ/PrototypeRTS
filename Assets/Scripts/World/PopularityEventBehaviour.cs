using System.Linq;
using Assets.Scripts.World;
using UnityEngine;

public class PopularityEventBehaviour : WorldEventBehaviour
{
    private readonly Player _player;
    private readonly TestUnitFactory _unitFactory;
    private float _updatePeriod = 3f;
    private int _popularityStep = 2;
    private float _lastUpdateTime;

    #region constructors

    public PopularityEventBehaviour(TestWorld testWorld,Player player,TestUnitFactory unitFactory) : 
        base(testWorld)
    {
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
        var stockpile = _testWorld.GetClosestStockpile(Vector3.zero);
        var citizensCount = _testWorld.FreeCitizensCount;
        var foodAmount = stockpile["Bread"];
        if (foodAmount < citizensCount)
        {
            _player.SetPopularity(_player.Popularity - _popularityStep);
        }
        else
        {
            _player.SetPopularity(_player.Popularity + _popularityStep);
        }
        if (_player.Popularity == 0)
        {
            RemoveCitizen();
        }
        else if (_player.Popularity>50 && _testWorld.FreeCitizensCount < 10)
        {
            _unitFactory.CreateUnit(_unitFactory.UnitInfos.FirstOrDefault(x => x.Name == "Peasant"));
        }
    }

    private void RemoveCitizen()
    {
        var citizen = _testWorld.GetFreeCitizen();
        if(citizen==null)return;
        _testWorld.RemoveEntity(citizen);
    }
}
