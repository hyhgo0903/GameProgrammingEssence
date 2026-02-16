using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
  public Gun gun; // 사용할 총

  // IK 갱신에 사용할 변수들
  public Transform gunPivot; // 총 배치의 기준점
  public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
  public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

  private PlayerInput playerInput; // 플레이어의 입력

  // FK : Forward Kinematics. 부모 조인트 -> 자식 조인트(종속) 순으로 움직임 적용. 누적된 움직임으로 결정
  // 만들어진 애니메이션에 맞춰 물건의 위치를 손으로 옮겨야 함.

  // IK : Inverse Kinematics. 자식 조인트 위치 결정 -> 부모 조인트가 그에 맞춰 변형
  // IK는 하위 조인트의 최종 위치를 먼저 결정할 수 있음 -> 물건의 위치에 맞춰 손 위치 결정하고 팔, 어깨 결정
  // 자연스럽게 물건을 집는 등, 물건의 위치에 손이 위치하도록 애니메이션을 변형 가능.
  private Animator playerAnimator; // 애니메이터 컴포넌트

  private void Start()
  {
    // 사용할 컴포넌트들을 가져오기
    playerInput = GetComponent<PlayerInput>();
    playerAnimator = GetComponent<Animator>();
  }

  private void OnEnable()
  {
    // 슈터가 활성화될 때 총도 함께 활성화
    gun.gameObject.SetActive(true);
  }

  private void OnDisable()
  {
    // 슈터가 비활성화될 때 총도 함께 비활성화
    gun.gameObject.SetActive(false);
  }

  private void Update()
  {
    // 입력을 감지하고 총 발사하거나 재장전
    if (this.playerInput.fire)
    {
      this.gun.Fire();
    }
    else if (this.playerInput.reload)
    {
      if (this.gun.Reload())
      {
        this.playerAnimator.SetTrigger("Reload");
      }
    }

    this.UpdateUI();
  }

  // 탄약 UI 갱신
  private void UpdateUI()
  {
    if (gun != null && UIManager.instance != null)
    {
      // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
      UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
    }
  }

  // 애니메이터의 IK 갱신
  private void OnAnimatorIK(int layerIndex)
  {
    // 총의 피벗 위치를 3D 모델의 오른쪽 팔꿈치 위치에 맞춤
    this.gunPivot.position = this.playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

    // IK에 대한 위치와 회전 가중치 설정
    this.playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
    this.playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

    // 왼손 위치와 회전을 총의 왼손 손잡이에 맞춤
    this.playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftHandMount.position);
    this.playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftHandMount.rotation);

    this.playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
    this.playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
    this.playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, this.rightHandMount.position);
    this.playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, this.rightHandMount.rotation);
  }
}