using System.Collections.Generic;

public class WorldBehaviourController<TBehaviour>
    where TBehaviour : IWorldUpdateBehaviour
{
    private List<TBehaviour> _behaviours = new List<TBehaviour>();
    private List<TBehaviour> _behavioursToRemove = new List<TBehaviour>();

    public IList<TBehaviour> GetItems()
    {
        return _behaviours;
    }

    public void AddItem(TBehaviour behaviour)
    {
        _behaviours.Add(behaviour);
    }

    public void RemoveItem(TBehaviour behaviour)
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
}