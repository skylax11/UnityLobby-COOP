using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectorItem : NetworkBehaviour,IConnector
{
    [SerializeField] int[] gameObjectIndexes;
    [SerializeField] ListOfObjects listOfObjects;
    public bool letPlayerUnlock;

    public List<GameObject> spawnedObjects;
    public void SetPuzzle(bool enabled, int[] index) => SetGameobjectServerRpc(index, enabled);
    public void SetPuzzleByPlayer(bool enabled)
    {
        if (letPlayerUnlock)
        {
            SetGameobjectServerRpc(gameObjectIndexes, enabled);
        }
    }
   [ServerRpc(RequireOwnership = false)]
    public void SetGameobjectServerRpc(int[] index, bool enabled)    // WE NEED INDEXES FOR USING ServerRpc, So we decleare a list from inspector...
    {
        List<GameObject> temporaryList = new List<GameObject>();
        for (int i = index[0]; i < index.Length + index[0]; i++)
        {
            temporaryList.Add(listOfObjects.objects[i]);
        }
        foreach (GameObject theObject in temporaryList)
        {
            if (enabled)
            {
                GameObject spawnedObject = Instantiate(theObject);
                spawnedObjects.Add(spawnedObject);
                spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            }
            else
            {
                foreach (GameObject spawnedObj in spawnedObjects)
                {
                    print(spawnedObj.name);
                    spawnedObj.GetComponent<NetworkObject>().Despawn();
                }
                spawnedObjects.Clear();
            }
        }
    }

}
