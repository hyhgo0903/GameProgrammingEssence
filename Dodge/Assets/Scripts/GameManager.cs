using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public GameObject gameoverText;
  public Text timeText;
  public Text recordText;

  private float surviveTime;
  private bool isGameover;
  
  void Start()
  {
    this.surviveTime = 0f;
    this.isGameover = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (this.isGameover)
    {
      if (Input.GetKeyDown(KeyCode.R))
      {
        SceneManager.LoadScene("SampleScene");
      }

      return;
    }

    this.surviveTime += Time.deltaTime;
    this.timeText.text = "Time: " + (int)this.surviveTime;
  }

  public void EndGame()
  {
    this.isGameover = true;
    this.gameoverText.SetActive(true);
    var bestTime = PlayerPrefs.GetFloat("BestTime");
    if (this.surviveTime > bestTime)
    {
      bestTime = this.surviveTime;
      PlayerPrefs.SetFloat("BestTime", this.surviveTime);
    }

    this.recordText.text = "Best Time: " + (int)bestTime;
  }
}
