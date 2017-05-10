using System.Collections.Generic;

public class GameWorld
{
    #region private properties

    private List<WorldEventBehaviour> _worldEventBehaviours = new List<WorldEventBehaviour>();

    #endregion

    #region constructor

    public GameWorld(EntitiesBehaviour entitiesBehaviour,List<Player> players,Player activePlayer)
    {
        EntitiesBehaviour = entitiesBehaviour;
        Players = new List<Player>();
        Players.AddRange(players);
        ActivePlayer = activePlayer;
    }

    #endregion

    #region public properties

    public EntitiesBehaviour EntitiesBehaviour { get; protected set; }

    public List<Player> Players { get; protected set; }

    public Player ActivePlayer { get; protected set; }

    #endregion

    #region public methods

    public void AddWorldBehaviour(WorldEventBehaviour eventBehaviour)
    {
        _worldEventBehaviours.Add(eventBehaviour);
    }

    public void UpdateStep(float deltaTime)
    {
        UpdateWorldEvents(deltaTime);
        EntitiesBehaviour.Update(deltaTime);
    }

    #endregion

    #region private methods
    
    private void UpdateWorldEvents(float deltaTime)
    {
        for (var i = 0; i < _worldEventBehaviours.Count; i++)
        {
            var behaviour = _worldEventBehaviours[i];
            behaviour.Update(deltaTime);
        }
    }

    #endregion

}
