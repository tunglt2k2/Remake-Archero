using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }
    private static EffectSet instance;

    [Header("Monster")]
    public GameObject DuckAttackEffect;
    public GameObject DuckDmgEffect;

    [Header("Player")]
    public GameObject PlayerAttackEffect;
    public GameObject PlayerDmgEffect;


}
