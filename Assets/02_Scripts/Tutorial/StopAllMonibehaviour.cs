using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAllMonibihaviar : MonoBehaviour
{
    public List<MonoBehaviour> disableTargets;

    void Awake()
    {
        foreach (var target in disableTargets)
            target.enabled = false;
    }
}
