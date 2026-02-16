using UnityEngine;

public class Jumper : MonoBehaviour
{
  public Rigidbody rb;

  private void Start()
  {
    rb.AddForce(0, 500f, 0);
  }
}
