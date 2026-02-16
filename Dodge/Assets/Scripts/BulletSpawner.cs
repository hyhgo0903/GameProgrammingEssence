using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
  public GameObject bulletPrefab;
  public float spawnRateMin = 0.5f;
  public float spawnRateMax = 3f;

  private Transform target;
  private float spawnRate;
  private float timerAfterSpawn;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    this.timerAfterSpawn = 0f;
    this.spawnRate = Random.Range(this.spawnRateMin, this.spawnRateMax);

    // 여러 총알생성기를 만들었을 때 일일히 연결 안 하기 위해 Find로
    // 씬에 있는 걸 검색하므로 업데이트 같은 곳에서 쓰면 성능에 안 좋음
    this.target = FindFirstObjectByType<PlayerController>().transform;
  }

  // Update is called once per frame
  void Update()
  {
    this.timerAfterSpawn += Time.deltaTime;

    if (this.timerAfterSpawn < this.spawnRate)
    {
      return;
    }

    this.timerAfterSpawn -= this.spawnRate;
    this.spawnRate = Random.Range(this.spawnRateMin, this.spawnRateMax);

    var bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
    bullet.transform.LookAt(this.target);
  }
}
