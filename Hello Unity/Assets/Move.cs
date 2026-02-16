using UnityEngine;

public class Move : MonoBehaviour
{
  public Transform childTransform;

  void Start()
  {
    transform.position = new Vector3(0, -1, 0);
    childTransform.localPosition = new Vector3(0, 2, 0);

    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30));
    childTransform.localRotation = Quaternion.Euler(new Vector3(0, 60, 0));

    // 내적 : 각도를 알아내기 위해 쓴다
    // 외적: 두 벡터와 수직하는 벡터를 구하는 연산

    // Vector3.forward : new Vector3(0, 0, 1)
    // Vector3.back : new Vector3(0, 0, -1)
    // Vector3.right : new Vector3(1, 0, 0)
    // Vector3.left : new Vector3(-1, 0, 0)
    // Vector3.up : new Vector3(0, 1, 0)
    // Vector3.down : new Vector3(0, -1, 0)

    // transform.forward : 오브젝트의 앞 방향 (z축 방향) .. 뒤는 * -1
    // transform.right : 오브젝트의 오른쪽 방향 (x축 방향) .. 왼쪽은 * -1
    // transform.up : 오브젝트의 위 방향 (y축 방향) .. 아래는 * -1
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.UpArrow))
    {
      transform.Translate(new Vector3(0, 2, 0) * Time.deltaTime);
      //transform.position = transform.position + transform.up * 2;

      // 전역 공간으로 평행이동 한다면..
      //transform.Translate(new Vector3(0, 2, 0) * Time.deltaTime, Space.World);
      //or transform.position = transform.position + new Vector(0, 2, 0);
    }

    if (Input.GetKey(KeyCode.DownArrow))
    {
      transform.Translate(2 * Time.deltaTime * Vector3.down);
    }

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
      childTransform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
      //childTransform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime, Space.World);
    }

    if (Input.GetKey(KeyCode.RightArrow))
    {
      transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
      childTransform.Rotate(new Vector3(0, -180, 0) * Time.deltaTime);
    }
  }
}
