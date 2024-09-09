using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.Archive;
using Unity.VisualScripting;
using UnityEngine;

public class TestUnitController : MonoBehaviour
{
    [SerializeField] AssembledUnit unit;                    //操作するユニットの情報

    [SerializeField] MyRigidBody rb = new MyRigidBody();    //自作の物理エンジン

    protected BoxCollider obj_hitbox;                       //当たり判定の

    protected RaycastHit underRay;                          //接地判定をとるためのRay
    [SerializeField] protected float underRayDistance = 0;  //underRayの飛ばす距離
    [SerializeField] protected bool isGround = false;                    //接地状態のフラグ

    protected RaycastHit sideRay;                          //接地判定をとるためのRay
    [SerializeField] protected float sideRayDistance = 0;  //underRayの飛ばす距離
    [SerializeField] protected bool isCollisionWall = false;                    //接地状態のフラグ

    private Vector3 moveStartPos = Vector3.zero;

    //動的に変更されるイベントステータス用のイベント
    protected delegate void activeMoveEvent(moveState newState);  
    protected event activeMoveEvent ActiveMoveState;
    protected event activeMoveEvent DeactivatedMoveState;
  
    [Flags]
    protected enum moveState                         //動作アクションのフラグ
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

    protected enum attackState                      //攻撃アクションの種類
    {
        none,
        rightarms,
        leftarms,
        backrightarms,
        backleftarms,
        specialattack,
    }
    [SerializeField] protected attackState activeAttack;

    public LayerMask GroundMask;        //接地判定をとれるレイヤー

    protected Dictionary<moveState, float> MoveActionCoolTime;          //次の行動に移れるまでのクールタイム
    [SerializeField]protected float actionTime = 0;                      //ワンアクションの全体時間
    [SerializeField] protected int jumpCount = 0;                        //ジャンプ回数のカウント


