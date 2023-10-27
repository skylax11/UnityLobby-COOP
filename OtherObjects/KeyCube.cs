using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCube : MonoBehaviour
{
    [SerializeField] int[] indexes;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<ConnectorItem>(out ConnectorItem connector))
        {
            connector.SetPuzzle(true,indexes);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        print(collision.transform.name);
        if (collision.transform.TryGetComponent<ConnectorItem>(out ConnectorItem connector))
        {
            connector.SetPuzzle(false,indexes);
        }
    }
}
