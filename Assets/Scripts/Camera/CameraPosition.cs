using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
       
        //iTween.MoveUpdate(this.gameObject, iTween.Hash(
        //    "position", Target.position,
        //    "time", 3.0f)
        //);
        //iTween.RotateUpdate(this.gameObject, iTween.Hash(
        //    "rotation", Target.rotation.eulerAngles,
        //    "time", 3.0f)
        //);
    }
    private void FixedUpdate()
    {

        transform.position = Vector3.Lerp(transform.position, Target.position, 3.0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, 3.0f);

    }
}
