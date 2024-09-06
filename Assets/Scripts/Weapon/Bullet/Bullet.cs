using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float lifeTime = 0;
    [SerializeField] private float gravity = 0;

    public enum shotbehavior
    {
        straight,
        parabola,
        homing,
    }
    public shotbehavior guns;

    [SerializeField] LayerMask destroyHitObjLayer;

    // �q�b�g�����I�u�W�F�N�g���L�����邽�߂�HashSet
    private HashSet<Collider> hitObjects = new HashSet<Collider>();

    public static Bullet CreateBullet(Vector3 position, Vector3 velo, Vector3 tarpos, float lifetime, shotbehavior behavior)
    {
        GameObject bulletObject = new GameObject("bullet");
        bulletObject.transform.position = position;

        Bullet bullet = bulletObject.AddComponent<Bullet>();
        bullet.Initialize( velo, tarpos, lifetime, behavior);

        return bullet;
    }

    public void Initialize(Vector3 velo ,Vector3 tarpos,float lifetime , shotbehavior behavior)
    {
        velocity = velo;
        targetPosition = tarpos;
        lifeTime = lifetime;
        guns = behavior;
    }

    public void SetUp()
    {
        
    }

    private void Update()
    {
        ShotBehaviorUpdate();

        //�������Ԃ̃J�E���g
        if (lifeTime > 0 )                      //�������Ԃ�0�ȏ� ���� �������Ԃ�����(-1)�łȂ��Ȃ�
        {
            lifeTime -= Time.deltaTime;                             //�������Ԃ�1f���ƂɌ���������
        }

        //�I�u�W�F�N�g�̔j������
        if (IsDestruction()) Destroy(gameObject);               //���\�b�h��true���A���Ă����炱���ŃI�u�W�F�N�g��j�󂷂�

        transform.position += ShotBehaviorUpdate();


        // �����͈͓��̃I�u�W�F�N�g�Ƀ_���[�W��^���鏈��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f,destroyHitObjLayer);
        foreach (var hitCollider in hitColliders)
        {
            // ���łɃq�b�g�����I�u�W�F�N�g�͖���
            if (!hitObjects.Contains(hitCollider))
            {
                // �V�����q�b�g�����I�u�W�F�N�g���L�^
                hitObjects.Add(hitCollider);

                // �_���[�W����
                // hitCollider.GetComponent<Health>()?.TakeDamage(damage);
                Debug.Log($"�e��({hitCollider.name})");
                Destroy(gameObject);
            }
        }
    }

    private Vector3 ShotBehaviorUpdate()
    {
        Vector3 v = Vector3.zero;
        switch (guns)
        {
            case shotbehavior.straight:
                v = velocity * Time.deltaTime;
                break;
            case shotbehavior.parabola:
                break;
            case shotbehavior.homing:
                break;
        }

        return v;
    }

    //==================================================================================================
    //                      ���ŏ���
    //
    // ���������ł��������B�����Ă��邩���m�F���郁�\�b�h
    // �������Ԃ��O�ȉ��ɂȂ�A�������͖ڕW�̔��a���傫���Ȃ鎖�����ŏ����B
    // �������Ԃ̐ݒ肪����ꍇ�͖ڕW���a���傫���Ȃ��Ă����ł����A���a������ȏ�傫���Ȃ�Ȃ�
    //==================================================================================================
    bool IsDestruction()
    {
        //�������Ԃ�0�ȉ��ɂȂ�����true��Ԃ��B
        if (lifeTime < 0)      //lifetime��0�ȉ��Ȃ����B�������Alifetie�������l�̏ꍇ�͂��̔���͖�������
        {
            return true;
        }

        return false;
    }
}
