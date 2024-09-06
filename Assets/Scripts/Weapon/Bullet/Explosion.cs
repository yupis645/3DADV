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

    // ヒットしたオブジェクトを記憶するためのHashSet
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
        // 爆風の広がりをシミュレート
        if (currentRadius <= radius)                            //現在の半径が目標の半径より小さければ
        {
            currentRadius += expansionSpeed * Time.deltaTime;       //半径を広げる
        }

        //生存時間のカウント
        if (lifeTime > 0 && lifeTime != -1)                      //生存時間が0以上 かつ 生存時間が初期(-1)でないなら
        {
            lifeTime -= Time.deltaTime;                             //生存時間を1fごとに減少させる
        }

        //オブジェクトの破棄判定
        if (IsDestruction()) Destroy(gameObject);               //メソッドでtrueが帰ってきたらここでオブジェクトを破壊する
      
        float scaleValue = currentRadius / originalRadius;

        // スケールを適用
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);


        // 爆風範囲内のオブジェクトにダメージを与える処理
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (var hitCollider in hitColliders)
        {
            // すでにヒットしたオブジェクトは無視
            if (!hitObjects.Contains(hitCollider))
            {
                // 新しくヒットしたオブジェクトを記録
                hitObjects.Add(hitCollider);

                // ダメージ処理
                // hitCollider.GetComponent<Health>()?.TakeDamage(damage);
                //Debug.Log($"({hitCollider.name})");
            }
        }
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
        //生存時間が設定されていない場合
        if (lifeTime == -1)
        {
            //現在の半径が目標の半径を超えたらtureを返す
            if (currentRadius > radius)                            
            {
                return true;                                      
            }
        }

        //生存時間が設定されている場合
        else
        {
            //生存時間が0以下になったらtrueを返す。
            if (lifeTime < 0)      //lifetimeが0以下なら入る。ただし、lifetieが初期値の場合はこの判定は無視する
            {
                return true;
            }
        }


        return false;
    }

    void OnDrawGizmos()
    {
        // エディタで爆風範囲を視覚化するためのGizmo描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
