using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour {

    private Button btn;
    public Text text;
    private enum State { RotateIn, RotateOut, normal };
    private State state = State.normal;
    private int frame = 20;
    private float rotate = 90;
    private float size = 0;

    // Use this for initialization  
    void Start()
    {
        btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }

    void Update()
    {
        if (state == State.RotateIn)
        {
            if (rotate == 90f)
            {
                state = State.normal;
                text.gameObject.SetActive(false);
            }
            else
            {
                rotate += 90f / frame;
                size -= 100f / frame;
                text.transform.rotation = Quaternion.Euler(rotate, 0, 0);
                text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, size);
            }
        }
        else if (state == State.RotateOut)
        {
            if (rotate == 0f)
            {
                state = State.normal;
            }
            else
            {
                rotate -= 90f / frame;
                size += 100f / frame;
                text.transform.rotation = Quaternion.Euler(rotate, 0, 0);
                text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, size);
            }
        }
    }    
    void Click()
    {
        if (text.gameObject.activeSelf)
        {
            state = State.RotateIn;
        }
        else
        {
            state = State.RotateOut;
            text.gameObject.SetActive(true);
        }

    }
}
