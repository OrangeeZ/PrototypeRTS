using System;
using System.Collections;
using System.Collections.Generic;
using csv;
using UnityEngine;

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

    public void Configure(Values values)
    {
    }
}
