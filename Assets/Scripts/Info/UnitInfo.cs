using System;
using System.Collections;
using System.Collections.Generic;
using csv;
using UnityEngine;
using Assets.Scripts.Actors;

public class UnitInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty("Name")]
    public string Name;

    [RemoteProperty("HP")]
    public int Hp;

    [RemoteProperty("AttackRange")]
    public int AttackRange;

    [RemoteProperty("AttackStrength")]
    public int AttackStrength;

    [RemoteProperty("AttackSpeed")]
    public int AttackSpeed;

    [RemoteProperty("BehaviourId")]
    public string BehaviourId;

    public ActorView Prefab;

    public EntityDisplayPanel DisplayPanelPrefab;

    public void Configure(Values values)
    {
        Prefab = values.GetPrefabWithComponent<ActorView>("Prefab", false);
        DisplayPanelPrefab = values.GetPrefabWithComponent<EntityDisplayPanel>("DisplayPanelPrefab", false);
    }
}
