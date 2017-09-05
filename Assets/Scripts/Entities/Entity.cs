using UnityEngine;

namespace Actors
{
    public abstract class Entity : IUpdateBehaviour
    {
        public Vector3 Position { get; protected set; }

        public int Health { get; protected set; }

        public BaseWorld World { get; private set; }

        public byte FactionId { get; private set; }

        #warning IsEnemy is a stub for backwards compatibility; will later be replaced with FactionId things
        public bool IsEnemy => FactionId != 0;
        
        /// <summary>
        /// is building active
        /// </summary>
        public bool IsActive { get; protected set; }

        protected ActorView ActorView;
        
        #region public methods

        public abstract void Update(float deltaTime);

        public Entity(BaseWorld world)
        {
            World = world;
        }

        public void SetFactionId(byte factionId)
        {
            FactionId = factionId;
            ActorView.SetIsEnemy(IsEnemy);
        }

        public void SetState(bool active)
        {
            IsActive = active;
            if (IsActive)
            {
                Activate();
                return;
            }
            Deactivate();
        }

        public void SetView(ActorView actorView)
        {
            Object.Destroy(ActorView?.gameObject);
            
            ActorView = actorView;
            ActorView.transform.position = Position;
            ActorView.SetIsEnemy(IsEnemy);
        }

        public virtual void SetPosition(Vector3 position)
        {
            Position = position;

            if (ActorView != null)
            {
                ActorView.transform.position = position;
            }
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

        public virtual void DealDamage(int amount, Entity damageSource)
        {
            Debug.Log($"Recieved damage {amount}");

            Health -= amount;

            if (Health <= 0)
            {
                Kill();
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

        public virtual void Kill()
        {
            World.Entities.Remove(this);
            if (ActorView != null)
            {
                Object.Destroy(ActorView.gameObject);
            }
        }

        #endregion

        protected virtual void Activate()
        {
            
        }

        protected virtual void Deactivate()
        {
            
        }
    }
}
