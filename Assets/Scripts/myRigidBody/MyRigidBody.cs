using System;
using UnityEngine;

[Serializable]
class MyRigidBody
{

    [SerializeField] private float Mass = 0;                 //�I�u�W�F�N�g�̎��ʁB
    [SerializeField] private float Drag = 0;              //�I�u�W�F�N�g�ɂ������C��R�̒l�B
    [SerializeField] private float Gravity = 0;

    [HideInInspector]
    public Vector3 Position;           //�I�u�W�F�N�g�̌��݂̈ʒu�B
    public Vector3 Velocity;           //�I�u�W�F�N�g�̌��݂̑��x�B
    public Vector3 Acceleration;       //�I�u�W�F�N�g�̉����x�i�͂̌��ʂƂ��Čv�Z�����j�B
    public Vector3 Force;              //�I�u�W�F�N�g�ɂ����鑍���I�ȗ́B

    public Vector3 AngularVelocity;    //�I�u�W�F�N�g�̉�]���x�i�p���x�j�B
    public Quaternion Rotation;        //�I�u�W�F�N�g�̉�]���B

    float AngularDrag;          //��]�ɂ������R�̒l�B
    public bool UseGravity;            //�d�͂�K�p���邩�ǂ����B
    bool IsKinematic;           //�����V�~�����[�V�����𖳎����Ď蓮�Ő��䂷�邩�ǂ����B

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

        // ���C�ɂ�錸������
        float frictionForce = Drag * speed;
        float frictionAcceleration = frictionForce / Mass;
        speed -= frictionAcceleration * t;

        // ���x���t�]���Ȃ��悤�ɃN�����v
        //if (Mathf.Abs(speed) < 0)
        //{
        //    speed = 0;
        //}

        return speed;
    }
    public void ApplyForce(Vector3 force)
    {
        // �͂����ʂŊ��邱�Ƃŉ����x�𓾂�
        Vector3 acceleration = force / Mass;

        // �����x�𑬓x�ɉ�����
        Velocity += acceleration * Time.deltaTime;
    }
    public void ApplyTorque(Vector3 torque)
    {
        // ���ʂɑ΂��ăg���N��K�p���A�p���x�𑝉�������
        Vector3 angularAcceleration = torque / Mass;

        // �p���x���X�V����
        AngularVelocity += angularAcceleration * Time.deltaTime;
    }
    public void UpdatePosition(float deltaTime)
    {
        // ���x�Ɋ�Â��Ĉʒu���X�V����
        Position += Velocity * deltaTime;
    }
    public void UpdateRotation(float deltaTime)
    {
        // �p���x�Ɋ�Â��ĉ�]���X�V����
        Quaternion deltaRotation = Quaternion.Euler(AngularVelocity * deltaTime);
        Rotation = Rotation * deltaRotation;
    }
    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                // �A���I�ȗ͂�������
                ApplyForce(force);
                break;
            case ForceMode.Impulse:
                // �u�ԓI�ȗ͂�������
                ApplyForce(force / Time.deltaTime);
                break;
                // ����ForceMode�͕K�v�ɉ����Ď���
        }
    }
    public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                // �A���I�ȃg���N��������
                ApplyTorque(torque);
                break;
            case ForceMode.Impulse:
                // �u�ԓI�ȃg���N��������
                ApplyTorque(torque / Time.deltaTime);
                break;
                // ����ForceMode�͕K�v�ɉ����Ď���
        }
    }
    public void ApplyGravity(float value = -9.81f)
    {
        if (UseGravity)
        {
            // �d�͂�K�p����i�n���̏d�͉����x���g�p�j
            Vector3 gravityForce = new Vector3(0, value * Mass, 0);
            ApplyForce(gravityForce);
        }
    }
    public void UpdateDrag(float deltaTime,float value = 0)
    {
        // ���`���x�ɑ΂����C��R��K�p
        Velocity *= 1f / (1f + Drag * deltaTime);

        // �p���x�ɑ΂����R��K�p
        AngularVelocity *= 1f / (1f + AngularDrag * deltaTime);
    }

    void VelocityClamp()
    {
        Vector3 v = Vector3.zero;

        v.x = Mathf.Min(Velocity.x, VelocityMaxValue.x);
        //v.y = Mathf.Min(Velocity.y, VelocityMaxValue.y);        //�l���Ⴂ�����擾(�������ɂ͏オ��Ȃ�)
        v.z = Mathf.Min(Velocity.z, VelocityMaxValue.z);

        v.x = Mathf.Max(Velocity.x, VelocityMinValue.x);
        //v.y = Mathf.Max(Velocity.y, VelocityMinValue.y);        //�l�����������擾(�������Ⴍ�͂Ȃ�Ȃ�)
        v.z = Mathf.Max(Velocity.z, VelocityMinValue.z);

        Velocity = v;
    }
}
