using UnityEngine;

namespace Assets.Scripts.Actors
{
    public abstract class Entity : IUpdateBehaviour
    {
        public Vector3 Position { get; protected set; }

        public int Health { get; protected set; }

        public BaseWorld World { get; private set; }

        public bool IsEnemy { get; private set; }

        protected ActorView ActorView;

        public abstract void Update(float deltaTime);

        public Entity(BaseWorld world)
        {
            World = world;
        }

        public void SetIsEnemy(bool isEnemy)
        {
            IsEnemy = isEnemy;
        }

        public void SetView(ActorView actorView)
        {
            ActorView = actorView;
        }

        public virtual void SetPosition(Vector3 position)
        {
            Position = position;
            ActorView.transform.position = position;
        }

        public void SetHealth(int amount)
        {
            Health = amount;
        }

        public Bounds GetBounds()
        {
            if (ActorView == null)
            {
                return new Bounds();
            }

            var result = ActorView.GetComponentInChildren<Renderer>().bounds;

            return result;
        }

        public virtual void DealDamage(int amount)
        {
            Debug.Log("Recieved damage " + amount);

            Health -= amount;

            if (Health <= 0)
            {
                World.Entities.Remove(this);

                if (ActorView != null)
                {
                    Object.Destroy(ActorView.gameObject);
                }
            }
        }

        public virtual SelectionEventHandler GetSelectionEventHandler()
        {
            return new NullSelectionEventHandler();
        }

        public virtual EntityDisplayPanel GetDisplayPanelPrefab()
        {
            return null;
        }
    }
}
