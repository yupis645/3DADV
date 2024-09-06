
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : CharacterControl
{


    private void Start()
    {
        obj_hitbox = GetComponent<BoxCollider>();
        activeMove = (moveState)0x00;
    }
    private void Update()
    {
        InputPlayerMove();

        CharacterUpdate();
    }

    

    void InputPlayerMove()
    {
        inputMoveVec.x = Input.GetAxisRaw("Horizontal");
        inputMoveVec.y = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        inputMoveVec.z = Input.GetAxisRaw("Vertical"); ;


        if (Input.GetKeyDown(KeyCode.Z))
        {
            AcitveStateUpdate(moveState.dash, true);
        }
         if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            OnaAttack(attackState.leftarms);
            AcitveStateUpdate(moveState.attack, true);
        }
         if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            OnaAttack(attackState.rightarms);
            AcitveStateUpdate(moveState.attack, true);
        }
         if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            OnaAttack(attackState.backleftarms);
            AcitveStateUpdate(moveState.attack, true);
        }
         if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            OnaAttack(attackState.backrightarms);
            AcitveStateUpdate(moveState.attack, true);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AcitveStateUpdate(moveState.jump, true);
        }

        if(inputMoveVec.x != 0 || inputMoveVec.z != 0 )
        {
            AcitveStateUpdate(moveState.move, true);
        }
        else
        {
            AcitveStateUpdate(moveState.move, false);
        }

    }


        private void OnDrawGizmos()
    {
        Vector3 t = new Vector3(0, underRayDistance, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - t);
    }
}
