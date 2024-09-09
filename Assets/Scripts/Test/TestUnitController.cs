using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.Archive;
using Unity.VisualScripting;
using UnityEngine;

public class TestUnitController : MonoBehaviour
{
    [SerializeField] AssembledUnit unit;                    //���삷�郆�j�b�g�̏��

    [SerializeField] MyRigidBody rb = new MyRigidBody();    //����̕����G���W��

    protected BoxCollider obj_hitbox;                       //�����蔻���

    protected RaycastHit underRay;                          //�ڒn������Ƃ邽�߂�Ray
    [SerializeField] protected float underRayDistance = 0;  //underRay�̔�΂�����
    [SerializeField] protected bool isGround = false;                    //�ڒn��Ԃ̃t���O

    protected RaycastHit sideRay;                          //�ڒn������Ƃ邽�߂�Ray
    [SerializeField] protected float sideRayDistance = 0;  //underRay�̔�΂�����
    [SerializeField] protected bool isCollisionWall = false;                    //�ڒn��Ԃ̃t���O

    private Vector3 moveStartPos = Vector3.zero;

    //���I�ɕύX�����C�x���g�X�e�[�^�X�p�̃C�x���g
    protected delegate void activeMoveEvent(moveState newState);  
    protected event activeMoveEvent ActiveMoveState;
    protected event activeMoveEvent DeactivatedMoveState;
  
    [Flags]
    protected enum moveState                         //����A�N�V�����̃t���O
    {
        frozon = 0,
        move = 1,
        dash = 2,
        jump = 4,
        attack = 8,
        knockback = 16,
        down = 32,
    }
    [SerializeField]protected moveState activeMove;

    protected enum attackState                      //�U���A�N�V�����̎��
    {
        none,
        rightarms,
        leftarms,
        backrightarms,
        backleftarms,
        specialattack,
    }
    [SerializeField] protected attackState activeAttack;

    public LayerMask GroundMask;        //�ڒn������Ƃ�郌�C���[

    protected Dictionary<moveState, float> MoveActionCoolTime;          //���̍s���Ɉڂ��܂ł̃N�[���^�C��
    [SerializeField]protected float actionTime = 0;                      //�����A�N�V�����̑S�̎���
    [SerializeField] protected int jumpCount = 0;                        //�W�����v�񐔂̃J�E���g


    private static readonly Color BoundsColor = new Color(1, 0, 0, 0.5f);



    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //==============================================================================================================
    //            ������
    //
    //==============================================================================================================
    private void Awake()
    {
        unit.initialize();                                      //���j�b�g���̏�����
        ActiveMoveState += ActiveMoveStateEvent;                //�A�N�V�����t���O��ON�g���K�[�C�x���g�̒ǉ�
        DeactivatedMoveState += DeactivatedMoveStateEvent;      //�A�N�V�����t���O��OFF�g���K�[�C�x���g�̒ǉ�
        rb.SetUp(transform.position,                             // �����̈ʒu�� MyRigidbody �ɐݒ�
                true,       //���ʂ̓K��
                true,       //��R�̓K��
                true,       //�d�͂̓K��
                new Vector3(unit.status.MoveSpeedLimit, float.MaxValue, unit.status.MoveSpeedLimit),                    //�����A���������ւ̍ő�ړ����x
                new Vector3(unit.status.MoveSpeed, unit.status.JumpPower, unit.status.MoveSpeed) * Time.deltaTime);     //�����A���������ւ̍ŏ��ړ����x

        obj_hitbox = GetComponent<BoxCollider>();       //�����蔻��̎擾
        activeMove = (moveState)0x00;                   //�ړ��A�N�V�����̃t���O��������
        activeAttack = 0;

        //�e�A�N�V�����̃N�[���^�C����������
        MoveActionCoolTime = new Dictionary<moveState, float>   //�e�A�N�V�����̖��O��float�l��dictionary�ɃZ�b�g
        {
            //{ moveState.frozon, 0.0f },
            //{ moveState.move, 0.0f },
            { moveState.dash, 0.0f },
            { moveState.jump, 0.0f },
            { moveState.attack, 0.0f },
            { moveState.knockback, 0.0f },
            { moveState.down, 0.0f }
        };



    }


