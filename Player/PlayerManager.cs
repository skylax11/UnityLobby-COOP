using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] NetworkVariable<Vector3> _startPoint;
    private void Start()
    {
        if(IsClient)
        {
            transform.position = _startPoint.Value;                               
        }
        if (IsOwner)
        {
            _startPoint.Value = new Vector3(426.18f, 1, 367.92f);
        }
        transform.position = _startPoint.Value;
    }
}
