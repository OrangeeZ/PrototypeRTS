using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.World;

public class PopularityEvent : WorldEvent
{
    private readonly Player _player;
    private readonly TestUnitFactory _unitFactory;
    private readonly WorldData _worldData;
    private float _updatePeriod = 3f;
    private int _inscreasePopularityStep = 1;
    private int _decreasePopularityStep = 2;
    private float _lastUpdateTime;
    private List<ResourceInfo> _foodInfos;

    #region constructors

    public PopularityEvent(BaseWorld gameWorld,
        WorldData worldData,
        Player player,
        TestUnitFactory unitFactory, float period) :
        base(gameWorld)
    {
        _worldData = worldData;
        _updatePeriod = period;
        _player = player;
        _unitFactory = unitFactory;
        _foodInfos = new List<ResourceInfo>(_worldData.ResourceInfos.
            Where(x => x.ResourceType == ResourceType.Food));
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
        UpdatePopularity(citizensCount);
        HireCitizen();
        ConsumeFood(citizensCount);
    }

    private void RemoveCitizen(BaseWorld world)
    {
        var citizen = world.HireCitizen();
        if (citizen == null) return;
        _gameWorld.Entities.Remove(citizen);
    }

    private void HireCitizen()
    {
        if (_player.Popularity <= 0)
        {
            RemoveCitizen(_gameWorld);
        }
        else if (_player.Popularity > 50 && _gameWorld.Population < _gameWorld.PopulationLimit)
        {
            _unitFactory.CreateUnit(_worldData.UnitInfos.First(x => x.Name == "Peasant"));
        }

    }

    private void UpdatePopularity(int citizensCount)
    {
        var foodVariety = 0;
        var foodAmount = _foodInfos.Sum(x =>
        {
            var amount = _gameWorld.Stockpile.GetTotalResourceAmount(x.Id);
            if (amount > 0)
                foodVariety++;
            return amount;
        });
        if (foodAmount < citizensCount)
        {
            _player.ChangePopularity(-_decreasePopularityStep);
        }
        else
        {
            _player.ChangePopularity(_inscreasePopularityStep + foodVariety);
        }
    }

    private void ConsumeFood(int requiredFood)
    {
        var consumedFood = 0;
        for (int i = 0; i < _foodInfos.Count; i++)
        {
            var food = _foodInfos[i];
            var foodCount = _gameWorld.Stockpile.GetTotalResourceAmount(food.Id);
            var foodTaken = foodCount + consumedFood > requiredFood ? requiredFood - foodCount : foodCount;
            consumedFood += foodTaken;
            _gameWorld.Stockpile.ChangeTotalResourceAmount(food.Id, -foodTaken);
        }

    }
}