    //==============================================================================================================
    //            �ړ��X�e�[�gON���\�b�h
    //
    // �����Ŏw�肳�ꂽ�X�e�[�g�ɕύX���A�X�e�[�g�ύX�C�x���g�������ɔ��΂�����
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void AcitveMoveStateTrigger(moveState state)
    {
        if (MoveActionCoolTime.ContainsKey(state))
        {
            if (MoveActionCoolTime[state] > 0)     //�N�[���^�C����'0'(�����l,�t���OOFF)�ɂȂ��Ă���Ȃ�
            {
                Debug.Log($"{state} �N�[���_�E���� ");
                return;
            }
        }
        ActiveMoveState?.Invoke(state);     //ON�g���K�[�C�x���g�𔭉΂�����
     
    }

    //==============================================================================================================
    //            �X�e�[�g�ύX�C�x���g
    //
    // �A�N�V�����X�e�[�g�ύX��A��x�����ʂ郁�\�b�h
    //==============================================================================================================
    void ActiveMoveStateEvent(moveState state)
    {
        switch (state)
        {
            case moveState.frozon:
                activeMove = 0;      //�S�Ẵt���O��0�ŏ���������
                break;

            case moveState.move:
                //�_�b�V������move��1�ɂ����Ȃ�
                if (activeMove.HasFlag(moveState.dash) || activeMove.HasFlag(moveState.attack)) return;
                break;

            case moveState.dash:
                activeMove &= ~(moveState.move | moveState.dash | moveState.jump);

                //�_�b�V�����Ԃ�ݒ肷��
                if (isGround) actionTime = unit.status.DashRange;     //�n��ɂ���ꍇ
                else actionTime = unit.status.AirRange;               //�󒆂ɂ���ꍇ
                rb.Velocity = Vector3.zero;
                Vector3 dashVec = new Vector3(
                    InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveHorizontal(),   
                    0,
                    InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveVertical()
                    ) ;
                rb.AddForce(dashVec * unit.status.DashSpeed, ForceMode.Impulse);
                rb.OnMass_Drag_Gravity(true, false, false);     //MyRightbody�̗L����


                break;

            case moveState.jump:
                if (jumpCount >= unit.status.MaxJumpCount               //�W�����v�J�E���g���ő�W�����v�񐔂𒴂��Ă���ꍇ�A
                    || activeMove.HasFlag(moveState.dash)) return;     //�������̓_�b�V���t���O��"1"�̏ꍇ�A�W�����v�t���O�͗��ĂȂ�(�W�����v�ł��Ȃ�)
                else
                {
                    rb.AddForce(new Vector3(0,unit.status.JumpPower,0), ForceMode.Impulse);
                    jumpCount++;
                }
                break;

            case moveState.attack:
                activeMove &= ~(moveState.move | moveState.dash | moveState.jump);
                rb.Velocity.y = isGround ? 0 : rb.OnGravity();
                actionTime += unit.weapons[(int)activeAttack - 1].CoolDownTime;
                Debug.Log($"{unit.weapons[(int)activeAttack - 1].WeaponName}");
                rb.OnMass_Drag_Gravity(true, true, false);
                break;
            case moveState.knockback:

                break;
            case moveState.down:

                break;

            // ����ȊO�̏ꍇ�̏���
            default:
                break;
        }

        activeMove |= state;    //�w�肳�ꂽ�X�e�[�g��"1"�ɂ���
    }

    //==============================================================================================================
    //            �ړ��X�e�[�gOFF���\�b�h
    //
    // �����Ŏw�肳�ꂽ�X�e�[�g�ɕύX���A�X�e�[�g�ύX�C�x���g�������ɔ��΂�����
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void DeactivatedMoveStateTrigger(moveState state)
    {
        DeactivatedMoveState?.Invoke(state);
    }

