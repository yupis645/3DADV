using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestControll : TestUnitController
{

    //====================================================
    //      ������
    //
    //
    //====================================================
    private void Start()
    {
       
    }

    //====================================================
    //              �X�V
    //
    //
    //====================================================
    private void Update()
    {
        InputPlayerMove();      //�v���C���[�̓���

        CharacterUpdate();      //�L�����N�^�[�̃A�b�v�f�[�g
    }

    private void FixedUpdate()
    {
        PositionFixedUpdate();
    }

    //====================================================
    //              �v���C���[�̓��͂ɑ΂��鏈��
    //
    //
    //====================================================
    void InputPlayerMove()
    {
        //�����L�[�̓���
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
            AcitveMoveStateTrigger(moveState.dash);     //Z�F�_�b�V��
        }
        if (Input.GetKeyDown(KeyCode.U))            
        {
            OnaAttack(attackState.leftarms);            //U : ���r����
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnaAttack(attackState.rightarms);             //i : �E�r����
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnaAttack(attackState.backleftarms);            //O: �o�b�N�p�b�N�� 
            AcitveMoveStateTrigger(moveState.attack);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnaAttack(attackState.backrightarms);           //P�F�o�b�N�p�b�N�E
            AcitveMoveStateTrigger(moveState.attack);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AcitveMoveStateTrigger(moveState.jump);         //space:�W�����v
        }


    }

    //====================================================
    //              �����蔻��̉���
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
