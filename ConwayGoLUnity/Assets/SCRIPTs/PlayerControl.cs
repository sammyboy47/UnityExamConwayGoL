using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool isMouseGUI;

    public void fncSetMouseGUI(bool isOnGUI) => isMouseGUI = isOnGUI;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMouseGUI)
            GridController.main.fncPlayerClick();
    }
}
