using System.Collections;
using BehaviourStateMachine;

namespace States
{
    public class DummyState : IStateBehaviour
    {
        public IEnumerator Execute()
        {
            yield return null;
        }

        public void Stop()
        {
        }
    }
}
