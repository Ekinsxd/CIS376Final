using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recoverMouseScript : MonoBehaviour
{
    /// <summary>
    /// The function sets the cursor lock state to none and makes it visible in a C# program.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
