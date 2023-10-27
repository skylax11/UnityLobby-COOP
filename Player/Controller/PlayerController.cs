using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;  // Singleton

    [Header("Player Properties")]

    public readonly float WalkSpeed = 2.6f;
    public readonly float SprintSpeed = 5.0f;  // Setting values for movement
    public  float TotalSpeed;
    private float Rotation;

    public State PlayerState;
    public GroundState groundState;

    [Header("Prevent Obstacle")]
    [SerializeField] GameObject _groundCheck;
    [SerializeField] GameObject _obstacleCheck;
    RaycastHit hitCollider;
    RaycastHit hitObstacle;
    private Vector3[] hitDirections = { new Vector3(1,0,0), new Vector3(-1, 0, 0) , new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

    [Header("Preferences")]
    [SerializeField] PlayerInputScript m_PlayerInput;
    [SerializeField] PlayerCamera m_PlayerCamera;
    public Vector3 tempDirection;

    public enum State
    {
        Walking,
        Jumping,
        Sprinting,
        Standing
    }
    public enum GroundState
    {
        onGround,
        onAir
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<IConnector>(out IConnector connector))
        {
            connector.SetPuzzleByPlayer(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<IConnector>(out IConnector connector))
        {
            connector.SetPuzzleByPlayer(false);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    void Start()
    {
         if (IsOwner)
         {
             m_PlayerCamera.cam.enabled = true;
         }
         else
         {
             m_PlayerCamera.cam.enabled = false;
         }
    }
    
    void Update()
    {
        if (!IsOwner) return;

        switch (PlayerState)
        {
            case State.Walking:
                TotalSpeed = Mathf.Lerp(TotalSpeed,WalkSpeed,Time.deltaTime*10f);
            break;

            case State.Sprinting:
                TotalSpeed = Mathf.Lerp(TotalSpeed, SprintSpeed, Time.deltaTime * 20f);
            break;

            case State.Standing:
                TotalSpeed = Mathf.Lerp(TotalSpeed, 0f, Time.deltaTime * 5f);
            break;
        }

        GroundCheck();
        Rotate();
        Move();
    }
    public void Move()
    {
        
        Vector2 inputDirection = m_PlayerInput.direction;
        Vector3 MoveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);

        Vector3 PlayerDirection = (transform.position) - (transform.position + MoveDirection);

        if(PlayerDirection.x < 0)
            PlayerDirection.x = -1;
        else
            PlayerDirection.x = 1;
        if(PlayerDirection.z < 0)
            PlayerDirection.z = -1;
        else
            PlayerDirection.z = 1;

        if(ObstacleCheck(PlayerDirection))
            transform.Translate(MoveDirection*TotalSpeed*Time.deltaTime);

    }
    public void Rotate()
    {
        Vector2 inputDirection = m_PlayerInput.direction;
        Rotation = m_PlayerCamera.yRot;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f, Rotation, 0f), Time.deltaTime * 60f);
    }
    public void GroundCheck()
    {
        if (Physics.Raycast(_groundCheck.transform.position, new Vector3(0, -1, 0), out hitCollider, 1.1f))
        {
            if (!hitCollider.transform.CompareTag("Player"))
            {
                groundState = GroundState.onGround;
            }
        }
        else
        {
            groundState = GroundState.onAir;
        }
    }
    public bool ObstacleCheck(Vector3 MoveDirection)    // PREVENTS CLIPPING THROUGH WALLS
    {
        foreach (var direction in hitDirections)
        {
            if (Physics.Raycast(_obstacleCheck.transform.position, direction, out hitObstacle, 1.1f))
            {
                if (hitObstacle.transform.TryGetComponent<IObstacle>(out IObstacle obstacle))
                {
                    MoveDirection = direction;
                    obstacle.GetRidOf(MoveDirection, transform);
                }
            }
        }
        return true;
    }
}
