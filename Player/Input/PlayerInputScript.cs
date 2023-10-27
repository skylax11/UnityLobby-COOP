using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : NetworkBehaviour
{
    public Vector2 direction;
    public static PlayerInputScript instance;

    [SerializeField] PlayerController m_Controller;
    private Rigidbody m_Rigidbody;
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
    private void Start() =>   m_Rigidbody = GetComponent<Rigidbody>();
    public void OnMove() => m_Controller.PlayerState = PlayerController.State.Walking;
    public void OnSprint() => m_Controller.PlayerState = PlayerController.State.Sprinting;


    private void Update()
    {
       if (!IsOwner)
       {
            return;
       }
        InputDetections();
    }
    public void OnJump()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 10, 0),Time.deltaTime*10f);
        m_Controller.PlayerState = PlayerController.State.Jumping;
    }
    public void InputDetections()
    {
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) { direction.y = 1f;  OnMove(); }
        if (Input.GetKey(KeyCode.S)) { direction.y = -1f; OnMove(); }
        if (Input.GetKey(KeyCode.A)) { direction.x = -1f; OnMove(); }
        if (Input.GetKey(KeyCode.D)) { direction.x = 1f;  OnMove(); }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) 
        { m_Controller.PlayerState = PlayerController.State.Standing; }
        if(Input.GetKey(KeyCode.LeftShift)) m_Controller.PlayerState = PlayerController.State.Sprinting;
        if(Input.GetKeyDown(KeyCode.Space) && (m_Controller.groundState == PlayerController.GroundState.onGround)) OnJump();
    }
}
