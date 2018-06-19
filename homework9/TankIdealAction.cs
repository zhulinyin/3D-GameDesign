using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankIdealAction : SSAction
{
    private float speed = 10f;
    private FirstController firstController;
    public static TankIdealAction GetSSAction()
    {
        TankIdealAction action = ScriptableObject.CreateInstance<TankIdealAction>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        agent = gameobject.GetComponent<NavMeshAgent>();
        firstController = SSDirector.GetInstance().CurrentSceneController as FirstController;
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
        // 若坦克处于跟踪玩家状态，则销毁动作
        if (gameobject.GetComponent<TankData>().follow)
        {
            this.destroy = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            this.callback.SSActionEvent(this, 0, gameobject);
            return;
        }
        // 在地图上随机一个坐标进行追踪，若距离目标小于10，则更换追踪目标，实现随机巡逻
        while(Vector3.Distance(gameobject.GetComponent<TankData>().target, transform.position) < 10)
        {
            gameobject.GetComponent<TankData>().target = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        }
        agent.SetDestination(gameobject.GetComponent<TankData>().target);
    }
}
