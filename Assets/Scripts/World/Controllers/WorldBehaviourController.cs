using System.Collections.Generic;

public class WorldBehaviourController<TBehaviour>
    where TBehaviour : IUpdateBehaviour
{
    private List<TBehaviour> _behaviours = new List<TBehaviour>();
    private List<TBehaviour> _behavioursToRemove = new List<TBehaviour>();

    #region public properties

    public int Count { get { return _behaviours.Count; } }

    public TBehaviour this[int index]
    {
        get { return _behaviours[index]; }
    }

    #endregion

    #region public methods

    public IList<TBehaviour> GetItems()
    {
        return _behaviours;
    }

    public void Add(TBehaviour behaviour)
    {
        _behaviours.Add(behaviour);
    }

    public void Remove(TBehaviour behaviour)
    {
        _behavioursToRemove.Add(behaviour);
    }

    public void Update(float deltaTime)
    {
        for (var i = 0; i < _behaviours.Count; i++)
        {
            var each = _behaviours[i];
            each.Update(deltaTime);
        }

        for (var i = 0; i < _behavioursToRemove.Count; i++)
        {
            var each = _behavioursToRemove[i];
            _behaviours.Remove(each);
        }

        _behavioursToRemove.Clear();
    }

    #endregion
}