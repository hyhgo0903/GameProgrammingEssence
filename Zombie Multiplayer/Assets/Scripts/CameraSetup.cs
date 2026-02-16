using Cinemachine;
using Photon.Pun; // PUN 관련 코드

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
public class CameraSetup : MonoBehaviourPun
{
  void Start()
  {
    // 자신이 로컬 플레이어라면
    if (this.photonView.IsMine)
    {
      // 씬에 있는 씨네머신 가상 카메라를 찾음
      var followCam = FindFirstObjectByType<CinemachineVirtualCamera>();

      // 가상 카메라의 추적 대상을 이 플레이어의 트랜스폼으로 설정
      followCam.Follow = this.transform;
      followCam.LookAt = this.transform;
    }
  }
}
