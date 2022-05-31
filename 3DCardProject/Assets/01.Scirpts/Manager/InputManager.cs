using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool playerControllerInputBlocked;

    public bool m_MouseUp;
    public bool m_MouseBtn;


    public bool MouseUp
    {
        get { return m_MouseUp; }
    }

    public bool MouseDown
    {
        get { return m_MouseBtn; }
    }

    void Update()
    {
            m_MouseUp = false;
        if (Input.GetMouseButtonUp(0))
        {
            m_MouseUp = true;
        }

        if (Input.GetMouseButton(0))
            m_MouseBtn = true;
    }


}
