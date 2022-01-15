using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSys : MonoBehaviour
{
    public Vector2Int cellID;
    public SpriteRenderer activeCell, cellGlow, spriteBorder;
    public bool inPlay, isFade;
    public AnimationCurve fadeCell, fadeGlow;

    public IEnumerator FadeOp, RecolourOp;

    public void fncSetupCell(Vector2Int getCoord) => cellID = getCoord;
    public void fncRecolour(Color tgtCol) {
         StopCoroutine(RecolourOp);
        RecolourOp = opRecolour(tgtCol);
        StartCoroutine(RecolourOp);
    }
    public void fncActivateCell(bool isAlive)
    {
        if (this.isActiveAndEnabled)
        {
            if (isFade) StopCoroutine(FadeOp);
            if (isAlive)
                FadeOp = opFadeIn(GridController.main.getFadeTime);
            else
                FadeOp = opFadeOut(GridController.main.getFadeTime);

            StartCoroutine(FadeOp);
        }
        else
        {
            activeCell.color = GridController.main.CellCol * new Color(1, 1, 1, isAlive ? 1 : 0);
            cellGlow.color = activeCell.color;
        }
        if(isAlive) GridController.main.activeCells.Add(cellID, this);
        else GridController.main.activeCells.Remove(cellID);
    }

    public IEnumerator opChangeGlowSize(float getSize)
    {
        float timer = 0, prevSize = cellGlow.gameObject.transform.localScale.x, curProgress = 0;
        //
        while (timer <= 1)
        {
            curProgress = Mathf.Lerp(prevSize, getSize, timer);
            cellGlow.gameObject.transform.localScale = new Vector3(curProgress, curProgress, curProgress);
            yield return null;
            timer += Time.deltaTime;
        }
    }
    public IEnumerator opRecolour(Color tgtColor){
        float t = 0;
        Color oldCol = activeCell.color, thisCol = oldCol;
        while (t <= 1)
        {
            thisCol = Color.Lerp(oldCol, tgtColor, t);
            activeCell.color = thisCol;
            cellGlow.color = thisCol;
            yield return null;
            t += Time.deltaTime;
        }
    }

    public IEnumerator opFadeIn(float getTime)
    {
        isFade = true;
        float tCntr = 0, t = 0;
        while (tCntr <= getTime)
        {
            t = Mathf.InverseLerp(0, getTime, tCntr);
            activeCell.color = new Color(activeCell.color.r, activeCell.color.g, activeCell.color.b, fadeCell.Evaluate(t));
            cellGlow.color = activeCell.color;
            yield return new WaitForFixedUpdate();
            tCntr += Time.deltaTime;
        }
        isFade = false;
    }
    public IEnumerator opFadeOut(float getTime)
    {
        isFade = true;
        float tCntr = 1, t = 0;
        while (tCntr <= getTime)
        {
            t = Mathf.InverseLerp(0, getTime, tCntr);
            activeCell.color = new Color(activeCell.color.r, activeCell.color.g, activeCell.color.b, fadeCell.Evaluate(t));
            cellGlow.color = activeCell.color;
            yield return new WaitForFixedUpdate();
            tCntr -= Time.deltaTime;
        }
        isFade = false;
    }
}