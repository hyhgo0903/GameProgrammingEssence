using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
  public Slider healthSlider; // 체력을 표시할 UI 슬라이더

  public AudioClip deathClip; // 사망 소리
  public AudioClip hitClip; // 피격 소리
  public AudioClip itemPickupClip; // 아이템 습득 소리

  private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
  private Animator playerAnimator; // 플레이어의 애니메이터

  private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
  private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

  private void Awake()
  {
    // 사용할 컴포넌트를 가져오기
    this.playerAudioPlayer = GetComponent<AudioSource>();
    this.playerAnimator = GetComponent<Animator>();
    this.playerMovement = GetComponent<PlayerMovement>();
    this.playerShooter = GetComponent<PlayerShooter>();
  }

  // 부활기능을 염두하여 구현
  protected override void OnEnable()
  {
    // LivingEntity의 OnEnable() 실행 (상태 초기화)
    base.OnEnable();
    this.healthSlider.gameObject.SetActive(true);
    this.healthSlider.maxValue = this.startingHealth;
    this.healthSlider.value = this.health;

    this.playerMovement.enabled = true;
    this.playerShooter.enabled = true;
  }

  // 체력 회복
  public override void RestoreHealth(float newHealth)
  {
    // LivingEntity의 RestoreHealth() 실행 (체력 증가)
    base.RestoreHealth(newHealth);

    this.healthSlider.value = this.health;
  }

  // 데미지 처리
  public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
  {
    // LivingEntity의 OnDamage() 실행(데미지 적용)
    if (!dead)
    {
      this.playerAudioPlayer.PlayOneShot(this.hitClip);
    }

    base.OnDamage(damage, hitPoint, hitDirection);
    this.healthSlider.value = this.health;
  }

  // 사망 처리
  public override void Die()
  {
    // LivingEntity의 Die() 실행(사망 적용)
    base.Die();

    this.healthSlider.gameObject.SetActive(false);

    this.playerAudioPlayer.PlayOneShot(this.deathClip);
    this.playerAnimator.SetTrigger("Die");

    this.playerMovement.enabled = false;
    this.playerShooter.enabled = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
    if (dead)
    {
      return;
    }

    var item = other.GetComponent<IItem>();
    if (item != null)
    {
      item.Use(this.gameObject);
      this.playerAudioPlayer.PlayOneShot(this.itemPickupClip);
    }
  }
}