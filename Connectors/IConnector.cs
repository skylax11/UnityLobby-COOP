using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnector
{
    void SetPuzzle(bool enabled, int[] index);
    public void SetPuzzleByPlayer(bool enabled);

}
