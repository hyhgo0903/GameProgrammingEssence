using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Rigidbody playerRigidbody;
  public float speed = 8f;

  void Start()
  {
    this.playerRigidbody = GetComponent<Rigidbody>();
  }
  
  void Update()
  {
    var version = 1;

    if (version == 0)
    {
      if (Input.GetKey(KeyCode.UpArrow))
      {
        this.playerRigidbody.AddForce(0f, 0f, this.speed);
      }

      if (Input.GetKey(KeyCode.DownArrow))
      {
        this.playerRigidbody.AddForce(0f, 0f, -this.speed);
      }

      if (Input.GetKey(KeyCode.LeftArrow))
      {
        this.playerRigidbody.AddForce(-this.speed, 0f, 0f);
      }

      if (Input.GetKey(KeyCode.RightArrow))
      {
        this.playerRigidbody.AddForce(this.speed, 0f, 0f);
      }
    }
    else if (version == 1)
    {
      var xInput = Input.GetAxis("Horizontal");
      var zInput = Input.GetAxis("Vertical");
      var xSpeed = xInput * this.speed;
      var zSpeed = zInput * this.speed;

      var newVelocity = new Vector3(xSpeed, 0f, zSpeed);
      this.playerRigidbody.linearVelocity = newVelocity;
    }
  }

  public void Die()
  {
    this.gameObject.SetActive(false);
    var gameManager = FindFirstObjectByType<GameManager>();
    gameManager.EndGame();
  }
}
