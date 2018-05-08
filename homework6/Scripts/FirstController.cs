using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour,ISceneController,IUserAction {

    SSDirector director;
    public SSActionManager actionManager { get; set; }
    public List<GameObject> Patrols = null;
    private GameState gameState;
    private GameObject player;
    private float moveSpeed = 6;
    private float turnSpeed = 10;
    private double speedTime = 0;
    void Awake()
    {
        director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        actionManager = GetComponent<CCActionManager>();
    }
	// Use this for initialization
	void Start () {
        gameState = GameState.ROUND_START;
    }
	
	// Update is called once per frame
	void Update () {
        if (speedTime > 0)
        {
            speedTime -= Time.deltaTime;
        }
        else//加速技能持续3秒，加速结束恢复玩家速度
        {
            moveSpeed = 6;
        }
    }
    public GameObject GetPlayer()//获取玩家
    {
        return player;
    }
    public void LoadResources()//加载游戏资源
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Scene"));
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Plane"));
        player = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/player"),new Vector3(1,0,0),Quaternion.identity);
        Patrols = Singleton<PatrolFactory>.Instance.GetPatrol();
    }
    public void Pause()
    {

    }
    public void Resume()
    {
        
    }
    void Follow(GameObject ob)//巡逻兵跟随
    {
        ob.GetComponentInParent<PatrolData>().follow = true;
    }
    void Escape(GameObject ob)//玩家脱离
    {
        ob.GetComponentInParent<PatrolData>().follow = false;
        Singleton<ScoreRecorder>.Instance.AddScore();
    }
    void OnEnable()
    {
        GameEventManager.OnGameOver += GameOver;
        GameEventManager.OnPatrolFollow += Follow;
        GameEventManager.OnPlayerEscape += Escape;
    }
    void OnDisable()
    {
        GameEventManager.OnGameOver -= GameOver;
        GameEventManager.OnPatrolFollow -= Follow;
        GameEventManager.OnPlayerEscape -= Escape;
    }
    public void PlayerMove(Vector3 target)//玩家向目标移动
    {
        if (gameState != GameState.GAMEOVER)
        {
            if (target.Equals(Vector3.zero))
            {
                player.GetComponent<Animator>().SetBool("run", false);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("run", true);
                player.transform.position += target * moveSpeed * Time.deltaTime;
            }
        }
    }
    public void SpeedUp()//加速
    {
        moveSpeed = 10;
        speedTime = 3;
    }
    public void PlayerRotate(Vector3 direction)
    {
        if (gameState != GameState.GAMEOVER)
        {
            if (!direction.Equals(Vector3.zero))
            {
                //将方向转换为四元数  
                Quaternion quaDir = Quaternion.LookRotation(direction, Vector3.up);
                //缓慢转动到目标点  
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, quaDir, Time.fixedDeltaTime * turnSpeed);
            }
        }       
    }
    public void Restart()//重新开始游戏
    {
        SceneManager.LoadScene("main");
    }
    public GameState GetGameState()//获取当前游戏状态
    {
        return gameState;
    }
    public void SetGameState(GameState gameState)//设置当前游戏状态
    {
        this.gameState = gameState;
    }
    public void GameOver()//游戏结束
    {
        gameState = GameState.GAMEOVER;
        player.GetComponent<Animator>().SetBool("death", true);
        player.GetComponent<Animator>().SetTrigger("dieTrigger");
        actionManager.DestroyAllActions();
        foreach(GameObject patrol in Patrols)
        {
            patrol.GetComponentInChildren<Animator>().SetBool("run", false);
            patrol.transform.localEulerAngles = new Vector3(0, patrol.transform.localEulerAngles.y, 0);
            patrol.transform.position = new Vector3(patrol.transform.position.x, 0, patrol.transform.position.z);
        }
    }
}
