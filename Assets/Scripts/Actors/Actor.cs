﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : Entity
{
    public Workplace Workplace { get; private set; }

    public bool IsOrderCompleted { get; private set; }

    public NavMeshAgent NavAgent { get { return ActorView.GetNavMeshAgent(); } }

    private ActorBehaviour _behaviour;

    public Actor(TestWorld world) : base(world)
    {

    }

    public void SetBehaviour(ActorBehaviour order)
    {
        _behaviour = order;
        _behaviour.SetActor(this);

        IsOrderCompleted = false;
    }

    public void SetWorkplace(Workplace workplace)
    {
        Workplace = workplace;
    }

    public override void Update(float deltaTime)
    {
        if (_behaviour == null || IsOrderCompleted)
        {
            return;
        }

        if (!_behaviour.Update(deltaTime))
        {
            IsOrderCompleted = true;
        }

        Position = NavAgent.transform.position;
    }

    public override void SetPosition(Vector3 position)
    {
        base.SetPosition(position);

        NavAgent.transform.position = position;
    }
}