using System.Collections;
using Actors;

namespace Behaviour
{
    public abstract class ActorBehaviour
    {
        protected Actor Actor;

        protected float DeltaTime;

        private IEnumerator _routine;

        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void OnActorDamageReceiveDamage(Entity damageSource)
        {
        }

        public void SetActor(Actor actor)
        {
            Actor = actor;
        }

        public bool Update(float deltaTime)
        {
            DeltaTime = deltaTime;

            _routine = _routine ?? UpdateRoutine();

            return _routine.MoveNext();
        }

        protected virtual IEnumerator UpdateRoutine()
        {
            yield return null;
        }
    }

    public class NullActorBehaviour : ActorBehaviour
    {
    }
}