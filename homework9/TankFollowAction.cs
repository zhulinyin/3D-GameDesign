using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankFollowAction : SSAction
{
    private GameObject player;
    private float LaunchForce = 15f;
    private float shootTime = 2f;
    private FirstController firstController;
    public static TankFollowAction GetSSAction()
    {
        TankFollowAction action = ScriptableObject.CreateInstance<TankFollowAction>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        firstController = SSDirector.GetInstance().CurrentSceneController as FirstController;
        player = firstController.GetPlayer();
        agent = gameobject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public override void Update()
    {
        // 若坦克处于非活跃或游戏结束，则销毁动作
        if (!gameobject.activeSelf || firstController.GetGameState() == GameState.GAMEOVER)
        {
            this.destroy = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            return;
        }
        // 若坦克处于非跟踪玩家状态，则销毁动作
        if (!gameobject.GetComponent<TankData>().follow)
        {
            this.destroy = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            this.callback.SSActionEvent(this,1,gameobject);
            return;
        }
        // 跟踪玩家
        agent.SetDestination(player.transform.position);
        // 每隔两秒发一次子弹
        shootTime -= Time.deltaTime;
        if (shootTime <= 0)
        {
            shootTime = 2f;
            shoot();
        }
    }

    void shoot()    // 当坦克与玩家距离小于20时，发射子弹
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 20)
        {
            GameObject shell = Singleton<Factory>.Instance.GetShell(TankType.ENEMY);//获取子弹
            shell.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z) +
                transform.forward * 1.5f;//设置子弹
            shell.transform.forward = transform.forward;//设置子弹方向
            shell.transform.eulerAngles = new Vector3(-10, 0, 0) + transform.eulerAngles;
            shell.GetComponent<Rigidbody>().velocity= LaunchForce * shell.transform.forward;
        }
    }
}
