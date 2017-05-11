using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorld : MonoBehaviour
    {
        public TestUnitFactory UnitFactory { get; private set; }

        public readonly EventSystem EventSystem = new EventSystem();
        private List<Entity> _entities = new List<Entity>();
        private List<Entity> _entitiesToRemove = new List<Entity>();
        private List<WorldEventBehaviour> _worldEventBehaviours = new List<WorldEventBehaviour>();
        private Queue<Actor> _freeCitizens = new Queue<Actor>();

        private ConstructionModule _constructionModule = new ConstructionModule();

        [SerializeField]
        private List<Stockpile> _stockpiles = new List<Stockpile>();

        [SerializeField]
        private Transform _fireplace;

        #region public methods

        public int FreeCitizensCount { get { return _freeCitizens.Count; } }

        void Awake()
        {
            UnitFactory = GetComponent<TestUnitFactory>();

            _constructionModule.SetWorld(this);
            _constructionModule.SetUnitFactory(UnitFactory);
        }

        public void RegisterFreeCitizen(Actor actor)
        {
            _freeCitizens.Enqueue(actor);
        }

        public void GetClosestStockpileWithResource(ResourceType resourceType)
        {

        }

        public Stockpile GetClosestStockpile(Vector3 position)
        {
            return _stockpiles.First();
        }

        public Actor GetFreeCitizen()
        {
            if (_freeCitizens.Any())
            {
                return _freeCitizens.Dequeue();
            }

            return null;
        }

        public Transform GetFireplace()
        {
            return _fireplace;
        }

        public void UpdateStep(float deltaTime)
        {
            UpdateWorldEvents(deltaTime);
            UpdateEntities(deltaTime);

            _constructionModule.Update(deltaTime);
        }

        public IList<Entity> GetEntities()
        {
            return _entities;//.OfType<Actor>();
        }

        public void AddWorldBehaviour(WorldEventBehaviour eventBehaviour)
        {
            _worldEventBehaviours.Add(eventBehaviour);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entitiesToRemove.Add(entity);
        }

        #endregion

        private void UpdateWorldEvents(float deltaTime)
        {
            for (int i = 0; i < _worldEventBehaviours.Count; i++)
            {
                var behaviour = _worldEventBehaviours[i];
                behaviour.Update(deltaTime);
            }
        }

        private void UpdateEntities(float deltaTime)
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                var each = _entities[i];
                each.Update(deltaTime);
            }

            for (var i = 0; i < _entitiesToRemove.Count; i++)
            {
                var each = _entitiesToRemove[i];
                _entities.Remove(each);
            }

            _entitiesToRemove.Clear();
        }

        void OnGUI()
        {
            _constructionModule.OnGUI();
        }

    }
}
