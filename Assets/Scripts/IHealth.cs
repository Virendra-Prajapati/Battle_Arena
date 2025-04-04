using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    void DealDamage(int damage, ulong clientId);
}
