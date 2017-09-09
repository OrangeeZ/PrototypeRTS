namespace BehaviourStateMachine
{
    public interface IStateFactory<TStateType>
    {
        IStateBehaviour Create(TStateType state);
    }
}