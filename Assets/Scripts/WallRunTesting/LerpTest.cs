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
    [SerializeField] BoxCollider boxCollider;

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
            //allowWallRun = !allowWallRun;

            if (allowWallRun)
            {
                MovementControl.gravity = 0;
                MovementControl.ResetVelocity();
            }
            else
            {
                MovementControl.gravity = savedGravity;
            }
        }

        if (allowWallRun)
        {
            Controller.myMovement.MoveVelocity((Goal_B.position - Goal_A.position).normalized * 15);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        allowWallRun = true;
        MovementControl.gravity = 0;
        MovementControl.ResetVelocity();
    }

    private void OnTriggerExit(Collider collider)
    {
        allowWallRun = false;
        MovementControl.gravity = savedGravity;
        Controller.myMovement.MoveVelocity((Controller.transform.forward));
    }
}

