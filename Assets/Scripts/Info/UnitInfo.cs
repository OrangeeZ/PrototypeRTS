using Actors;
using csv;
using UnityEngine;

public class UnitInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public int Hp;

    [RemoteProperty]
    public int AggressiveDetectionRange;
    
    [RemoteProperty]
    public int PassiveDetectionRange;
    
    [RemoteProperty]
    public int DefensiveDetectionRange;

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
    public ResourceInfo RequiredWeapon;

    [RemoteProperty]
    public ResourceInfo RequiredArmor;
    
    public void Configure(Values values)
    {
    }

    public void OnPostLoad()
    {
        
    }
}