    private static readonly Color BoundsColor = new Color(1, 0, 0, 0.5f);



    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //==============================================================================================================
    //            初期化
    //
    //==============================================================================================================
    private void Awake()
    {
        unit.initialize();                                      //ユニット情報の初期化
        ActiveMoveState += ActiveMoveStateEvent;                //アクションフラグのONトリガーイベントの追加
        DeactivatedMoveState += DeactivatedMoveStateEvent;      //アクションフラグのOFFトリガーイベントの追加
        rb.SetUp(transform.position,                             // 初期の位置を MyRigidbody に設定
                true,       //質量の適応
                true,       //抵抗の適応
                true,       //重力の適応
                new Vector3(unit.status.MoveSpeedLimit, float.MaxValue, unit.status.MoveSpeedLimit),                    //水平、垂直方向への最大移動速度
                new Vector3(unit.status.MoveSpeed, unit.status.JumpPower, unit.status.MoveSpeed) * Time.deltaTime);     //水平、垂直方向への最小移動速度

        obj_hitbox = GetComponent<BoxCollider>();       //当たり判定の取得
        activeMove = (moveState)0x00;                   //移動アクションのフラグを初期化
        activeAttack = 0;

        //各アクションのクールタイムを初期化
        MoveActionCoolTime = new Dictionary<moveState, float>   //各アクションの名前とfloat値をdictionaryにセット
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
    //            移動ステートONメソッド
    //
    // 引数で指定されたステートに変更し、ステート変更イベントも同時に発火させる
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void AcitveMoveStateTrigger(moveState state)
    {
        if (MoveActionCoolTime.ContainsKey(state))
        {
            if (MoveActionCoolTime[state] > 0)     //クールタイムが'0'(初期値,フラグOFF)になっているなら
            {
                Debug.Log($"{state} クールダウン中 ");
                return;
            }
        }
        ActiveMoveState?.Invoke(state);     //ONトリガーイベントを発火させる
     
    }

    //==============================================================================================================
    //            ステート変更イベント
    //
    // アクションステート変更後、一度だけ通るメソッド
    //==============================================================================================================
    void ActiveMoveStateEvent(moveState state)
    {
        switch (state)
        {
            case moveState.frozon:
                activeMove = 0;      //全てのフラグを0で初期化する
                break;

            case moveState.move:
                //ダッシュ時はmoveを1にさせない
                if (activeMove.HasFlag(moveState.dash) || activeMove.HasFlag(moveState.attack)) return;
                break;

            case moveState.dash:
                activeMove &= ~(moveState.move | moveState.dash | moveState.jump);

                //ダッシュ時間を設定する
                if (isGround) actionTime = unit.status.DashRange;     //地上にいる場合
                else actionTime = unit.status.AirRange;               //空中にいる場合
                rb.Velocity = Vector3.zero;
                Vector3 dashVec = new Vector3(
                    InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveHorizontal(),   
                    0,
                    InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveVertical()
                    ) ;
                rb.AddForce(dashVec * unit.status.DashSpeed, ForceMode.Impulse);
                rb.OnMass_Drag_Gravity(true, false, false);     //MyRightbodyの有効化


                break;

            case moveState.jump:
                if (jumpCount >= unit.status.MaxJumpCount               //ジャンプカウントが最大ジャンプ回数を超えている場合、
                    || activeMove.HasFlag(moveState.dash)) return;     //もしくはダッシュフラグが"1"の場合、ジャンプフラグは立てない(ジャンプできない)
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

            // それ以外の場合の処理
            default:
                break;
        }

        activeMove |= state;    //指定されたステートを"1"にする
    }

    //==============================================================================================================
    //            移動ステートOFFメソッド
    //
    // 引数で指定されたステートに変更し、ステート変更イベントも同時に発火させる
    //InstanceManager.Instance.GetSoundManagerInstance<ISoundManager>().SE_Shot(SoundManager.SEMenu.gameover);   
    //==============================================================================================================
    protected void DeactivatedMoveStateTrigger(moveState state)
    {
        DeactivatedMoveState?.Invoke(state);
    }

    //==============================================================================================================
    //            移動ステートOFFイベント
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
                FlagsBitUtility.RemoveFlagsBit(ref activeMove, moveState.move, moveState.dash, moveState.jump);      //移動、ダッシュ、ジャンプを無効にする
                actionTime = 0;                                                 //アクションタイムを'0'で初期化する
                rb.OnMass_Drag_Gravity(true, true, true);                       //MyRigitbodyの全てをONにする
                float cooltime = unit.weapons[(int)activeAttack - 1].CoolDownTime;    //使用武器のクールタイムの値を取得する
                Debug.Log($"{unit.weapons[(int)activeAttack - 1].WeaponName}.FriezeTime = {cooltime}");
                StartCooldown(moveState.attack, cooltime);

                OnaAttack(attackState.none);                                    //アタックステートをnoneにする
                break;
            case moveState.knockback:

                break;
            case moveState.down:

                break;


            // それ以外の場合の処理
            default:
                break;
        }

