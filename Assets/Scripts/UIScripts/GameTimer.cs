using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float time = 0f;
    public TextMeshProUGUI timerText;

    private bool isRunning = true;

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            time += Time.deltaTime;
            displayerTimer(time);
        }
    }

    private void displayerTimer(float Timer)
    {
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void pauseTime() => isRunning = false;
    public void resumeTime() => isRunning = true;   
}