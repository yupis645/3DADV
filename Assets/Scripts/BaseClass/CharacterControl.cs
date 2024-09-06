using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.Archive;
using Unity.VisualScripting;
using UnityEngine;


public class CharacterControl : MonoBehaviour
{
    [SerializeField]  AssembledUnit unit;

    [SerializeField] MyRigidBody rb = new MyRigidBody();


    [SerializeField] protected float underRayDistance = 0;

    protected delegate void activeMoveEvent(moveState newState,bool onstate);
    protected event activeMoveEvent OnActiveStateChanged;

    protected RaycastHit underRay;

    protected BoxCollider obj_hitbox;

    [Flags]
    protected enum moveState
    {
        none = 0,
        move = 1,
        dash = 2,
        jump = 4, 
        attack = 8,
        knockback = 16,
        down = 32,
    }
    protected moveState activeMove;

    protected enum attackState
    {
        none,
        rightarms,
        leftarms,
        backrightarms,
        backleftarms,
        specialattack,
    }
    protected attackState activeAttack;


    protected Vector3 inputMoveVec = Vector3.zero;
    [SerializeField]protected Vector3 currentMoveVec = Vector3.zero;

    protected float actionTime= 0;
    protected float dashRange = 0;

    [SerializeField]protected float currentAirSpeed = 0;
    [SerializeField] protected int jumpCount = 0;

