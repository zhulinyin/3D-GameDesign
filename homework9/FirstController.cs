using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour,ISceneController,IUserAction {

    SSDirector director;
    public SSActionManager actionManager { get; set; }
    public List<GameObject> tanks = null;
    public float LaunchForce = 15f;
    private GameState gameState;
    private GameObject player;
    private Rigidbody m_Rigidbody;
    public float m_Speed = 12f;                 // 坦克移动速度
    public float m_TurnSpeed = 180f;            // 坦克转弯速度
    void Awake()
    {
        director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        actionManager = GetComponent<CCActionManager>();
        m_Rigidbody = player.GetComponent<Rigidbody>();

    }
    // Use this for initialization
    void Start () {
        gameState = GameState.ROUND_START;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameState == GameState.GAMEOVER) return;
        for(int i = 0; i < tanks.Count; i++)
        {
            tanks[i].transform.position = new Vector3(tanks[i].transform.position.x, 0, tanks[i].transform.position.z);
        }
        if (tanks.Count < 5)        //若坦克小于5只，则产生新的坦克
        {
            tanks.Add(Singleton<Factory>.Instance.GetTank());
        }
        if (gameState != GameState.GAMEOVER && Singleton<ScoreRecorder>.Instance.GetScore() <= 0)       //若分数小于等于0，则游戏结束，销毁所有动作
        {
            gameState = GameState.GAMEOVER;
            player.SetActive(false);
            actionManager.DestroyAllActions();
        }
    }

    public void Move(float MovementInputValue)      //玩家移动
    {
        Vector3 movement = player.transform.forward * MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }
    public void Turn(float TurnInputValue)      //玩家旋转
    {
        float turn = TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
    public void shoot()     //玩家射击
    {
        GameObject shellInstance = Singleton<Factory>.Instance.GetShell(TankType.PLAYER);
        shellInstance.transform.position = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z) +
            player.transform.forward * 1.5f;
        shellInstance.transform.forward = player.transform.forward;
        shellInstance.transform.eulerAngles = new Vector3(-10, 0, 0) + player.transform.eulerAngles;
        shellInstance.GetComponent<Rigidbody>().velocity = LaunchForce * shellInstance.transform.forward;
    }
    public GameObject GetPlayer()       //获取玩家
    {
        return player;
    }
    public void LoadResources()     //加载游戏资源
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/LevelArt"));
        player = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/player"));
        for (int i = 0; i < 5; i++)
        {
            tanks.Add(Singleton<Factory>.Instance.GetTank());
        }
    }
    public void Pause()
    {

    }
    public void Resume()
    {
        
    }    
    public void Restart()       //重新开始游戏
    {
        SceneManager.LoadScene("main");
    }
    public GameState GetGameState()     //获取当前游戏状态
    {
        return gameState;
    }
    public void SetGameState(GameState gameState)       //设置当前游戏状态
    {
        this.gameState = gameState;
    }
    public void GameOver()      //游戏结束
    {
        gameState = GameState.GAMEOVER;
        player.GetComponent<Animator>().SetBool("death", true);
        player.GetComponent<Animator>().SetTrigger("dieTrigger");
        actionManager.DestroyAllActions();
        foreach(GameObject patrol in tanks)
        {
            patrol.GetComponentInChildren<Animator>().SetBool("run", false);
            patrol.transform.localEulerAngles = new Vector3(0, patrol.transform.localEulerAngles.y, 0);
            patrol.transform.position = new Vector3(patrol.transform.position.x, 0, patrol.transform.position.z);
        }
    }
}
