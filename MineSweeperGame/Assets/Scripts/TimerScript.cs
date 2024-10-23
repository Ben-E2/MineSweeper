using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float TimeTaken = 0f;
    public bool TimerActive = false;

    [SerializeField] private TextMeshProUGUI TimerText;

    void Update()
    {
        if (TimerActive)
        {
            TimeTaken += Time.deltaTime;

            // Update UI
        }

        TimerText.text = TimeTaken.ToString("F3");
    }

    public void ToggleTimer(bool active)
    {
        if (active)
        {
            TimerActive = true;
        }
        else
        {
            TimerActive = false;
        }
    }

    public void ClearTimer()
    {
        TimeTaken = 0f;
    }
}