    [SerializeField] protected bool isGround = false;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        OnActiveStateChanged += ActiveStateChangeEvent;
        rb.OnMass_Drag_Gravity(true, true, true);
    }


    //==============================================================================================================
    //            �X�e�[�g�ύX���\�b�h
    //
    // �����Ŏw�肳�ꂽ�X�e�[�g�ɕύX���A�X�e�[�g�ύX�C�x���g�������ɔ��΂�����
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void AcitveStateUpdate(moveState state, bool onstate)
    {
        if(onstate) activeMove |= state;                        //�X�e�[�g�������Ŏw�肳�ꂽ�X�e�[�g�ɕύX
        else        activeMove &= ~state;

        OnActiveStateChanged?.Invoke(state, onstate);        //�C�x���g�̔���
    }

    void ActiveStateChangeEvent(moveState state, bool onstate)
    {
        switch (state)
        {
            case moveState.none:
                activeMove = moveState.none;      //�S�Ẵt���O��0�ŏ���������
                break;

            case moveState.move:     
                if(activeMove.HasFlag(moveState.dash))
                {
                    activeMove &= ~moveState.move;        //�_�b�V������move��1�ɂ����Ȃ�
                }
                break;

            case moveState.dash:
                if (onstate)                //�_�b�V����"1"�ɕύX���ꂽ�u��
                {
                    activeMove &= ~moveState.move;                  //�_�b�V���J�n���ɂ͈ړ��t���O��"0"�ɂ���
                    activeMove &= ~moveState.jump;                  //�_�b�V���J�n���ɂ̓W�����v�t���O��"0"�ɂ���

                    if (isGround) actionTime = unit.status.DashRange;     //�n��ɂ���ꍇ�͒n��_�b�V��������dashRange�ɋL��
                    else actionTime = unit.status.AirRange;               //�󒆂ɂ���ꍇ�͋󒆃_�b�V��������dashRange�ɋL��

                    if (inputMoveVec.x != 0 || inputMoveVec.y != 0) //�ړ���������͂���Ă���ꍇ��
                    {
                        currentMoveVec = inputMoveVec;                  //���͕����Ƀx�N�g�������킹��
                    }                                                   //���͂��Ȃ��ꍇ�͌��݂̈ړ��x�N�g�������Ƀ_�b�V������
                    currentMoveVec.y = 0;                           //���̎�Y�����ւ̈ړ��ʂ�"0"�ɐݒ肷��
                    currentAirSpeed = 0;                            //Y���̈ړ����x��"0"�ɂ���
                    rb.OnMass_Drag_Gravity(true, false, false);     //MyRightbody�̗L����

                }
                else                         //�_�b�V����"0"�ɕύX���ꂽ�u��
                {
                    dashRange = 0;
                    rb.OnMass_Drag_Gravity(true, true, true);
                }
                actionTime = 0;
                break;

            case moveState.jump:
                if (activeMove.HasFlag(moveState.dash))
                {
                    activeMove &= ~moveState.jump;        //�_�b�V������move��0�̂܂�
                    break;
                }
                if (jumpCount < unit.status.MaxJumpCount && onstate)
                {
                    currentAirSpeed = unit.status.JumpPower;
                    jumpCount++;
                }
                break;

            case moveState.attack:
                activeMove &= ~moveState.jump; 
                activeMove &= ~moveState.move; 
                activeMove &= ~moveState.dash;
                if (onstate)
                {
                    actionTime += unit.weapons[(int)activeAttack].CoolDownTime;
                    rb.OnMass_Drag_Gravity(true, false, false);
                }
                else
                {
                    actionTime = 0;
                    rb.OnMass_Drag_Gravity(true,true,true);
                }
                break;
            case moveState.knockback:
               
                break;
            case moveState.down:
               
                break;

            // ����ȊO�̏ꍇ�̏���
            default:
                break;
        }
    }

    protected void CharacterUpdate()
    {
        //Ray�����
        RayShot(transform.position);

        if(activeMove.HasFlag(moveState.move))
        {
            currentMoveVec = move(currentMoveVec,inputMoveVec);
        }

        if (activeMove.HasFlag(moveState.dash))
        {
            currentMoveVec = Dash(currentMoveVec);
        }

        if (activeMove.HasFlag(moveState.jump))
        {
            if (currentAirSpeed < -0) AcitveStateUpdate(moveState.jump, false);
        }

        if (activeMove.HasFlag(moveState.jump))
        {
            if (!isGround) currentAirSpeed -= 0.01f;
        }

        if (isGround && !activeMove.HasFlag(moveState.jump))
        {
            jumpCount = 0;
        }

        if (rb.onMass)

        if (rb.onDrag) currentMoveVec = Drag(currentMoveVec, inputMoveVec);

        if (rb.onGravity) currentAirSpeed = Gravity(currentAirSpeed);


        transform.position += (currentMoveVec) + (Vector3.up * currentAirSpeed);

    }

    Vector3 move(Vector3 movevec,Vector3 dirVec)
    {
        Vector3 nextvec = movevec;

        if (dirVec.x != 0) nextvec.x += unit.status.MoveSpeed * Mathf.Sign(dirVec.x) * Time.deltaTime;
        if (nextvec.x > unit.status.MoveSpeedLimit) nextvec.x = movevec.x;

        if (dirVec.z != 0) nextvec.z += unit.status.MoveSpeed * Mathf.Sign(dirVec.z) * Time.deltaTime;
        if (nextvec.z > unit.status.MoveSpeedLimit) nextvec.z = movevec.z;

        return nextvec;
    }


    Vector3 Dash(Vector3 v)
    {
        actionTime += Time.deltaTime;
        if (actionTime >= dashRange) AcitveStateUpdate(moveState.dash, false);

        return v.normalized * unit.status.DashSpeed;
    }


    Vector3 Drag(Vector3 currentVec,Vector3 dirVec)
    {
        Vector3 v = currentVec;



        //if (currentVec.x != 0) v.x += rb.OnDrag() * -currentVec.x;
        //if (Mathf.Abs(currentVec.x) < (unit.status.MoveSpeed * Time.deltaTime) && dirVec.x == 0) v.x = 0;

        //if (currentVec.z != 0) v.z += rb.OnDrag() * -currentVec.z;
        //if (Mathf.Abs(currentVec.z) < (unit.status.MoveSpeed * Time.deltaTime) && dirVec.z == 0) v.z = 0;

        return v;
    }

    float Gravity(float y_axis)
    {
        if(isGround && !activeMove.HasFlag(moveState.jump)) return 0; 
        else         return y_axis - rb.OnGravity(); ;
    }

    void Attack()
    {
        
    }
    void Knockback()
    {

    }

    void Down()
    {

    }

    protected void OnaAttack(attackState state)
    {
        activeAttack = state;
    }

    bool RayShot(Vector3 currentPos)
    {
        Ray ss = new Ray(currentPos, Vector3.down);

        Vector3 rayPosition = currentPos - new Vector3(0.0f, 0.0f, 0.0f);

        Ray ray = new Ray(rayPosition, Vector3.down);
        isGround = Physics.Raycast(ray, out underRay,underRayDistance);
        Debug.DrawRay(rayPosition, Vector3.down * underRayDistance, Color.blue);

        return isGround;
    }

    float HorizontalStrength(Vector3 v)
    {
        float x = v.x;
        float z = v.z;
        return Mathf.Sqrt(x * x + z * z);
    }

}