         activeMove &= ~state;      //指定されたステートを"0"にする
    }

    //==============================================================================================================
    //            キャラクターの更新処理
    //
    //  
    //==============================================================================================================
    protected void CharacterUpdate()
    {
        //Rayを放つ
         RayShot();

        if (activeMove.HasFlag(moveState.move))
        {
            //// 移動方向の計算
            Vector3 movement = Vector3.zero;
            movement.x = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveHorizontal();
            movement.z = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveVertical();

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            movement = cameraForward * movement.z + Camera.main.transform.right * movement.x;       //


            move(movement);
        }
    }

    protected void PositionFixedUpdate()
    {

        if (activeMove.HasFlag(moveState.dash)) if(Dash()) DeactivatedMoveStateTrigger(moveState.dash);    //ダッシュステートをOFFにする;

        if (activeMove.HasFlag(moveState.jump)) if (rb.Velocity.y < -0) DeactivatedMoveStateTrigger(moveState.jump);

        if (activeMove.HasFlag(moveState.attack)) if(Attack()) DeactivatedMoveStateTrigger(moveState.attack);      //アタックステートをOFFにする;


        // 重力の適用
        if (rb.onGravity) rb.ApplyGravity(-rb.OnGravity());

        //接地 & ジャンプらフラグがfalse 
        if (isGround && !activeMove.HasFlag(moveState.jump))
        {
            jumpCount = 0;
            rb.Velocity.y = 0;
        }
        // ドラッグの適用
        if(rb.onDrag)rb.UpdateDrag(Time.fixedDeltaTime,rb.OnDrag());

        // 位置の更新
        rb.Position = PositionhAdjust(transform.position);
        rb.UpdatePosition(Time.fixedDeltaTime);
        transform.position = rb.Position;           // 位置を transform に反映

        // 回転の更新（必要に応じて）
        rb.UpdateRotation(Time.fixedDeltaTime);
        transform.rotation = rb.Rotation;
    }


    //==============================================================================================================
    //                  移動
    //
    //  移動ベクトルが０以外の場合、加速させる。またその時の速度が上限を超えているなら加速させる前の値に戻す
    //==============================================================================================================
    void move(Vector3 movement)
    {
        // 入力に基づいて力を適用
        if (movement.magnitude > 0)
        {
            // 力の大きさを調整（例えば10倍）
            rb.AddForce(movement * unit.status.MoveSpeed, ForceMode.Force);
        }

    }

    //==============================================================================================================
    //                  ダッシュ
    //
    //  アクションタイムが０になるまでは一定の速度を返し続ける
    //==============================================================================================================
    bool Dash()
    {
        return TimerUtility.TimerCountDown(ref actionTime, Time.deltaTime);     //タイマーをカウントダウンする
    }

    //==============================================================================================================
    //                  アタック
    //
    //  アクションタイムが０になるまでは一定の落下速度を返し続ける
    //==============================================================================================================
    bool Attack()
    {
       return TimerUtility.TimerCountDown(ref actionTime, Time.deltaTime);

    }

    //==============================================================================================================
    //                  ノックバック
    //==============================================================================================================
    void Knockback()
    {

    }

    //==============================================================================================================
    //                  ダウン
    //==============================================================================================================
    void Down()
    {

    }


    //==============================================================================================================
    //                 指定した攻撃ステートに変更
    //==============================================================================================================
    protected void OnaAttack(attackState state)
    {
        activeAttack = state;
    }

    //==============================================================================================================
    //                  当たり判定を取得するために飛ばすRay
    //==============================================================================================================
    void RayShot()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Vector3 raycenter = transform.position + new Vector3(0f, obj_hitbox.size.y / 2, 0);

        isGround = Physics.Raycast(ray, out underRay, underRayDistance);
        Debug.Log($"距離{Vector3.Distance(transform.position, underRay.point)}");

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
    //                  半端な値となった座標を補正する
    //
    // 接地判定や壁への激突判定の値で端数となった値を修正する
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
    //                 各アクションの現在のクールタイムを取得する
    //==============================================================================================================
    protected float GetMoveStateDuration(moveState state)
    {
        //指定されたステートが存在するなら
        if (MoveActionCoolTime.ContainsKey(state))
        {
            return MoveActionCoolTime[state];           //ステート名のfloat値を取得する
        }

        return -1.0f; // デフォルト値として -1.0f を返す
    }

    protected void StartCooldown(moveState state, float duration)
    {
        if (MoveActionCoolTime[state] > float.MinValue)
        {
            Debug.Log($"{state} のクールダウンは既に進行中です");
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
            yield return null; // 1フレーム待機
        }

        MoveActionCoolTime[state] = 0;
        Debug.Log($"{MoveActionCoolTime.ContainsKey(state)} cooldown完了 ");
    }

    protected void SetMoveStateDuration(moveState state, float duration)
    {
        //指定されたステートが存在するなら
        if (MoveActionCoolTime.ContainsKey(state))
        {
            MoveActionCoolTime[state] = duration < 0 ? 0 : duration;    //第二引数の値が0以下にならないようにする
            if (MoveActionCoolTime[state] == 0) Debug.Log($"{state} cooldown完了");
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
