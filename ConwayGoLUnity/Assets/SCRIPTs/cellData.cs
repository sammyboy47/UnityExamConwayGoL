using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellData : MonoBehaviour
{
    public Vector2Int coordID;
    public bool isAlive;
    public dispCell disp;

    public void fncSetupCell(Vector2Int getCoord, dispCell getOutput)
    {
        this.coordID = getCoord;
        this.disp = getOutput;
    }
    public void fncSetStatus(bool willLive)
    {
        isAlive = willLive;
        disp.fncStartFade(willLive);
    }

}
