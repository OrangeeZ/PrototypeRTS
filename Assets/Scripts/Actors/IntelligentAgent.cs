using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelligentAgent
{
    public bool IsOrderCompleted { get; private set; }

    private Order _order;

    private Actor _targetActor;

    public IntelligentAgent(Actor targetActor)
    {
        _targetActor = targetActor;
    }

    public void SetOrder(Order order)
    {
        _order = order;
		_order.SetActor(_targetActor);

        IsOrderCompleted = false;
    }

    public void Update(float deltaTime)
    {
        if (_order == null || IsOrderCompleted)
        {
            return;
        }

        if (!_order.Update(deltaTime))
        {
            IsOrderCompleted = true;
        }
    }
}
