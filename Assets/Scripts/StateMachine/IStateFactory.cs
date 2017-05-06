namespace Assets.Scripts.StateMachine
{
    public interface IStateFactory<TStateType>
    {
        IState Create(IStateController<TStateType> stateController,TStateType state);
    }
}