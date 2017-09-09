namespace BehaviourStateMachine
{
    public interface IStateManager<TStateType>
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        void Dispose();
        void SetState(TStateType state);
    }
}