using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraPan : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.down * 0.05f, Space.World);
    }
}
