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
    //            ステート変更メソッド
    //
    // 引数で指定されたステートに変更し、ステート変更イベントも同時に発火させる
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void AcitveStateUpdate(moveState state, bool onstate)
    {
        if(onstate) activeMove |= state;                        //ステートを引数で指定されたステートに変更
        else        activeMove &= ~state;

        OnActiveStateChanged?.Invoke(state, onstate);        //イベントの発火
    }

    void ActiveStateChangeEvent(moveState state, bool onstate)
    {
        switch (state)
        {
            case moveState.none:
                activeMove = moveState.none;      //全てのフラグを0で初期化する
                break;

            case moveState.move:     
                if(activeMove.HasFlag(moveState.dash))
                {
                    activeMove &= ~moveState.move;        //ダッシュ時はmoveを1にさせない
                }
                break;

            case moveState.dash:
                if (onstate)                //ダッシュが"1"に変更された瞬間
                {
                    activeMove &= ~moveState.move;                  //ダッシュ開始時には移動フラグは"0"にする
                    activeMove &= ~moveState.jump;                  //ダッシュ開始時にはジャンプフラグは"0"にする

                    if (isGround) actionTime = unit.status.DashRange;     //地上にいる場合は地上ダッシュ距離をdashRangeに記憶
                    else actionTime = unit.status.AirRange;               //空中にいる場合は空中ダッシュ距離をdashRangeに記憶

                    if (inputMoveVec.x != 0 || inputMoveVec.y != 0) //移動方向を入力されている場合は
                    {
                        currentMoveVec = inputMoveVec;                  //入力方向にベクトルをあわせる
                    }                                                   //入力がない場合は現在の移動ベクトル方向にダッシュする
                    currentMoveVec.y = 0;                           //この時Y方向への移動量を"0"に設定する
                    currentAirSpeed = 0;                            //Y軸の移動速度を"0"にする
                    rb.OnMass_Drag_Gravity(true, false, false);     //MyRightbodyの有効化

                }
                else                         //ダッシュが"0"に変更された瞬間
                {
                    dashRange = 0;
                    rb.OnMass_Drag_Gravity(true, true, true);
                }
                actionTime = 0;
                break;

            case moveState.jump:
                if (activeMove.HasFlag(moveState.dash))
                {
                    activeMove &= ~moveState.jump;        //ダッシュ時はmoveは0のまま
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

            // それ以外の場合の処理
            default:
                break;
        }
    }

    protected void CharacterUpdate()
    {
        //Rayを放つ
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
