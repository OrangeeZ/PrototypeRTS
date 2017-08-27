using System;
using System.Collections;
using System.Collections.Generic;
using csv;
using UnityEngine;
using Assets.Scripts.Actors;

public class UnitInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int Hp;

    [RemoteProperty]
    public int AttackRange;

    [RemoteProperty]
    public int AttackStrength;

    [RemoteProperty]
    public int AttackSpeed;

    [RemoteProperty]
    public string BehaviourId;

    [RemoteProperty]
    public ActorView Prefab;

    [RemoteProperty]
    public EntityDisplayPanel DisplayPanelPrefab;

    [RemoteProperty]
    public ResourceInfo RequiredWeaponId;

    [RemoteProperty]
    public ResourceInfo RequiredArmorId;
    
    public void Configure(Values values)
    {
    }
}