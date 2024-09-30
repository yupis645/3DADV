using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected delegate void attackEvent();      //攻撃イベント
    protected event attackEvent StartAttack;
    protected event attackEvent EndAttack;
    protected event attackEvent HitObject;
    protected event attackEvent AttackCancel;

    // ステータス
    public WeaponStatus weaponStatus;

    public Transform rockonTarget;

    public bool usecheck;           //攻撃可能状態の確認
    public bool reloadcheck;           

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
    public virtual void SetUp(Transform target)
    {
        Debug.Log($"{weaponStatus.WeaponName} initialized with Power: {weaponStatus.AttackPower}");
        TargetSet(target);
    }

    public void TargetSet(Transform changetarget)
    {
        rockonTarget = changetarget;
    }



    protected virtual IEnumerator CooldownCoroutine(float cooldownDuration ,bool checkbool)
    {
        checkbool = false;
        yield return new WaitForSeconds(cooldownDuration);
        checkbool = true;
    }

    // UseStartメソッドは共通のロジック
    protected virtual void OnStartAttack()
    {
        shotVec = rockonTarget.position;          //発射方向の終点を設定(ロックオンしている対象のいた座標)   
        StartCoroutine(CooldownCoroutine(weaponStatus.UseTime, usecheck));
    }

    protected virtual void OnEndAttack()
    {
        StartCoroutine(CooldownCoroutine(weaponStatus.CoolDownTime,reloadcheck));
    }
    protected virtual void OnAttackUpdate()
    {

    }

    protected virtual void OnHitObject()
    {

    }
    protected virtual void OnAttackCancel()
    {
        EndAttack?.Invoke();
    }

}