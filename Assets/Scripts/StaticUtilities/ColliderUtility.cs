using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderUtility
{

    //=======================================================================
    //                  Box�R���C�_�[�̒��_��Ԃ����\�b�h
    //=======================================================================
    public static Vector3[] GetBoxColliderVertices(BoxCollider Col)
    {
        Transform trs = Col.transform;
        Vector3 sc = trs.lossyScale;

        sc.x *= Col.size.x;
        sc.y *= Col.size.y;
        sc.z *= Col.size.z;

        sc *= 0.5f;

        Vector3 cp = trs.TransformPoint(Col.center);

        Vector3 vx = trs.right * sc.x;
        Vector3 vy = trs.up * sc.y;
        Vector3 vz = trs.forward * sc.z;

        Vector3 p1 = -vx + vy + vz;
        Vector3 p2 = vx + vy + vz;
        Vector3 p3 = vx + -vy + vz;
        Vector3 p4 = -vx + -vy + vz;

        Vector3[] vertices = new Vector3[8];

        vertices[0] = cp + p1;
        vertices[1] = cp + p2;
        vertices[2] = cp + p3;
        vertices[3] = cp + p4;

        vertices[4] = cp - p1;
        vertices[5] = cp - p2;
        vertices[6] = cp - p3;
        vertices[7] = cp - p4;

        return vertices;
    }
}
