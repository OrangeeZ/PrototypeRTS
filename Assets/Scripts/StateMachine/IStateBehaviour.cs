namespace BehaviourStateMachine
{
    public interface IStateBehaviour : ICommandRoutine
    {
        void Stop();
    }
}