using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    public abstract class Entity : IEntity
    {
        public Vector3 Position { get; protected set; }

        public TestWorld World { get; private set; }

        protected ActorView ActorView;

        public abstract void Update(float deltaTime);

        public Entity(TestWorld world)
        {
            World = world;
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
    }
}
