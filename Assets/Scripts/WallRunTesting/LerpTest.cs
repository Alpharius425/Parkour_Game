using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    [SerializeField] Transform Goal_A;
    [SerializeField] Transform Goal_B;
    [SerializeField] PlayerController Controller;
    [SerializeField] PlayerMovementUpdated MovementControl;
    [SerializeField] bool allowWallRun;
    [SerializeField] float Speed = 10;
    [SerializeField] float savedGravity;

    // Start is called before the first frame update
    void Start()
    {
        savedGravity = MovementControl.gravity;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            allowWallRun = !allowWallRun;
            MovementControl.gravity = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            MovementControl.gravity = savedGravity;
        }

        if (allowWallRun)
        {
            Controller.myMovement.MoveVelocity((Goal_B.position - Goal_A.position).normalized * 15);
        }

    }
}
