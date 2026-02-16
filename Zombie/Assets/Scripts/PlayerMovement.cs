using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f; // 앞뒤 움직임의 속도
  public float rotateSpeed = 180f; // 좌우 회전 속도


  private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
  private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
  private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

  private void Start()
  {
    // 사용할 컴포넌트들의 참조를 가져오기
    this.playerAnimator = GetComponent<Animator>();
    this.playerRigidbody = GetComponent<Rigidbody>();
    this.playerInput = GetComponent<PlayerInput>();
  }

  // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
  // 물리 주기에 맞춰 실행되므로 이동 회전은 여기서 실행하는 게 오차가 적다고 한다
  private void FixedUpdate()
  {
    // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
    this.Rotate();
    this.Move();
    this.playerAnimator.SetFloat("Move", this.playerInput.move);
  }

  // 입력값에 따라 캐릭터를 앞뒤로 움직임
  private void Move()
  {
    // Time.fixedDeltaTime을 사용하는 것이 맞지만 FixedUpdate에서 deltaTime으로 접근하면 자동으로 fixedDeltaTime이 반환된다
    // (디버그로 찍어볼 것)
    var moveDistance = this.playerInput.move * this.moveSpeed * Time.fixedDeltaTime;
    moveDistance = this.playerInput.move * this.moveSpeed * Time.deltaTime;

    this.playerRigidbody.MovePosition(this.playerRigidbody.position + this.transform.forward * moveDistance);
    
    // 이렇게하면 물리 처리를 무시, 즉 벽뚫이 가능 (안 한 이유)
    //transform.position += transform.forward * moveDistance;
  }

  // 입력값에 따라 캐릭터를 좌우로 회전
  private void Rotate()
  {
    var turn = this.playerInput.rotate * this.rotateSpeed * Time.fixedDeltaTime;
    this.playerRigidbody.rotation = this.playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);
  }
}