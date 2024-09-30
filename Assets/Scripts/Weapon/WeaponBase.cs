using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected delegate void attackEvent();      //�U���C�x���g
    protected event attackEvent StartAttack;
    protected event attackEvent EndAttack;
    protected event attackEvent HitObject;
    protected event attackEvent AttackCancel;

    // �X�e�[�^�X
    public WeaponStatus weaponStatus;

    public Transform rockonTarget;

    public bool usecheck;           //�U���\��Ԃ̊m�F
    public bool reloadcheck;           

    protected Vector3 shotVec = Vector3.zero;

    // Initialize���\�b�h�̓T�u�N���X�ł��̂܂܎g�p����ꍇ
    public virtual void Initialize(WeaponStatus status)
    {
        this.weaponStatus = ScriptableObject.Instantiate(status);
        StartAttack += OnStartAttack;
        EndAttack += OnEndAttack;
        HitObject += OnHitObject;
        AttackCancel += OnAttackCancel;
    }

    // SetUp���\�b�h���T�u�N���X�ŃI�[�o�[���C�h�\
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

    // UseStart���\�b�h�͋��ʂ̃��W�b�N
    protected virtual void OnStartAttack()
    {
        shotVec = rockonTarget.position;          //���˕����̏I�_��ݒ�(���b�N�I�����Ă���Ώۂ̂������W)   
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