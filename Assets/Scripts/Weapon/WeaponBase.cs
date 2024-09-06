using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected delegate void attackEvent();
    protected event attackEvent StartAttack;
    protected event attackEvent EndAttack;
    protected event attackEvent HitObject;
    protected event attackEvent AttackCancel;

    // ステータス
    public WeaponStatus weaponStatus;

    public float cooldowntimer;
    public float Usingtimer;
    public bool usecheck;

    protected Vector3 shotVec = Vector3.zero;

    // Initializeメソッドはサブクラスでそのまま使用する場合
    public virtual void Initialize(WeaponStatus status)
    {
        this.weaponStatus = ScriptableObject.Instantiate(status);
        StartAttack += OnStartAttack;
        EndAttack += OnEndAttack;
        HitObject += OnHitObject;
        AttackCancel += OnAttackCancel;
    }

    // SetUpメソッドもサブクラスでオーバーライド可能
    public virtual void SetUp()
    {
        Debug.Log($"{weaponStatus.WeaponName} initialized with Power: {weaponStatus.AttackPower}");
    }


    // UpdateCooldownメソッドも共通のロジック
    public void UpdateCooldown()
    {
        bool cooldownend = TimerUtility.TimerCountDown(ref cooldowntimer, Time.deltaTime);

        if (!cooldownend)
        {
            // クールダウン中の処理
        }
        else
        {
            cooldowntimer = 0;
            usecheck = true;
        }
    }

    protected virtual IEnumerator CooldownCoroutine(float cooldownDuration)
    {
        usecheck = false;
        yield return new WaitForSeconds(cooldownDuration);
        usecheck = true;
    }

    // UseStartメソッドは共通のロジック
    protected virtual void OnStartAttack()
    {
        shotVec = transform.forward;
        Usingtimer = weaponStatus.UseTime;
        usecheck = false;
    }

    protected virtual void OnEndAttack()
    {
        StartCoroutine(CooldownCoroutine(weaponStatus.CoolDownTime));
        cooldowntimer = weaponStatus.CoolDownTime;
    }
      protected virtual void OnAttackUpdate()
    {
        cooldowntimer = weaponStatus.CoolDownTime;
    }

    protected virtual void OnHitObject()
    {

    }
    protected virtual void OnAttackCancel()
    {
        EndAttack?.Invoke();
    }

}