using System.Collections;
using Assets.Scripts.Actors;

namespace Assets.Scripts.Behaviour
{
    public abstract class ActorBehaviour
    {
        protected Actor Actor;

        protected float _deltaTime;

        private IEnumerator _routine;
        

        public void SetActor(Actor actor)
        {
            Actor = actor;
        }

        public bool Update(float deltaTime)
        {
            _deltaTime = deltaTime;

            _routine = _routine ?? UpdateRoutine();

            return _routine.MoveNext();
        }

        protected virtual IEnumerator UpdateRoutine()
        {
            yield return null;
        }
    }
}
