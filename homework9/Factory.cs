using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    public GameObject tankPrefab;
    public GameObject shellPrefab;
    public ParticleSystem psPrefab;
    public GameObject player;

    private Dictionary<int, GameObject> usingTanks;
    private Dictionary<int, GameObject> freeTanks;
    private Dictionary<int, GameObject> usingShells;
    private Dictionary<int, GameObject> freeShells;
    private List<ParticleSystem> particleSystems;

    private void Awake()
    {
        usingTanks = new Dictionary<int, GameObject>();
        freeTanks = new Dictionary<int, GameObject>();
        usingShells = new Dictionary<int, GameObject>();
        freeShells = new Dictionary<int, GameObject>();
        particleSystems = new List<ParticleSystem>();
    }

    private void Start()
    {
        player = (SSDirector.GetInstance().CurrentSceneController as FirstController).GetPlayer();  //获取玩家

    }
    public GameObject GetTank()     //获取坦克
    {
        if (freeTanks.Count == 0)
        {
            GameObject newTank = Instantiate<GameObject>(tankPrefab);
            usingTanks.Add(newTank.GetInstanceID(), newTank);

            //在一个随机范围内设置坦克位置，如果距离玩家太近，则重新随机
            Vector3 position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            while (Vector3.Distance(position, player.transform.position) < 25)
            {
                position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
            newTank.transform.position = position;

            //随机产生一个坦克追踪的目标，如果距离塔克太近，则重新随机
            Vector3 target = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            while (Vector3.Distance(target, newTank.transform.position) < 10)
            {
                target = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
            newTank.GetComponent<TankData>().target = target;
            newTank.GetComponent<TankData>().follow = false;
            return newTank;
        }
        foreach (KeyValuePair<int, GameObject> pair in freeTanks)
        {
            Vector3 position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            while (Vector3.Distance(position, player.transform.position) < 25)
            {
                position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
            pair.Value.transform.position = position;

            Vector3 target = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            while (Vector3.Distance(target, pair.Value.transform.position) < 10)
            {
                target = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            }
            pair.Value.GetComponent<TankData>().target = target;
            pair.Value.GetComponent<TankData>().follow = false;
            pair.Value.SetActive(true);
            freeTanks.Remove(pair.Key);
            usingTanks.Add(pair.Key, pair.Value);            
            return pair.Value;
        }
        return null;
    }
    public GameObject GetShell(TankType tankType)       //获取子弹并指明子弹是由什么坦克发出的
    {
        if (freeShells.Count == 0)
        {
            GameObject newBullet = Instantiate<GameObject>(shellPrefab);
            newBullet.GetComponent<Shell>().tankType = tankType;
            usingShells.Add(newBullet.GetInstanceID(), newBullet);
            return newBullet;
        }
        foreach (KeyValuePair<int, GameObject> pair in freeShells)
        {
            pair.Value.SetActive(true);
            pair.Value.GetComponent<Shell>().tankType = tankType;
            freeShells.Remove(pair.Key);
            usingShells.Add(pair.Key, pair.Value);
            return pair.Value;
        }
        return null;
    }
    public ParticleSystem getPs()   //获取爆炸的粒子效果
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            if (!particleSystems[i].isPlaying)
            {
                return particleSystems[i];
            }
        }
        ParticleSystem newPs = Instantiate<ParticleSystem>(psPrefab);
        particleSystems.Add(newPs);
        return newPs;
    }
    public void recycleTank(GameObject tank)    //回收坦克
    {
        usingTanks.Remove(tank.GetInstanceID());
        freeTanks.Add(tank.GetInstanceID(), tank);
        tank.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        tank.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        tank.SetActive(false);
    }

    public void recycleBullet(GameObject shell)     //回收子弹
    {
        usingShells.Remove(shell.GetInstanceID());
        freeShells.Add(shell.GetInstanceID(), shell);
        shell.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        shell.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        shell.SetActive(false);
    }
}
