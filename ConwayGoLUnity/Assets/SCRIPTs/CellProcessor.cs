using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProcessor : MonoBehaviour
{
    public Dictionary<Vector2Int, bool> toCheck = new Dictionary<Vector2Int, bool>();

    public bool isProcess = false;
    IEnumerator ProcesssCellOp, InitiateProcessOp;

    float simCycleTime { get { return GridController.main.lastCycleTime; } set { GridController.main.lastCycleTime = value; } }

    private void FixedUpdate()
    {
        if (GridController.main != null)// { print("GridControllernot yet active"); }
            if (GridController.main.isPlay)
            {

                if (Time.time > (simCycleTime) && !isProcess)// && (GridController.main.listAlive.Count >= 1))
                {
                    fncCheckCells();
                    simCycleTime = simCycleTime + GridController.main.getCycleTime;
                }
            }
    }
    public void fncCheckCells()
    {
        if (isProcess) StopCoroutine(InitiateProcessOp);
        InitiateProcessOp = opInitiateProcessCell();
        StartCoroutine(InitiateProcessOp);
    }

    IEnumerator opInitiateProcessCell()
    {
        toCheck.Clear();// = new SortedDictionary<Vector2Int, bool>();
        yield return new WaitUntil(() => !isProcess);
        // StopCoroutine(ProcesssCellOp);
        ProcesssCellOp = opProcessCells();
        StartCoroutine(ProcesssCellOp);
    }
    IEnumerator opProcessCells()
    {
        print("Initiating process at " + Time.time.ToString("00.00"));
        isProcess = true;
        //  Vector2Int thisCoord;
        List<Vector2Int> cellNeighbours = new List<Vector2Int>(), toRemove = new List<Vector2Int>(), toAdd = new List<Vector2Int>();
        int liveCntr = 0; //Counts how many live neighbours
        int batchCntr = 0, batchLimit = (int)(30 / Time.unscaledDeltaTime);
        //neighbourCount<2 or >3 kill cell

        //3 neighbouurs,activate cell
        foreach (KeyValuePair<Vector2Int, bool> thisCell in GridController.main.listAlive)
        {
            liveCntr = 0;//resets counter
            cellNeighbours = fncGetNeighbours(thisCell.Key);
            foreach (Vector2Int neighbourCoord in cellNeighbours)
            {
                if (!GridController.main.listAlive.ContainsKey(neighbourCoord))
                {
                    if (!toCheck.ContainsKey(neighbourCoord))
                    {
                        toCheck.Add(neighbourCoord, true);
                    }
                }
                else liveCntr++;
                batchCntr++;
            }
            bool willLive = ((liveCntr >= 2) && (liveCntr <= 3));

            if (!willLive)//Determines if it has too few or too much neighbours.
                toRemove.Add(thisCell.Key);

            #region 
            // if (batchCntr < batchLimit)
            // {
            //     batchCntr++;
            //     yield return new WaitForFixedUpdate();
            // }
            // else
            // {
            //     batchCntr = 0;
            //     batchLimit = (int)(50 / Time.unscaledDeltaTime);
            //     yield return null;
            // }
            #endregion
        }
        foreach (Vector2Int thisID in toRemove)
        {
            fncSetCellStatus(false, thisID);
        }
        //
        yield return null;
        //
        foreach (KeyValuePair<Vector2Int, bool> checkCells in toCheck)
        {
            liveCntr = 0;
            foreach (Vector2Int chkNeighbour in fncGetNeighbours(checkCells.Key))
            {
                if (GridController.main.listAlive.ContainsKey(chkNeighbour))
                    liveCntr++;
            }
            if (liveCntr == 3)
                toAdd.Add(checkCells.Key);
        }
        foreach (Vector2Int thisID in toAdd)
        {
            fncSetCellStatus(true, thisID);
        }
        isProcess = false;
    }

    List<Vector2Int> fncGetNeighbours(Vector2Int getCoord)
    {
        List<Vector2Int> getNeighbours = new List<Vector2Int>();
        Vector2Int thisCoord;
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                thisCoord = getCoord + new Vector2Int(x, y);
                if (!isInGrid(thisCoord)) continue;
                //
                if (GridController.main.gridCells.ContainsKey(thisCoord)) getNeighbours.Add(thisCoord);
                else
                if (toCheck.ContainsKey(thisCoord))
                    toCheck.Add(thisCoord, false);
            }
        return getNeighbours;
    }
    bool isInGrid(Vector2Int thisCoord)
    {
        return (thisCoord.x >= 0 && thisCoord.x <= GridController.main.gridSize)
        && (thisCoord.y >= 0 && thisCoord.y <= GridController.main.gridSize);
    }
    void fncSetCellStatus(bool isActivate, Vector2Int getID)
    {
        GridController.main.gridCells[getID] = isActivate;
        if (isActivate && !GridController.main.listAlive.ContainsKey(getID))
        {
            GridController.main.listAlive.Add(getID, true);
            print($"Cell {getID} went alive");
        }
        else
        {
            GridController.main.listAlive.Remove(getID);
            print($"Cell {getID} died");
        }
        GridController.main.dispGrid[getID].fncStartFade(isActivate);
    }

    public void fncGetPlayerClick(Vector3 getMousePos)
    {
        print("Player clicked");
        Vector2Int getPos = new Vector2Int((int)getMousePos.x, (int)getMousePos.y);
        if (isInGrid(getPos))
        {
            bool getState = GridController.main.gridCells[getPos];
            fncSetCellStatus(!getState, getPos);
        }
    }

    /*
        // PREPARE CELLLS STARTS HERE //
    */
    public void fncPrepareCells()
    {
        StartCoroutine(opPrepareCells());
    }
    public void fncReArrangeCells() => StartCoroutine(opArrangeCells());
    public void fncRecolourCell() => StartCoroutine(opCellRecolour());
    IEnumerator opPrepareCells()
    {
        // GridController.main.fncRecolourBackground();
        GridController.main.gridCells = new Dictionary<Vector2Int, bool>();
        GridController.main.listAlive = new Dictionary<Vector2Int, bool>();
        GridController.main.dispGrid = new Dictionary<Vector2Int, dispCell>();
        int breakCntr = 0, breakCount = (int)(100 / Time.unscaledDeltaTime);
        // CellSys newCell;
        dispCell newDispCell;
        for (int y = 0; y < 60; y++)
            for (int x = 0; x < 60; x++)
            {
                //// newCell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity) as CellSys;
                // newCell.name = $"Cell_{x.ToString("00")}-{y.ToString("00")}";
                // newCell.fncSetupCell(new Vector2Int(x, y));
                // cellDir.Add(new Vector2Int(x, y), newCell);
                // newCell.transform.position = new Vector3(.5f + x, .5f + y, 0);
                // newCell.transform.SetParent(transform);
                // newCell.fncActivateCell(false);
                //
                Vector2Int newCoord = new Vector2Int(x, y);
                GridController.main.gridCells.Add(newCoord, false);
                newDispCell = Instantiate(GridController.main.dispCellPrefab, Vector3.zero, Quaternion.identity) as dispCell;
                newDispCell.name = $"Cell_{x.ToString("00")}-{y.ToString("00")}";
                GridController.main.dispGrid.Add(new Vector2Int(x, y), newDispCell);
                newDispCell.transform.position = new Vector3(.5f + x, .5f + y, 0);
                newDispCell.transform.SetParent(transform);
                //
                if (breakCntr >= breakCount)
                {
                    yield return null;
                    breakCntr = 0;
                    breakCount = (int)(100 / Time.unscaledDeltaTime);
                }
                else breakCntr++;
            }
        fncReArrangeCells();
        // StartCoroutine(opArrangeCells());
        // yield return new WaitForSeconds(2);
        // StartCoroutine(tempRando());
    }

    // bool isArranging = false;
    IEnumerator opArrangeCells()
    {
        GridController.main.fncRecolourBackground();
        fncRecolourCell();
        StartCoroutine(opAdjustCam());
        GridController.main.isPlay = false;
        int breakCntr = 0, breakCount = (int)(100 / Time.unscaledDeltaTime);
        Vector2Int curCoord = Vector2Int.zero;
        int gridSize = GridController.main.gridSize;
        bool withinGrid = false;
        for (int y = 0; y < 60; y++)
        {
            for (int x = 0; x < 60; x++)
            {
                curCoord = new Vector2Int(x, y);
                withinGrid = isInGrid(curCoord);
                GridController.main.dispGrid[curCoord].transform.position = new Vector3(.5f + x, .5f + y, 0);
                GridController.main.dispGrid[curCoord].gameObject.SetActive(true);
                GridController.main.dispGrid[curCoord].fncActivatedGridCell(withinGrid);

                if (breakCntr >= breakCount)
                {
                    yield return null;
                    breakCntr = 0;
                    breakCount = (int)(100 / Time.unscaledDeltaTime);
                }
                else breakCntr++;
            }
        }
        GridController.main.isPlay = true;
        StartCoroutine(opCellRecolour());
    }

    IEnumerator opAdjustCam()
    {
        Camera thisCam = Camera.main;
        int gridSize = GridController.main.gridSize;
        float blockSize = GridController.main.dispCellPrefab.activeCell.sprite.rect.width;
        Vector3 newCamPos = new Vector3(((gridSize / 2) * blockSize) + .5f, ((gridSize / 2) * blockSize) + .5f, -10), oldCamPos = thisCam.transform.position, smoothDamp = Vector3.zero;
        float newCamSize = (((Screen.currentResolution.width / Screen.currentResolution.height) * (gridSize * blockSize)) / 2) + .5f, oldCamSize = thisCam.orthographicSize;
        float resizeDamp = 0, t = 0, tCur = 0, tLim = GridController.main.getCycleTime * .5f;

        // thisCam.transform.position = new Vector3(gridSize/2, gridSize/2,-10);
        // thisCam.orthographicSize = ((Screen.currentResolution.width/Screen.currentResolution.height)*gridSize)/2;
        yield return null;

        newCamSize = (gridSize / 2) + .5f;
        newCamPos = new Vector3(newCamSize, newCamSize, -10);
        // while ((thisCam.transform.position != newCamPos) && (thisCam.orthographicSize != newCamSize))
        tLim = tLim <= .5f ? .5f : tLim;
        while (t <= tLim)
        {
            // thisCam.transform.position = Vector3.SmoothDamp(thisCam.transform.position, newCamPos, ref smoothDamp, GridController.main.getCycleTime, 40);
            // thisCam.orthographicSize = Mathf.SmoothDamp(thisCam.orthographicSize, newCamSize, ref resizeDamp, GridController.main.getCycleTime, 40);
            thisCam.transform.position = Vector3.Lerp(oldCamPos, newCamPos, tCur);
            thisCam.orthographicSize = Mathf.Lerp(oldCamSize, newCamSize, tCur);
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            tCur = t * t;
            if ((thisCam.transform.position).magnitude < .05) thisCam.transform.position = newCamPos;
            if (Mathf.Abs(thisCam.orthographicSize - newCamSize) < .05f) thisCam.orthographicSize = newCamSize;
        }
    }

    IEnumerator tempRando()
    {
        int gridSize = GridController.main.gridSize;
        int randAmount = Random.Range(gridSize, gridSize * 3), randX, randY;
        Vector2Int randCoord = Vector2Int.zero;
        for (int i = 0; i < randAmount; i++)
        {
            randX = Random.Range(0, gridSize - 1);
            randY = Random.Range(0, gridSize - 1);
            randCoord.x = randX;
            randCoord.y = randY;
            //
            fncSetCellStatus(true, randCoord);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator opCellRecolour()
    {
        yield return new WaitUntil(()=>GridController.main.isPlay);
        print("Recolouring Cells");
        // GridController.main.isPlay = false;
        // List<Vector2Int> activeList = new List<Vector2Int>(), cellList = new List<Vector2Int>();
        Vector2Int thisCoord = Vector2Int.zero;
        bool sweepOrient = Random.Range(1, 100) % 2 == 0;
        for (int x = 0; x < GridController.main.gridSize; x++)
        {
            for (int y = 0; y < GridController.main.gridSize; y++)
            {
                thisCoord = sweepOrient ? new Vector2Int(x, y) : new Vector2Int(y, x);
                GridController.main.dispGrid[thisCoord].fncStartRecolour();
            }
            yield return new WaitForFixedUpdate();// WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        // GridController.main.isPlay = true;
    }
}