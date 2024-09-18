using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public enum viewmode
    {
        Top,
        Quarter,
        Side,
        PlayerBack,
    }
    public viewmode cameratype;

    public enum viewdirection
    {
        Negative_X,
        Negative_Z,
        Positive_X,
        Positive_Z
    }
    public viewdirection cameradir;

    [Range(0.0f, 1.0f)]
    public float X_range;
    
    [Range(0.0f, 1.0f)]
    public float Z_range;
    public float radiusdistance = 0;
    public float higicamera = 0;

    public Vector3 dirCorrection = Vector3.zero;

    public Transform[] unit = new Transform[4];
    public Transform playerunit;
    public Transform stage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraPos();
    }

    void CameraPos()
    {
        Vector3 target = Vector3.zero;

        Vector3 pos = Vector3.zero;

        float x_rangeCorr = 0;
        float z_rangeCorr = 0;

        switch (cameratype)
        {
            case viewmode.Top:
                pos = stage.position;
                x_rangeCorr = X_range;
                z_rangeCorr = Z_range;
                pos += new Vector3(X_range, stage.localScale.y + higicamera, Z_range);

                target= new Vector3(pos.x,0,pos.z);
                break;

            case viewmode.Quarter:
                pos = (unit[0].transform.position + unit[1].transform.position) * 0.5f;
                pos.y += stage.localScale.y + higicamera;
                x_rangeCorr = X_range *  - radiusdistance;
                z_rangeCorr = Z_range * - radiusdistance;

                if (cameradir == viewdirection.Positive_X) pos += new Vector3(stage.localScale.x - x_rangeCorr, 0, stage.localScale.x - z_rangeCorr);
                else if (cameradir == viewdirection.Negative_X) pos += new Vector3(-stage.localScale.x + x_rangeCorr, 0, stage.localScale.x - z_rangeCorr);
                else if (cameradir == viewdirection.Positive_Z) pos += new Vector3(-stage.localScale.x + x_rangeCorr, 0, -stage.localScale.x + z_rangeCorr);
                else if (cameradir == viewdirection.Negative_Z) pos += new Vector3(stage.localScale.x - x_rangeCorr, 0, -stage.localScale.x + z_rangeCorr);


                target = pos;
                break;

            case viewmode.Side:
                pos = stage.position;
                pos.y += stage.localScale.y + higicamera;
                x_rangeCorr = X_range * stage.localScale.x;
                z_rangeCorr = Z_range * stage.localScale.z;
                if (cameradir == viewdirection.Positive_X) pos.x += stage.localScale.x - x_rangeCorr;
                else if (cameradir == viewdirection.Negative_X) pos.x -= stage.localScale.x - x_rangeCorr;
                else if (cameradir == viewdirection.Positive_Z) pos.z += stage.localScale.z * z_rangeCorr;
                else if (cameradir == viewdirection.Negative_Z) pos.z -= stage.localScale.z * z_rangeCorr;

                target = stage.position;
                break;

            case viewmode.PlayerBack:
                pos = playerunit.position;
                pos.y += higicamera;

                x_rangeCorr = X_range * 360;
                z_rangeCorr = Z_range * 360;

                // 角度の度（degree）をラジアンにする
                float radian = Mathf.Deg2Rad * (x_rangeCorr - z_rangeCorr);

                // 回転中の座標
                pos += new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * radiusdistance;
                target = playerunit.position;
                break;
        }
        
        var aim = (target - pos) + dirCorrection;      //ワールド座標での注目対象との距離 + 距離の補正値(Vector3)
        var look = Quaternion.LookRotation(aim);                //注目対象の方向の角度のQuatanionを取得

        this.transform.localRotation = look;                    //向きを代入
        this.transform.position = pos;                          //座標の代入

    }

}
