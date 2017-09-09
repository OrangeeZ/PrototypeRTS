using System.Linq;

namespace Assets.Scripts.World.Events
{
    public class HireCitizenEvent : WorldEvent

    {
        private readonly Player _player;
        private readonly TestUnitFactory _unitFactory;
        private readonly WorldData _worldData;
        private float _updatePeriod = 3f;
        private float _lastUpdateTime;

        #region constructors

        public HireCitizenEvent(BaseWorld world,
            WorldData worldData,
            Player player,
            TestUnitFactory unitFactory, float period) :
            base(world)
        {
            _worldData = worldData;
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
                HireCitizen();
                _lastUpdateTime = 0;
            }
        }

        #endregion

        private void HireCitizen()
        {
            if (_world.Popularity <= 0 && _world.Population > _world.MinPopulation)
            {
                RemoveCitizen();
            }
            else if (CanHire())
            {
                _unitFactory.CreateUnit(_worldData.UnitInfos.First(x => x.Name == "Peasant"));
            }
        }

        private bool CanHire()
        {
            return (_player.World.Popularity > 50 && _world.Population < _world.MaxPopulation) ||
                   _world.Population < _world.MinPopulation;
        }

        private void RemoveCitizen()
        {
            var citizen = _world.HireCitizen();
            if (citizen == null) return;
            _world.Entities.Remove(citizen);
        }

    }
}
