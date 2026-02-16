using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed = 8f;
  private Rigidbody rb;

  void Start()
  {
    this.rb = this.GetComponent<Rigidbody>();
    var random = Random.Range(0.8f, 1.2f);
    this.speed *= random;
    this.rb.linearVelocity = this.transform.forward * this.speed;

    Destroy(this.gameObject, 3f);
  }

  public void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Player")
    {
      return;
    }

    var playerController = other.GetComponent<PlayerController>();
    if (playerController != null)
    {
      playerController.Die();
    }
  }
}
