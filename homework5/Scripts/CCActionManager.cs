using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : MonoBehaviour,IActionManager,ISSActionCallback {

    public FirstController SceneController;
    private DiskFly diskfly;
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    // Use this for initialization
    protected void Start()
    {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        SceneController.actionManager = this;
    }

    // Update is called once per frame  
    protected void Update()
    {
        foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    public void playDisk(GameObject gameObject)
    {
        
        diskfly = DiskFly.GetSSAction();
        this.RunAction(gameObject, diskfly, this);
        
    }
    
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        if(source is DiskFly)
        {
            source.gameobject.SetActive(false);
        }
    }
}
