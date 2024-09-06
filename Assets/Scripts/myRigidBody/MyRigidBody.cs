using System;
using UnityEngine;

[Serializable]
class MyRigidBody
{

    [SerializeField] private float Mass = 0;                 //オブジェクトの質量。
    [SerializeField] private float Drag = 0;              //オブジェクトにかかる空気抵抗の値。
    [SerializeField] private float Gravity = 0;

    [HideInInspector]
    public Vector3 Position;           //オブジェクトの現在の位置。
    public Vector3 Velocity;           //オブジェクトの現在の速度。
    public Vector3 Acceleration;       //オブジェクトの加速度（力の結果として計算される）。
    public Vector3 Force;              //オブジェクトにかかる総合的な力。

    public Vector3 AngularVelocity;    //オブジェクトの回転速度（角速度）。
    public Quaternion Rotation;        //オブジェクトの回転情報。

    float AngularDrag;          //回転にかかる抵抗の値。
    public bool UseGravity;            //重力を適用するかどうか。
    bool IsKinematic;           //物理シミュレーションを無視して手動で制御するかどうか。

    public Vector3 VelocityMaxValue;
    public Vector3 VelocityMinValue;


    private bool onMass_ = false;
    private bool onDrag_ = false;
    private bool onGravity_ = false;
    public bool onMass { get => onMass_; set => onMass_ = value; }
    public bool onDrag { get => onDrag_; set => onDrag_ = value; }
    public bool onGravity { get => onGravity_; set => onGravity_ = value; }

    public void initialize()
    {

    }

    public void SetUp(Vector3 pos,bool mass, bool drag, bool gravity, Vector3 VelocityMax ,Vector3 VelocityMin)
    {
        Position = pos;
        OnMass_Drag_Gravity(mass, drag, gravity);
        VelocityMaxValue = VelocityMax;
        VelocityMinValue = VelocityMin;

    }

    public void OnMass_Drag_Gravity(bool m, bool d, bool g)
    {
        onMass = m; onDrag = d; onGravity = g;
    }
    public float OnGravity()
    {
        return Gravity;
    }

    public float OnDrag()
    {
        return Drag;
    }

    public float AddDrag()
    {
        return Drag;
    }
    public float Dragon(float speed)
    {

        float t = Time.deltaTime;

        // 摩擦による減速処理
        float frictionForce = Drag * speed;
        float frictionAcceleration = frictionForce / Mass;
        speed -= frictionAcceleration * t;

        // 速度が逆転しないようにクランプ
        //if (Mathf.Abs(speed) < 0)
        //{
        //    speed = 0;
        //}

        return speed;
    }
    public void ApplyForce(Vector3 force)
    {
        // 力を質量で割ることで加速度を得る
        Vector3 acceleration = force / Mass;

        // 加速度を速度に加える
        Velocity += acceleration * Time.deltaTime;
    }
    public void ApplyTorque(Vector3 torque)
    {
        // 質量に対してトルクを適用し、角速度を増加させる
        Vector3 angularAcceleration = torque / Mass;

        // 角速度を更新する
        AngularVelocity += angularAcceleration * Time.deltaTime;
    }
    public void UpdatePosition(float deltaTime)
    {
        // 速度に基づいて位置を更新する
        Position += Velocity * deltaTime;
    }
    public void UpdateRotation(float deltaTime)
    {
        // 角速度に基づいて回転を更新する
        Quaternion deltaRotation = Quaternion.Euler(AngularVelocity * deltaTime);
        Rotation = Rotation * deltaRotation;
    }
    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                // 連続的な力を加える
                ApplyForce(force);
                break;
            case ForceMode.Impulse:
                // 瞬間的な力を加える
                ApplyForce(force / Time.deltaTime);
                break;
                // 他のForceModeは必要に応じて実装
        }
    }
    public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                // 連続的なトルクを加える
                ApplyTorque(torque);
                break;
            case ForceMode.Impulse:
                // 瞬間的なトルクを加える
                ApplyTorque(torque / Time.deltaTime);
                break;
                // 他のForceModeは必要に応じて実装
        }
    }
    public void ApplyGravity(float value = -9.81f)
    {
        if (UseGravity)
        {
            // 重力を適用する（地球の重力加速度を使用）
            Vector3 gravityForce = new Vector3(0, value * Mass, 0);
            ApplyForce(gravityForce);
        }
    }
    public void UpdateDrag(float deltaTime,float value = 0)
    {
        // 線形速度に対する空気抵抗を適用
        Velocity *= 1f / (1f + Drag * deltaTime);

        // 角速度に対する抵抗を適用
        AngularVelocity *= 1f / (1f + AngularDrag * deltaTime);
    }

    void VelocityClamp()
    {
        Vector3 v = Vector3.zero;

        v.x = Mathf.Min(Velocity.x, VelocityMaxValue.x);
        //v.y = Mathf.Min(Velocity.y, VelocityMaxValue.y);        //値が低い方を取得(上限より上には上がらない)
        v.z = Mathf.Min(Velocity.z, VelocityMaxValue.z);

        v.x = Mathf.Max(Velocity.x, VelocityMinValue.x);
        //v.y = Mathf.Max(Velocity.y, VelocityMinValue.y);        //値が高い方を取得(下限より低くはならない)
        v.z = Mathf.Max(Velocity.z, VelocityMinValue.z);

        Velocity = v;
    }
}
