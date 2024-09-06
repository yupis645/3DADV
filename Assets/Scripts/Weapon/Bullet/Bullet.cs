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

    // ヒットしたオブジェクトを記憶するためのHashSet
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

        //生存時間のカウント
        if (lifeTime > 0 )                      //生存時間が0以上 かつ 生存時間が初期(-1)でないなら
        {
            lifeTime -= Time.deltaTime;                             //生存時間を1fごとに減少させる
        }

        //オブジェクトの破棄判定
        if (IsDestruction()) Destroy(gameObject);               //メソッドでtrueが帰ってきたらここでオブジェクトを破壊する

        transform.position += ShotBehaviorUpdate();


        // 爆風範囲内のオブジェクトにダメージを与える処理
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f,destroyHitObjLayer);
        foreach (var hitCollider in hitColliders)
        {
            // すでにヒットしたオブジェクトは無視
            if (!hitObjects.Contains(hitCollider))
            {
                // 新しくヒットしたオブジェクトを記録
                hitObjects.Add(hitCollider);

                // ダメージ処理
                // hitCollider.GetComponent<Health>()?.TakeDamage(damage);
                Debug.Log($"弾丸({hitCollider.name})");
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
    //                      消滅条件
    //
    // 爆発が消滅する条件を達成しているかを確認するメソッド
    // 生存時間が０以下になる、もしくは目標の半径より大きくなる事が消滅条件。
    // 生存時間の設定がある場合は目標半径より大きくなっても消滅せず、半径もそれ以上大きくならない
    //==================================================================================================
    bool IsDestruction()
    {
        //生存時間が0以下になったらtrueを返す。
        if (lifeTime < 0)      //lifetimeが0以下なら入る。ただし、lifetieが初期値の場合はこの判定は無視する
        {
            return true;
        }

        return false;
    }
}
