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

    // �X�e�[�^�X
    public WeaponStatus weaponStatus;

    public float cooldowntimer;
    public float Usingtimer;
    public bool usecheck;

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
    public virtual void SetUp()
    {
        Debug.Log($"{weaponStatus.WeaponName} initialized with Power: {weaponStatus.AttackPower}");
    }


    // UpdateCooldown���\�b�h�����ʂ̃��W�b�N
    public void UpdateCooldown()
    {
        bool cooldownend = TimerUtility.TimerCountDown(ref cooldowntimer, Time.deltaTime);

        if (!cooldownend)
        {
            // �N�[���_�E�����̏���
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

    // UseStart���\�b�h�͋��ʂ̃��W�b�N
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