    //==============================================================================================================
    //            �ړ��X�e�[�gOFF�C�x���g
    //
    //==============================================================================================================
    void DeactivatedMoveStateEvent(moveState state)
    {
        switch (state)
        {
            case moveState.frozon:
                break;

            case moveState.move:
                break;

            case moveState.dash:
                actionTime = 0;
                rb.OnMass_Drag_Gravity(true, true, true);
                CooldownCoroutine(moveState.dash, 1.0f);
                //SetMoveStateDuration(moveState.dash, 1.0f);

                break;

            case moveState.jump:

                break;

            case moveState.attack:
                FlagsBitUtility.RemoveFlagsBit(ref activeMove, moveState.move, moveState.dash, moveState.jump);      //�ړ��A�_�b�V���A�W�����v�𖳌��ɂ���
                actionTime = 0;                                                 //�A�N�V�����^�C����'0'�ŏ���������
                rb.OnMass_Drag_Gravity(true, true, true);                       //MyRigitbody�̑S�Ă�ON�ɂ���
                float cooltime = unit.weapons[(int)activeAttack - 1].CoolDownTime;    //�g�p����̃N�[���^�C���̒l���擾����
                Debug.Log($"{unit.weapons[(int)activeAttack - 1].WeaponName}.FriezeTime = {cooltime}");
                StartCooldown(moveState.attack, cooltime);

                OnaAttack(attackState.none);                                    //�A�^�b�N�X�e�[�g��none�ɂ���
                break;
            case moveState.knockback:

                break;
            case moveState.down:

                break;


            // ����ȊO�̏ꍇ�̏���
            default:
                break;
        }

         activeMove &= ~state;      //�w�肳�ꂽ�X�e�[�g��"0"�ɂ���
    }

    //==============================================================================================================
    //            �L�����N�^�[�̍X�V����
    //
    //  
    //==============================================================================================================
    protected void CharacterUpdate()
    {
        //Ray�����
         RayShot();

        if (activeMove.HasFlag(moveState.move))
        {
            //// �ړ������̌v�Z
            Vector3 movement = Vector3.zero;
            movement.x = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveHorizontal();
            movement.z = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveVertical();

            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            movement = cameraForward * movement.z + Camera.main.transform.right * movement.x;       //


            move(movement);
        }
    }

    protected void PositionFixedUpdate()
    {

        if (activeMove.HasFlag(moveState.dash)) if(Dash()) DeactivatedMoveStateTrigger(moveState.dash);    //�_�b�V���X�e�[�g��OFF�ɂ���;

        if (activeMove.HasFlag(moveState.jump)) if (rb.Velocity.y < -0) DeactivatedMoveStateTrigger(moveState.jump);

        if (activeMove.HasFlag(moveState.attack)) if(Attack()) DeactivatedMoveStateTrigger(moveState.attack);      //�A�^�b�N�X�e�[�g��OFF�ɂ���;


        // �d�͂̓K�p
        if (rb.onGravity) rb.ApplyGravity(-rb.OnGravity());

        //�ڒn & �W�����v��t���O��false 
        if (isGround && !activeMove.HasFlag(moveState.jump))
        {
            jumpCount = 0;
            rb.Velocity.y = 0;
        }
        // �h���b�O�̓K�p
        if(rb.onDrag)rb.UpdateDrag(Time.fixedDeltaTime,rb.OnDrag());

        // �ʒu�̍X�V
        rb.Position = PositionhAdjust(transform.position);
        rb.UpdatePosition(Time.fixedDeltaTime);
        transform.position = rb.Position;           // �ʒu�� transform �ɔ��f

        // ��]�̍X�V�i�K�v�ɉ����āj
        rb.UpdateRotation(Time.fixedDeltaTime);
        transform.rotation = rb.Rotation;
    }


    //==============================================================================================================
    //                  �ړ�
    //
    //  �ړ��x�N�g�����O�ȊO�̏ꍇ�A����������B�܂����̎��̑��x������𒴂��Ă���Ȃ����������O�̒l�ɖ߂�
    //==============================================================================================================
    void move(Vector3 movement)
    {
        // ���͂Ɋ�Â��ė͂�K�p
        if (movement.magnitude > 0)
        {
            // �͂̑傫���𒲐��i�Ⴆ��10�{�j
            rb.AddForce(movement * unit.status.MoveSpeed, ForceMode.Force);
        }

    }

    //==============================================================================================================
    //                  �_�b�V��
    //
    //  �A�N�V�����^�C�����O�ɂȂ�܂ł͈��̑��x��Ԃ�������
    //==============================================================================================================
    bool Dash()
    {
        return TimerUtility.TimerCountDown(ref actionTime, Time.deltaTime);     //�^�C�}�[���J�E���g�_�E������
    }

    //==============================================================================================================
    //                  �A�^�b�N
    //
    //  �A�N�V�����^�C�����O�ɂȂ�܂ł͈��̗������x��Ԃ�������
    //==============================================================================================================
    bool Attack()
    {
       return TimerUtility.TimerCountDown(ref actionTime, Time.deltaTime);

    }

