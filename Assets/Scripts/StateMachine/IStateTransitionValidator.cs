namespace Assets.Scripts.StateMachine
{
    public interface IStateTransitionValidator<TStateType>  {

        /// <summary>
        /// calidate transition between state
        /// </summary>
        /// <param name="from">start state</param>
        /// <param name="to">target state</param>
        /// <returns>is transaction valid</returns>
        bool Validate(TStateType from, TStateType to);

    }
}
