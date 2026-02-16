using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{
  public AudioClip deathClip; // 사망시 재생할 오디오 클립
  public float jumpForce = 700f; // 점프 힘

  private int jumpCount = 0; // 누적 점프 횟수
  private bool isGrounded = false; // 바닥에 닿았는지 나타냄
  private bool isDead = false; // 사망 상태

  private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
  private Animator animator; // 사용할 애니메이터 컴포넌트
  private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

  private void Start()
  {
    this.playerRigidbody = GetComponent<Rigidbody2D>();
    this.animator = GetComponent<Animator>();
    this.playerAudio = GetComponent<AudioSource>();
  }

  private void Update()
  {
    if (this.isDead)
    {
      return;
    }

    // 0: 왼쪽, 1: 오른쪽, 2: 휠 버튼
    // GetMouseButtonDown: 마우스 버튼을 누르는 순간 true 반환
    // GetMouseButtonUp: 마우스 버튼에서 손을 떼는 순간 true 반환
    // GetMouseButton: 마우스 버튼을 누르고 있는 동안 true 반환
    // (GetKey와 형태 동일)
    if (Input.GetMouseButtonDown(0) && this.jumpCount < 2)
    {
      ++this.jumpCount;
      this.playerRigidbody.linearVelocity = Vector2.zero;
      this.playerRigidbody.AddForce(new Vector2(0, this.jumpForce));
      this.playerAudio.Play();
    }
    else if (Input.GetMouseButtonUp(0) && this.playerRigidbody.linearVelocity.y > 0)
    {
      this.playerRigidbody.linearVelocity *= 0.5f;
    }

    this.animator.SetBool("Grounded", this.isGrounded);
  }

  private void Die()
  {
    this.animator.SetTrigger("Die");
    this.playerAudio.clip = this.deathClip;
    this.playerAudio.Play();
    this.playerRigidbody.linearVelocity = Vector2.zero;
    this.isDead = true;

    GameManager.instance.OnPlayerDead();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Dead" && !this.isDead)
    {
      Die();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    // 어떤 콜라이더와 닿았으며 충돌 표면이 위쪽을 보고 있으면
    // collision.contracts는 여러 충돌 정보를 배열로 담음
    // ContractPoint.normal은 충돌 표면의 법선 벡터
    // 0.7f는 약 45도 각도. 위쪽이며 경사가 너무 급하지 않을 때만 멈추겠다
    if (collision.contacts[0].normal.y > 0.7f)
    {
      this.isGrounded = true;
      this.jumpCount = 0;
    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    this.isGrounded = false;
  }
}