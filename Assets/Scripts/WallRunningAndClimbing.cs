using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class WallRunningAndClimbing : MonoBehaviour
{
    public Vector3 alongWall;
    [SerializeField] PlayerMovement myPlayer;

    [SerializeField] float maxWallDistance; // how far away we detect walls
    [SerializeField] float minimumHeight; // how far off the ground we need to be to wall run
    [SerializeField] float cameraAngleRoll = 20; // for if we want the camera to change angle when we wall run
    [SerializeField] float maxAngleRoll;
    

    [Range(0.0f, 1.0f)]
    public float normalizedAngleThreshold = 0.1f; // threshold to tell if we are wall running

    public float jumpDuration; // prevents the player from chaining wall jumps back to back
    public float wallBouncing; // sets the direction for the player
    public float cameraTransitionDuration; // how long to transition from wall running camera affect back to normal

    //[Space]
    //public Volume wallRunVolume; // volume of camera affect when wall running
    //float lastVolumeValue;

    Vector3[] directions; // checks around player if theres a wall to run on
    RaycastHit[] hits; // stores info about what we hit for wall running

    public bool isWallRunning = false; // is the player wall running right now?
    Vector3 lastwallPosition;
    Vector3 lastWallNormal;
    [SerializeField] float timeSinceJump = 0;
    [SerializeField] float timeSinceWallAttach = 0;
    [SerializeField] float timeSinceWallDetach = 0;
    [SerializeField] bool isJumping = false;

    bool isPlayergrounded() => myPlayer.isGrounded;

    public bool IsWallRunning() => isWallRunning;

    bool CanWallRun() // checks several variables to see if we can start wall running
    {
        return !isPlayergrounded() && myPlayer.inputMovement.y > 0 && VerticalCheck();
    }

    bool VerticalCheck() // checks if we are at the minimum height to wall run
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }

    bool canAttach() // checks if the player is able to attach to a wall
    {
        if (myPlayer.isGrounded) // checks if we are jumping
        {
            timeSinceJump += Time.deltaTime; // determines how long its been since we jumped
            if (timeSinceJump > jumpDuration) // checks how long its been since we jumped
            {
                timeSinceJump = 0;
                isJumping = false;
            }
            return false;
        }
        return true;
    }

    void Start()
    {

        directions = new Vector3[] // saves a list of directions for the wall run checker
        {
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left,
            Vector3.left + Vector3.forward
        };

        //if(wallRunVolume != null) // turns off special wall running affect when the scene starts in case its on
        //{
        //    SetVolumeWeight(0);
        //}
    }

    void LateUpdate()
    {
        isWallRunning = false;
        
        if (canAttach()) // if we can attach to something
        {
            Debug.Log("Can attach");
            hits = new RaycastHit[directions.Length]; // saves our raycast info

            for (int i = 0; i < directions.Length; i++) // a for loop for shooting raycast
            {
                Vector3 dir = transform.TransformDirection(directions[i]); // determines which direction for the raycast to go
                Physics.Raycast(transform.position, dir, out hits[i], maxWallDistance); // shoots a raycast with the information above
                if (hits[i].collider != null && hits[i].collider.gameObject != gameObject) // if we hit something
                {
                    isWallRunning = true;
                    Debug.Log(hits[i].collider.name);
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green); //the line turns green
                }
                else // if we don't
                {
                    Debug.DrawRay(transform.position, dir * maxWallDistance, Color.red); // the line turns red
                }
            }

            if (CanWallRun())
            {
                hits = hits.ToList().Where(hits => hits.collider != null).OrderBy(hits => hits.distance).ToArray(); // saves every hit object to an array arranged by distance
                if (hits.Length > 0) // prioritizes the first collider in the array which is the closest one
                {
                    OnWall(hits[0]);
                    lastwallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }
            }

            if (isWallRunning) // if we are wall running
            {
                Debug.Log("Wall running");
                timeSinceWallDetach = 0; // resets the time since we detached from a wall
                //if(timeSinceWallAttach == 0 && wallRunVolume != null) // sets the volume
                //{
                //    lastVolumeValue = wallRunVolume.weight;
                //}
                timeSinceWallAttach += Time.deltaTime; // keeps track of time on the wall
            }
            else
            {
                Debug.Log("Not wall running");
                timeSinceWallAttach = 0; // resets how long we've been on a wall for
                //if(timeSinceWallDetach == 0 && wallRunVolume != null) // sets the volume back
                //{
                //    lastVolumeValue = wallRunVolume.weight;
                //}
                timeSinceWallDetach += Time.deltaTime; // keeps track of how long we haven't been on a wall
            }

            //if(wallRunVolume != null)
            //{
            //    HandleVolume();
            //}
        }
    }

    void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if(d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            alongWall = transform.TransformDirection(Vector3.forward);

            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);

            //myPlayer.velocity = alongWall;
        }
    }

    float CalculateSide()
    {
        if(isWallRunning)
        {
            Vector3 heading = lastwallPosition - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = myPlayer.cameraComponent.transform.eulerAngles.z;
        float targetAngle = 0;
        if (dir != 0)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }
        return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(timeSinceWallAttach, timeSinceWallDetach) / cameraAngleRoll);
    }

    public Vector3 GetWallJumpDirection()
    {
        if(isWallRunning)
        {
            return lastWallNormal * wallBouncing + (Vector3.up * myPlayer.jumpHeight * -2f * myPlayer.gravity);
        }
        return Vector3.zero;
    }

    //void HandleVolume() // determines what the weight of the wall run volume affect
    //{
    //    float w = 0;
    //    if(isWallRunning)
    //    {
    //        w = Mathf.Lerp(lastVolumeValue, 1, timeSinceWallAttach / cameraTransitionDuration);
    //    }
    //    else
    //    {
    //        w = Mathf.Lerp(lastVolumeValue, 1, timeSinceWallDetach / cameraTransitionDuration);
    //    }

    //    SetVolumeWeight(w);
    //}

    //void SetVolumeWeight(float weight) // sets the weight of the wall run volume affect
    //{
    //    wallRunVolume.weight = weight;
    //}
}
