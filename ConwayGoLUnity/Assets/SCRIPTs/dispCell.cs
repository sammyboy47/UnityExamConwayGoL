using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dispCell : MonoBehaviour
{
    public SpriteRenderer activeCell, cellGlow, spriteBorder;
    // public bool inPlay, isFade;
    public AnimationCurve fadeCell, fadeGlow;
    float tCur = 0, tTime = 0, fadeTime;
    bool isRecolor = false, isFading = false;

    IEnumerator fncFade, fncRecolour;

    public void fncStartFade(bool isActivate)
    {
        fadeTime = GridController.main.getFadeTime;
        if (isFading) StopCoroutine(fncFade);
        fncFade = opFade(isActivate);
        if (gameObject.activeSelf)
            StartCoroutine(fncFade);
    }
    public void fncStartRecolour()
    {
        if (isRecolor) StopCoroutine(fncRecolour);
        fncRecolour = opRecolor();
        if (gameObject.activeSelf)
            StartCoroutine(fncRecolour);
    }
    public void fncActivatedGridCell(bool isActivate)
    {
        StartCoroutine(opActivateGridCell(isActivate));
    }

    IEnumerator opFade(bool isActivate)
    {
        isFading = true;
        tTime = 0;
        tCur = 0;
        while (tTime <= fadeTime)
        {
            if (isActivate)
                tCur = Mathf.InverseLerp(0, fadeTime, tTime);
            else
                tCur = Mathf.InverseLerp(fadeTime, 0, tTime);
            activeCell.color = new Color(activeCell.color.r, activeCell.color.g, activeCell.color.b, fadeCell.Evaluate(tCur));
            cellGlow.color = new Color(cellGlow.color.r, cellGlow.color.g, cellGlow.color.b, fadeGlow.Evaluate(tCur));
            yield return null;
            tTime += Time.deltaTime;
        }
        isFading = false;
    }
    IEnumerator opRecolor()
    {
        isRecolor = true;
        Color newColor = GridController.main.CellCol, oldColor = activeCell.color, oldGrid = spriteBorder.color, newGrid = GridController.main.GridCol;
        float t = 0, tLim = GridController.main.getCycleTime * .5f, tCnt = 0;

        // tLim = Mathf.Clamp(.1f, .5f, tLim);
        // while (t <= tLim)
        // {
        //     tCnt = Mathf.Lerp(0, tLim, t);
        //     // if(isFading)
        //     newColor = new Color(newColor.r, newColor.g, newColor.b, activeCell.color.a);
        //     activeCell.color = Color.Lerp(oldColor, newColor, tCnt);//Mathf.Lerp(0, tLim, t));
        //     cellGlow.color = activeCell.color;
        //     spriteBorder.color = Color.Lerp(oldGrid, newGrid, tCnt);
        //     yield return null;
        //     t += Time.deltaTime;
        // }
        // if (activeCell.color == newColor && cellGlow.color == newColor && spriteBorder.color == newGrid)
        // {
        activeCell.color = new Color(newColor.r, newColor.g, newColor.b, activeCell.color.a);
        cellGlow.color = new Color(newColor.r, newColor.g, newColor.b, cellGlow.color.a);
        spriteBorder.color = newGrid;
        // }
        isRecolor = false;
        yield return null;
    }
    IEnumerator opActivateGridCell(bool isActivate)
    {
        tTime = 0;
        tCur = 0;
        Color oldColor = activeCell.color, gridCol = GridController.main.GridCol,
        newColor = new Color(GridController.main.CellCol.r, GridController.main.CellCol.g, GridController.main.CellCol.b, 0), curCol = newColor;
        activeCell.color = newColor;
        cellGlow.color = newColor;
        spriteBorder.color = gridCol;
        // if (isActivate)
        // fncStartFade(GridController)
        // while (tTime <= fadeTime)
        // {

        //     tCur = Mathf.InverseLerp(0, fadeTime, tTime);
        //     tCur = tCur * tCur;

        //     curCol = Color.Lerp(oldColor, newColor, tCur);
        //     activeCell.color = curCol;//new Color(activeCell.color.r, activeCell.color.g, activeCell.color.b, fadeCell.Evaluate(tCur));
        //     cellGlow.color = curCol;
        //     spriteBorder.color = Color.Lerp(new Color(gridCol.r, gridCol.g, gridCol.b, 0), gridCol, isActivate ? tCur : (1 - tCur));
        //     yield return null;
        //     tTime += Time.unscaledDeltaTime;
        // }
        yield return null;
        gameObject.SetActive(isActivate);
    }
}