using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour
{
  // 총의 상태를 표현하는데 사용할 타입을 선언한다
  public enum State
  {
    Ready, // 발사 준비됨
    Empty, // 탄창이 빔
    Reloading // 재장전 중
  }

  public State state { get; private set; } // 현재 총의 상태

  public Transform fireTransform; // 총알이 발사될 위치

  public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
  public ParticleSystem shellEjectEffect; // 탄피 배출 효과

  private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

  private AudioSource gunAudioPlayer; // 총 소리 재생기
  public AudioClip shotClip; // 발사 소리
  public AudioClip reloadClip; // 재장전 소리

  public float damage = 25; // 공격력
  private float fireDistance = 50f; // 사정거리

  public int ammoRemain = 100; // 남은 전체 탄약
  public int magCapacity = 25; // 탄창 용량
  public int magAmmo; // 현재 탄창에 남아있는 탄약


  public float timeBetFire = 0.12f; // 총알 발사 간격
  public float reloadTime = 1.8f; // 재장전 소요 시간
  private float lastFireTime; // 총을 마지막으로 발사한 시점


  private void Awake()
  {
    // 사용할 컴포넌트들의 참조를 가져오기
    this.gunAudioPlayer = GetComponent<AudioSource>();
    this.bulletLineRenderer = GetComponent<LineRenderer>();

    this.bulletLineRenderer.positionCount = 2;

    // 인스펙터에서 비활성화했지만 확실하게
    this.bulletLineRenderer.enabled = false;
  }

  private void OnEnable()
  {
    // 총 상태 초기화
    this.magAmmo = this.magCapacity;
    this.state = State.Ready;
    this.lastFireTime = 0f;
  }

  // 발사 시도
  public void Fire()
  {
    if (this.state != State.Ready)
    {
      return;
    }

    if (Time.time < this.lastFireTime + this.timeBetFire)
    {
      return;
    }

    this.lastFireTime = Time.time;
    Shot();
  }

  // 실제 발사 처리
  private void Shot()
  {
    Vector3 hitPosition = Vector3.zero;

    if (Physics.Raycast(
      this.fireTransform.position,
      this.fireTransform.forward,
      out var hit,
      this.fireDistance))
    {
      var target = hit.collider.GetComponent<IDamageable>();
      if (target != null)
      {
        target.OnDamage(this.damage, hit.point, hit.normal);
      }

      hitPosition = hit.point;
    }
    else
    {
      // 최대 사정 거리
      hitPosition 
        = this.fireTransform.position
        + this.fireDistance * this.fireTransform.forward;
    }

    this.StartCoroutine(this.ShotEffect(hitPosition));
    
    // 취소를 위해선 이런 형태로 이름을 붙일 수 있다
    //this.StartCoroutine("ShotEffect");
    //this.StopCoroutine("ShotEffect");

    --this.magAmmo;
    if (this.magAmmo <= 0)
    {
      this.state = State.Empty;
    }
  }

  // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
  // 코루틴 - 대기 시간을 가질 수 있는 메서드
  // IEnumerator를 반환 타입으로 사용하고 처리를 일시 대기할 곳에 yeid 키워드 명시
  private IEnumerator ShotEffect(Vector3 hitPosition)
  {
    this.muzzleFlashEffect.Play();
    this.shellEjectEffect.Play();

    // 중첩하여 재생 가능
    this.gunAudioPlayer.PlayOneShot(this.shotClip);

    // 선 시작점은 총구, 끝점은 충돌 위치
    this.bulletLineRenderer.SetPosition(0, this.fireTransform.position);
    this.bulletLineRenderer.SetPosition(1, hitPosition);
    
    // 라인 렌더러를 활성화하여 총알 궤적을 그린다
    this.bulletLineRenderer.enabled = true;

    // 0.03초 동안 잠시 처리를 대기
    yield return new WaitForSeconds(0.03f);

    // 한 프레임만 쉬기
    //yield return null;

    // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
    this.bulletLineRenderer.enabled = false;
  }

  // 재장전 시도
  public bool Reload()
  {
    if (this.state == State.Reloading)
    {
      return false;
    }

    if (this.ammoRemain <= 0)
    {
      return false;
    }

    if (this.magAmmo >= this.magCapacity)
    {
      return false;
    }

    this.StartCoroutine(this.ReloadRoutine());
    return true;
  }

  // 실제 재장전 처리를 진행
  private IEnumerator ReloadRoutine()
  {
    this.state = State.Reloading;
    this.gunAudioPlayer.PlayOneShot(this.reloadClip);

    yield return new WaitForSeconds(reloadTime);
    
    var ammoToFill = this.magCapacity - this.magAmmo;
    if (this.ammoRemain < ammoToFill)
    {
      ammoToFill = this.ammoRemain;
    }

    this.magAmmo += ammoToFill;
    this.state = State.Ready;
  }
}