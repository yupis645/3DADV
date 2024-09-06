using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestControll : TestUnitController
{

    //====================================================
    //      初期化
    //
    //
    //====================================================
    private void Start()
    {
       
    }

    //====================================================
    //              更新
    //
    //
    //====================================================
    private void Update()
    {
        InputPlayerMove();      //プレイヤーの入力

        CharacterUpdate();      //キャラクターのアップデート
    }

    private void FixedUpdate()
    {
        PositionFixedUpdate();
    }

    //====================================================
    //              プレイヤーの入力に対する処理
    //
    //
    //====================================================
    void InputPlayerMove()
    {
        //方向キーの入力
        Vector3 movement = Vector3.zero;
        movement.x = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveHorizontal();
        movement.z = InstanceManager.Instance.GetInputManagerInstance<IInputManager>().PlayerMoveVertical();

        if (movement.x != 0 || movement.z != 0)
        {
            AcitveMoveStateTrigger(moveState.move);
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("sw");
            Explosion.CreateExplosion(transform.position, 1,0.1f, 5f);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AcitveMoveStateTrigger(moveState.dash);     //Z：ダッシュ
        }
        if (Input.GetKeyDown(KeyCode.U))            
        {
            OnaAttack(attackState.leftarms);            //U : 左腕武器
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnaAttack(attackState.rightarms);             //i : 右腕武器
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnaAttack(attackState.backleftarms);            //O: バックパック左 
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnaAttack(attackState.backrightarms);           //P：バックパック右
            AcitveMoveStateTrigger(moveState.attack);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AcitveMoveStateTrigger(moveState.jump);         //space:ジャンプ
        }


    }

    //====================================================
    //              当たり判定の可視化
    //
    //
    //====================================================
    private void OnDrawGizmos()
    {
        //Vector3 t = new Vector3(0, underRayDistance, 0);
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position - t);

    }
}
