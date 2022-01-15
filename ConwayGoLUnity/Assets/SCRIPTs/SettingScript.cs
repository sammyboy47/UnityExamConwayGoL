using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
    public Slider timeSlider, sizeSlider;
    public void Start()
    {
        fncChangeBGColor(Random.Range(0, 3));
        fncChangeGridColor(Random.Range(0, 7));
        timeSlider.value = 2 - GridController.main.getCycleTime;
        sizeSlider.value = GridController.main.gridSize;
        // fncChangeCycleTime(.1f);
    }
    public Color[] cellColour, colBG, colGrid;
    public void fncChangeBGColor(int i ){
        GridController.main.BGCol = colBG[i];
        GridController.main.GridCol = colGrid[i];
        GridController.main.fncRecolourBackground();
        GridController.main.gridProcessor.fncRecolourCell();
    }
    public void fncChangeGridColor(int i){
        GridController.main.CellCol = cellColour[i];
        GridController.main.gridProcessor.fncRecolourCell();
    }
    
    public void fncChangeCycleTime(UnityEngine.UI.Slider thisSlider) => GridController.main.fncChanceCycleTime(thisSlider.value);
    public void fncResize(UnityEngine.UI.Slider thisSlider)
    {
        GridController.main.gridSize = (int)thisSlider.value;
        GridController.main.gridProcessor.fncReArrangeCells();
    }

    // public void fncDisp
}
