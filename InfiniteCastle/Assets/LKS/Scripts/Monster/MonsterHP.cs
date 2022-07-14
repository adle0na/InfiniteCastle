using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHP : MonoBehaviour
{
    private IAttackable target;
    public Image hpBar;

    private void Awake()
    {
        target = GetComponentInParent<IAttackable>();
    }

    private void OnEnable()
    {
        if (target != null)
        {
            RefreshHPBar();
            target.onHpChange += RefreshHPBar;
        }
    }

    private void OnDisable()
    {
        target.onHpChange -= RefreshHPBar;
    }

    public void RefreshHPBar()
    {
        hpBar.fillAmount = (float) target.Health / target.MaxHealth;
    }
}
