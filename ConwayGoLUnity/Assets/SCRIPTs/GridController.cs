using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController main;
    public CellProcessor gridProcessor;
    // public SpriteRenderer spriteBackground;

    public Camera thisCam;

    public bool isPlay=false;
    [Range(20, 60)] public int gridSize;
    public Color CellCol, BGCol, GridCol;
    [SerializeField] [Range(.1f,.8f)]float fadeSpeed;
    [SerializeField] [Range(.1f, 2.5f)] float cycleSpeed;

    public float getCycleTime { get { return 3 - cycleSpeed; } }
    public float getFadeTime { get { return (1 - fadeSpeed) * getCycleTime; } }

    public float lastCycleTime = 0;

    public CellSys cellPrefab;
    public dispCell dispCellPrefab;
    public Dictionary<Vector2Int, CellSys> cellDir, activeCells;
    public Dictionary<Vector2Int, bool> gridCells, listAlive;
    public Dictionary<Vector2Int, dispCell> dispGrid;
    //
    private void Awake()
    {
        if (main != null) Destroy(this);
        else main = this;
    }
    public void Start()
    {
        isPlay = false;
        gridProcessor.fncPrepareCells();
        thisCam = Camera.main;
        // StartCoroutine(opPrepareCells());
    }
    public void fncPlayerClick()
    {
        gridProcessor.fncGetPlayerClick(thisCam.ScreenToWorldPoint(Input.mousePosition));
    }
    public void fncRecolourBackground() => thisCam.backgroundColor = BGCol;
    public void fncChanceCycleTime(float getTime) => cycleSpeed = Mathf.Clamp(getTime, .1f, 2.5f);
    //
    // IEnumerator opPrepareCells()
    // {
    //     cellDir = new Dictionary<Vector2Int, CellSys>();
    //     activeCells = new Dictionary<Vector2Int, CellSys>();
    //     int breakCntr = 0, breakCount = (int)(100 / Time.unscaledDeltaTime);
    //     // CellSys newCell;
    //     dispCell newDispCell = new dispCell();
    //     for (int y = 0; y < 60; y++)
    //         for (int x = 0; x < 60; x++)
    //         {
    //             //// newCell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity) as CellSys;
    //             // newCell.name = $"Cell_{x.ToString("00")}-{y.ToString("00")}";
    //             // newCell.fncSetupCell(new Vector2Int(x, y));
    //             // cellDir.Add(new Vector2Int(x, y), newCell);
    //             // newCell.transform.position = new Vector3(.5f + x, .5f + y, 0);
    //             // newCell.transform.SetParent(transform);
    //             // newCell.fncActivateCell(false);
    //             //
    //             Vector2Int newCoord = new Vector2Int(x, y);
    //             gridCells.Add(newCoord, false);
    //             newDispCell = Instantiate(dispCellPrefab, Vector3.zero, Quaternion.identity) as dispCell;
    //             newDispCell.name = $"Cell_{x.ToString("00")}-{y.ToString("00")}";
    //             dispGrid.Add(new Vector2Int(x, y), newDispCell);
    //             newDispCell.transform.position = new Vector3(.5f + x, .5f + y, 0);
    //             newDispCell.transform.SetParent(transform);
    //             //
    //             if (breakCntr >= breakCount)
    //             {
    //                 yield return null;
    //                 breakCntr = 0;
    //                 breakCount = (int)(100 / Time.unscaledDeltaTime);
    //             }
    //             else breakCntr++;
    //         }

    //     StartCoroutine(opArrangeCells());
    //     StartCoroutine(opAdjustCam());
    //     yield return new WaitForSeconds(2);
    //     StartCoroutine(tempRando());
    // }

    // IEnumerator opArrangeCells()
    // {
    //     isPlay = false;
    //     int breakCntr = 0, breakCount = (int)(100 / Time.unscaledDeltaTime);
    //     Vector2Int curCoord = Vector2Int.zero;
    //     for (int y = 0; y < 60; y++)
    //     {
    //         for (int x = 0; x < 60; x++)
    //         {
    //             curCoord = new Vector2Int(x, y);
    //             // if (x >= gridSize || y >= gridSize)
    //             // {
    //             //     cellDir[curCoord].gameObject.SetActive(false);
    //             // }
    //             // else
    //             // {
    //             cellDir[curCoord].gameObject.SetActive((x < gridSize) && (y < gridSize));
    //             cellDir[curCoord].transform.position = new Vector3(.5f + x, .5f + y, 0);
    //             // }
    //             //
    //             if (breakCntr >= breakCount)
    //             {
    //                 yield return null;
    //                 breakCntr = 0;
    //                 breakCount = (int)(100 / Time.unscaledDeltaTime);
    //             }
    //             else breakCntr++;
    //         }
    //     }
    // }

    // IEnumerator opAdjustCam()
    // {
    //     Camera thisCam = Camera.main;
    //     Vector3 newCamPos = new Vector3(gridSize / 2, gridSize / 2, -10), smoothDamp = Vector3.zero;
    //     float newCamSize = gridSize/2;
    //     float resizeDamp=0;

    //     while ((thisCam.transform.position != newCamPos) && (thisCam.orthographicSize != newCamSize))
    //     {
    //         thisCam.transform.position = Vector3.SmoothDamp(thisCam.transform.position, newCamPos, ref smoothDamp, getCycleTime, 20);
    //         thisCam.orthographicSize = Mathf.SmoothDamp(thisCam.orthographicSize, newCamSize, ref resizeDamp, getCycleTime, 20);
    //         yield return new WaitForFixedUpdate();
    //         if((thisCam.transform.position).magnitude<.05)thisCam.transform.position = newCamPos;
    //         if(Mathf.Abs(thisCam.orthographicSize-newCamSize)<.05f)thisCam.orthographicSize = newCamSize;
    //     }
    // }

    // IEnumerator tempRando()
    // {
    //     int randAmount = Random.Range(gridSize, gridSize*2 ), randX, randY;
    //     Vector2Int randCoord=Vector2Int.zero;
    //     for (int i = 0; i < randAmount; i++)
    //     {
    //         randX = Random.Range(0, gridSize - 1);
    //         randY = Random.Range(0, gridSize - 1);
    //         randCoord.x = randX;
    //         randCoord.y = randY;
    //         //
    //         cellDir[randCoord].fncActivateCell(true);
    //         yield return new WaitForFixedUpdate();
    //     }
    //     isPlay = true;
    // }
    // IEnumerator opCellRecolour()
    // {
    //     // List<Vector2Int> activeList = new List<Vector2Int>(), cellList = new List<Vector2Int>();
    //     Vector2Int thisCoord = Vector2Int.zero;
    //     bool sweepOrient = Random.Range(-100, 100) % 2 == 0;
    //     for (int x = 0; x < 60; x++)
    //     {
    //         for (int y = 0; y < 60; y++)
    //         {
    //             thisCoord = sweepOrient ? new Vector2Int(x, y) : new Vector2Int(y, x);
    //             cellDir[thisCoord].fncRecolour(CellCol);
    //         }
    //         yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
    //     }
    // }
}