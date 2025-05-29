using System;
using System.Collections;
using System.Collections.Generic;
using Snowman;
using UnityEngine;

public class SnowmanTakeDamage : MonoBehaviour
{
    private BaseSnowman _baseSnowman;

    private void Awake()
    {
        _baseSnowman = GetComponentInParent<BaseSnowman>();
    }

    public void TakeDamage(float damage)
    {
        _baseSnowman.TakeDamage(damage);
    }
}
