using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurrentLevelLabel : MonoBehaviour
{
    public Text currentLevelLabel;

    public void Start( )
    {
        currentLevelLabel.text = "LEVEL " + (SceneManager.GetActiveScene( ).buildIndex - 1);
    }
}
