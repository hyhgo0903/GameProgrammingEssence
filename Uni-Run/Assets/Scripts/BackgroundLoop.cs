using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour
{
  private float width; // 배경의 가로 길이

  // Start 처럼 초기 1회 실행되는 유니티 이벤트 메서드지만 Start보다 한 프레임 빠름.
  private void Awake()
  {
    var boxCollider = GetComponent<BoxCollider2D>();
    this.width = boxCollider.size.x;
  }

  private void Update()
  {
    if (this.transform.position.x <= -this.width)
    {
      this.transform.Translate(this.width * 2f, 0, 0);
    }
  }
}