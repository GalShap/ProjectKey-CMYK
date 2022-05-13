using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public abstract void Damage(int amount);

    public abstract void Heal(int amount);

    public abstract void SetHealth(int amount);

    public abstract int GetHealth();

    public abstract void Dead();
}

