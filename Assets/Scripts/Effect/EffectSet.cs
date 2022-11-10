using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    static EffectSet instance;
    public static EffectSet Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if(instance= null) {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }

    [Header("-------Monster")]
    public GameObject DuckAtkEffect;
    public GameObject DuckDmgEffect;

    [Header("-------Player")]
    public GameObject PlayerAtkEffect;
    public GameObject PlayerDmgEffect;

}
