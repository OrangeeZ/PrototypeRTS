using System.Collections;
using Actors;
<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
using Assets.Scripts.Actors;
=======
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286

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