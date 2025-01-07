using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeSpeed;
    [Range(0, 23)] public int hours;

    [SerializeField] TMP_Text gameTime_Text;

    private void Start()
    {
        InvokeRepeating("IncrementTime", 0, timeSpeed);
    }

    void IncrementTime()
    {
        int tempHours = hours;
        tempHours++;
        if (tempHours >= 24)
        {
            tempHours = 0;
        }
        hours = tempHours;
    }

    private void Update()
    {
        gameTime_Text.SetText(hours.ToString());
    }
}
