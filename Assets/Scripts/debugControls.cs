using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugControls : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject playerObject;
    public GameObject playerStartPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Debug - Player Position Reset"))
        {
            Debug.Log("1");
            playerObject.transform.position = playerStartPosition.transform.position;

            this.gameObject.transform.position = playerStartPosition.transform.position;
        }
    }
}
