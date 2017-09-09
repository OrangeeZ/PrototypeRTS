using System.Collections;

namespace BehaviourStateMachine
{
    public abstract class StateBehaviour : IStateBehaviour
    {
        #region public methods
        
        public abstract IEnumerator Execute();

        public virtual void Stop() {}

        public virtual void Dispose()
        {
            Stop();
        }

        #endregion
    }
}
