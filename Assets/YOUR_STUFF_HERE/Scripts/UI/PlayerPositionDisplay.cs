using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPositionDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] positionDisplays;

    public void UpdateDisplays(int[] playerPositions)
    {
        positionDisplays[playerPositions[0]].text = "1st";
        positionDisplays[playerPositions[1]].text = "2nd";
        positionDisplays[playerPositions[2]].text = "3rd";
        positionDisplays[playerPositions[3]].text = "4th"; 
    }
} 
