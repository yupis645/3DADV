using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    public List<Transform> transList = new List<Transform>();   //カメラ範囲内におさめたいオブジェクトのリスト
    private Transform cameraPos;
    public Camera usingCamera;
    private Vector3 pos = new Vector3();
    private Vector3 center = new Vector3();
    private float radius;
    private float margin = 1.0f;        //半径を少し余分にとるための値
    private float distance;
    public float cameraHeight = 5f;  //カメラが地面にめり込まないようにカメラを浮かせる高さ


    void Start()
    {
        cameraPos = GameObject.Find("CameraPosition").GetComponent<Transform>();
    }

    void Update()
    {
        pos = new Vector3(0, 0, 0);
        radius = 0.0f;
        foreach (Transform trans in transList)
        {     //オブジェクトのポジションの平均値を算出
            pos += trans.position;
        }
        center = pos / transList.Count;
        this.transform.position = center;           //CenterPointのポジションを中心に配置
        foreach (Transform trans in transList)
        {     //中心から最も遠いオブジェクトとの距離を算出
            radius = Mathf.Max(radius, Vector3.Distance(center, trans.position));
        }
        distance = (radius + margin) / Mathf.Sin(usingCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);   //カメラの距離を算出
        cameraPos.localPosition = new Vector3(0, cameraHeight, -distance);    //CameraPositionをカメラの距離をもとに配置
        cameraPos.LookAt(this.transform);           //CameraPositionを中心の方向に向かせる
    }
}