    //==============================================================================================================
    //                  �m�b�N�o�b�N
    //==============================================================================================================
    void Knockback()
    {

    }

    //==============================================================================================================
    //                  �_�E��
    //==============================================================================================================
    void Down()
    {

    }


    //==============================================================================================================
    //                 �w�肵���U���X�e�[�g�ɕύX
    //==============================================================================================================
    protected void OnaAttack(attackState state)
    {
        activeAttack = state;
    }

    //==============================================================================================================
    //                  �����蔻����擾���邽�߂ɔ�΂�Ray
    //==============================================================================================================
    void RayShot()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Vector3 raycenter = transform.position + new Vector3(0f, obj_hitbox.size.y / 2, 0);

        isGround = Physics.Raycast(ray, out underRay, underRayDistance);
        Debug.Log($"����{Vector3.Distance(transform.position, underRay.point)}");

        var ss = ColliderUtility.GetBoxColliderVertices(obj_hitbox);
        foreach (Vector3 vec in ss)
        {
            if (Physics.Raycast(new Ray(vec, rb.Velocity.normalized), out sideRay, sideRayDistance))
            {
                isCollisionWall = true;
                break;
            }
            else
            {
                isCollisionWall = false;
            }
        }
    }

    //==============================================================================================================
    //                  ���[�Ȓl�ƂȂ������W��␳����
    //
    // �ڒn�����ǂւ̌��˔���̒l�Œ[���ƂȂ����l���C������
    //==============================================================================================================
    Vector3 PositionhAdjust(Vector3 pos)
    {
        if (isGround)
        {
            if (Vector3.Distance(transform.position, underRay.point) > 0.1f)
            {
                pos.y -= Vector3.Distance(transform.position, underRay.point) - 0.1f;
            }
        }
        //if(isCollisionWall)
        //{
        //    if (Vector3.Distance(transform.position, sideRay.point) > 0.1f)
        //    {
        //        pos.y -= Vector3.Distance(transform.position, sideRay.point) - 0.1f;
        //    }
        //}

        return pos;
    }

    //==============================================================================================================
    //                 �e�A�N�V�����̌��݂̃N�[���^�C�����擾����
    //==============================================================================================================
    protected float GetMoveStateDuration(moveState state)
    {
        //�w�肳�ꂽ�X�e�[�g�����݂���Ȃ�
        if (MoveActionCoolTime.ContainsKey(state))
        {
            return MoveActionCoolTime[state];           //�X�e�[�g����float�l���擾����
        }

        return -1.0f; // �f�t�H���g�l�Ƃ��� -1.0f ��Ԃ�
    }

    protected void StartCooldown(moveState state, float duration)
    {
        if (MoveActionCoolTime[state] > float.MinValue)
        {
            Debug.Log($"{state} �̃N�[���_�E���͊��ɐi�s���ł�");
            return;
        }

        StartCoroutine(CooldownCoroutine(state, duration));
    }

    protected IEnumerator CooldownCoroutine(moveState state, float duration)
    {
        MoveActionCoolTime[state] = duration;

        while (MoveActionCoolTime[state] > float.MinValue)
        {
            MoveActionCoolTime[state] -= Time.deltaTime;
            yield return null; // 1�t���[���ҋ@
        }

        MoveActionCoolTime[state] = 0;
        Debug.Log($"{MoveActionCoolTime.ContainsKey(state)} cooldown���� ");
    }

    protected void SetMoveStateDuration(moveState state, float duration)
    {
        //�w�肳�ꂽ�X�e�[�g�����݂���Ȃ�
        if (MoveActionCoolTime.ContainsKey(state))
        {
            MoveActionCoolTime[state] = duration < 0 ? 0 : duration;    //�������̒l��0�ȉ��ɂȂ�Ȃ��悤�ɂ���
            if (MoveActionCoolTime[state] == 0) Debug.Log($"{state} cooldown����");
        }
    }

    float HorizontalStrength(Vector3 v)
    {
        float x = v.x;
        float z = v.z;
        return Mathf.Sqrt(x * x + z * z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var ss = ColliderUtility.GetBoxColliderVertices(obj_hitbox);
        foreach (Vector3 vec in ss)
        {
            Gizmos.DrawLine(vec, rb.Velocity.normalized * 10f);
        }
    }
    //a
}
