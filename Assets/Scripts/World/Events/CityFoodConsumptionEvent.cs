using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.World;

public class CityFoodConsumptionEvent : WorldEvent
{
    private readonly WorldData _worldData;
    private readonly float _updatePeriod = 3f;
    private readonly List<ResourceInfo> _foodInfos;

    private float _lastUpdateTime;
    
    #region constructors

    public CityFoodConsumptionEvent(BaseWorld world,
        WorldData worldData,float period) :
        base(world)
    {
        _worldData = worldData;
        _updatePeriod = period;
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
            var citizensCount = _world.FreeCitizensCount;
            ConsumeFood(citizensCount);
            _lastUpdateTime = 0;
        }
    }

    #endregion

    private void ConsumeFood(int requiredFood)
    {
        var consumedFood = 0;
        for (int i = 0; i < _foodInfos.Count; i++)
        {
            var food = _foodInfos[i];
            var foodCount = _world.Stockpile.GetTotalResourceAmount(food.Id);
            var foodTaken = foodCount + consumedFood > requiredFood ? requiredFood - foodCount : foodCount;
            consumedFood += foodTaken;
            _world.Stockpile.ChangeTotalResourceAmount(food.Id, -foodTaken);
        }

    }
}