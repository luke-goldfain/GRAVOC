using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private bool cursorLocked;

    private void Start()
    {
        cursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;
        }

        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;
        }
    }
}
