using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLevelText : MonoBehaviour
{
    public TextMesh levelText;
    private void OnEnable()
    {
        levelText.text = StageManager.Instance.currentStage.ToString();
    }
}
