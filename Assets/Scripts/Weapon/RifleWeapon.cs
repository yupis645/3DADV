using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : WeaponBase
{
    public int shotsFired;      //î≠éÀêî
    public List<Bullet> bullet = new List<Bullet>();
    

    protected override void OnAttackUpdate()
    {
        base.OnAttackUpdate();

    }

}
