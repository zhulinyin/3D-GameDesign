using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankType { PLAYER,ENEMY};
public class Shell : MonoBehaviour {

    public float m_ExplosionForce = 300;              // 爆炸力
    public float m_ExplosionRadius = 1f;              // 爆炸半径
    public TankType tankType;                         // 坦克类型

    private void OnCollisionEnter(Collision other)
    {
        // 获取爆炸半径内所有碰撞盒
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);

        // 遍历所有碰撞盒
        for (int i = 0; i < colliders.Length; i++)
        {
            // 若该碰撞盒勾选了isTrigger，则该碰撞盒用于敌人坦克追踪玩家的检测包围盒
            if (colliders[i].isTrigger) continue;

            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
                continue;

            // 给刚体添加一个爆炸力
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            // 若该子弹由玩家发出且击中敌人坦克，则加分并回收敌人坦克
            if (colliders[i].tag.Contains("Finish") && tankType == TankType.PLAYER)
            {
                Singleton<Factory>.Instance.recycleTank(colliders[i].gameObject);
                (SSDirector.GetInstance().CurrentSceneController as FirstController).tanks.Remove(colliders[i].gameObject);
                Singleton<ScoreRecorder>.Instance.AddScore();
            }

            // 若该子弹由敌人坦克发出且击中玩家，则扣分
            if(colliders[i].tag.Contains("Player") && tankType == TankType.ENEMY)
            {
                Singleton<ScoreRecorder>.Instance.SubScore();
            }
        }

        // 播放爆炸的粒子效果
        ParticleSystem explosion = Singleton<Factory>.Instance.getPs();
        explosion.transform.position = transform.position;
        explosion.Play();

        // 回收子弹
        Singleton<Factory>.Instance.recycleBullet(gameObject);
    }
}
