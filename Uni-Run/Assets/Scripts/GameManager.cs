using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public bool isGameover = false;
  public Text scoreText;
  public GameObject gameoverUI;

  private int score = 0; // 게임 점수

  // 게임 시작과 동시에 싱글톤을 구성
  void Awake()
  {
    // 싱글톤 변수 instance가 비어있는가?
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
      Destroy(gameObject);
    }
  }

  void Update()
  {
    if (this.isGameover && Input.GetMouseButtonDown(0))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  public void AddScore(int newScore)
  {
    if (this.isGameover)
    {
      return;
    }

    this.score += newScore;
    this.scoreText.text = $"Score: {this.score}";
  }

  public void OnPlayerDead()
  {
    this.isGameover = true;
    this.gameoverUI.SetActive(true);
  }
}