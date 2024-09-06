using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float originalRadius = 0.5f;
    [SerializeField] private float radius;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime = -1;
    [SerializeField] private float expansionSpeed;
    [SerializeField] private float currentRadius;

    // �q�b�g�����I�u�W�F�N�g���L�����邽�߂�HashSet
    private HashSet<Collider> hitObjects = new HashSet<Collider>();

    public static Explosion CreateExplosion(Vector3 position, float damage, float expansionSpeed, float radius, float lifeTime = -1)
    {
        GameObject explosionObject = new GameObject("Explosion");
        explosionObject.transform.position = position;

        Explosion explosion = explosionObject.AddComponent<Explosion>();
        explosion.Initialize( damage, expansionSpeed,  radius, lifeTime);

        return explosion;
    }
    void Initialize( float damage,  float expansionSpeed, float radius , float lifeTime = -1)
    {
        this.radius = radius;
        this.damage = damage;
        this.lifeTime = lifeTime;
        this.expansionSpeed = expansionSpeed;
        currentRadius = transform.localScale.x;

    }

    void SetUp()
    {
        
    }

    void Update()
    {
        // �����̍L������V�~�����[�g
        if (currentRadius <= radius)                            //���݂̔��a���ڕW�̔��a��菬�������
        {
            currentRadius += expansionSpeed * Time.deltaTime;       //���a���L����
        }

        //�������Ԃ̃J�E���g
        if (lifeTime > 0 && lifeTime != -1)                      //�������Ԃ�0�ȏ� ���� �������Ԃ�����(-1)�łȂ��Ȃ�
        {
            lifeTime -= Time.deltaTime;                             //�������Ԃ�1f���ƂɌ���������
        }

        //�I�u�W�F�N�g�̔j������
        if (IsDestruction()) Destroy(gameObject);               //���\�b�h��true���A���Ă����炱���ŃI�u�W�F�N�g��j�󂷂�
      
        float scaleValue = currentRadius / originalRadius;

        // �X�P�[����K�p
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);


        // �����͈͓��̃I�u�W�F�N�g�Ƀ_���[�W��^���鏈��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (var hitCollider in hitColliders)
        {
            // ���łɃq�b�g�����I�u�W�F�N�g�͖���
            if (!hitObjects.Contains(hitCollider))
            {
                // �V�����q�b�g�����I�u�W�F�N�g���L�^
                hitObjects.Add(hitCollider);

                // �_���[�W����
                // hitCollider.GetComponent<Health>()?.TakeDamage(damage);
                //Debug.Log($"({hitCollider.name})");
            }
        }
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
        //�������Ԃ��ݒ肳��Ă��Ȃ��ꍇ
        if (lifeTime == -1)
        {
            //���݂̔��a���ڕW�̔��a�𒴂�����ture��Ԃ�
            if (currentRadius > radius)                            
            {
                return true;                                      
            }
        }

        //�������Ԃ��ݒ肳��Ă���ꍇ
        else
        {
            //�������Ԃ�0�ȉ��ɂȂ�����true��Ԃ��B
            if (lifeTime < 0)      //lifetime��0�ȉ��Ȃ����B�������Alifetie�������l�̏ꍇ�͂��̔���͖�������
            {
                return true;
            }
        }


        return false;
    }

    void OnDrawGizmos()
    {
        // �G�f�B�^�Ŕ����͈͂����o�����邽�߂�Gizmo�`��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
