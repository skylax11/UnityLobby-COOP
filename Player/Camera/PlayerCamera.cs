using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    public float SensX;
    public float SensY;

    public Transform Orientation;

    public float xRot;
    public float yRot;

    [SerializeField] Transform Player;
    public Camera cam;
    Vector3 position;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        position = new Vector3(Player.position.x, Player.position.y + 0.5f, Player.position.z);
        transform.position = position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensY;

        yRot += mouseX;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        Orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
