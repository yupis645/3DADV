using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    public List<Transform> transList = new List<Transform>();   //�J�����͈͓��ɂ����߂����I�u�W�F�N�g�̃��X�g
    private Transform cameraPos;
    public Camera usingCamera;
    private Vector3 pos = new Vector3();
    private Vector3 center = new Vector3();
    private float radius;
    private float margin = 1.0f;        //���a�������]���ɂƂ邽�߂̒l
    private float distance;
    public float cameraHeight = 5f;  //�J�������n�ʂɂ߂荞�܂Ȃ��悤�ɃJ�����𕂂����鍂��


    void Start()
    {
        cameraPos = GameObject.Find("CameraPosition").GetComponent<Transform>();
    }

    void Update()
    {
        pos = new Vector3(0, 0, 0);
        radius = 0.0f;
        foreach (Transform trans in transList)
        {     //�I�u�W�F�N�g�̃|�W�V�����̕��ϒl���Z�o
            pos += trans.position;
        }
        center = pos / transList.Count;
        this.transform.position = center;           //CenterPoint�̃|�W�V�����𒆐S�ɔz�u
        foreach (Transform trans in transList)
        {     //���S����ł������I�u�W�F�N�g�Ƃ̋������Z�o
            radius = Mathf.Max(radius, Vector3.Distance(center, trans.position));
        }
        distance = (radius + margin) / Mathf.Sin(usingCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);   //�J�����̋������Z�o
        cameraPos.localPosition = new Vector3(0, cameraHeight, -distance);    //CameraPosition���J�����̋��������Ƃɔz�u
        cameraPos.LookAt(this.transform);           //CameraPosition�𒆐S�̕����Ɍ�������
    }
}
