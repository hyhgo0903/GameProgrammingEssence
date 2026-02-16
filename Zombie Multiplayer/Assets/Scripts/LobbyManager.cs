using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
// 포톤 콜백 감지
public class LobbyManager : MonoBehaviourPunCallbacks
{
  private string gameVersion = "1"; // 게임 버전

  public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
  public Button joinButton; // 룸 접속 버튼

  // 게임 실행과 동시에 마스터 서버 접속 시도
  private void Start()
  {
    PhotonNetwork.GameVersion = gameVersion;
    PhotonNetwork.ConnectUsingSettings();

    this.joinButton.interactable = false;
    this.connectionInfoText.text = "마스터 서버에 접속 중...";
  }

  // 마스터 서버 접속 성공시 자동 실행
  public override void OnConnectedToMaster()
  {
    this.joinButton.interactable = true;
    this.connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
  }

  // 마스터 서버 접속 실패시 자동 실행
  public override void OnDisconnected(DisconnectCause cause)
  {
    this.joinButton.interactable = false;
    this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n재접속 시도 중...";

    PhotonNetwork.ConnectUsingSettings();
  }

  // 룸 접속 시도
  public void Connect()
  {
    this.joinButton.interactable = false;

    if (PhotonNetwork.IsConnected)
    {
      this.connectionInfoText.text = "룸에 접속 중...";
      PhotonNetwork.JoinRandomRoom();
    }
    else
    {
      this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n재접속 시도 중...";
      PhotonNetwork.ConnectUsingSettings();
    }
  }

  // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    this.connectionInfoText.text = "새 룸 생성 중...";
    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
  }

  // 룸에 참가 완료된 경우 자동 실행
  public override void OnJoinedRoom()
  {
    this.connectionInfoText.text = "룸 참가 성공";

    // 모든 룸 참가자가 씬을 넘어간다.
    // LoadScene: 네트워크 정보가 유지되지 않고 각 플레이어의 동기화가 없음
    // LoadLevel: 룸의 플레이어들이 함께 새로운 씬으로 이동하는 포톤의 메서드.
    // 룸을 생성한 방장이 실행하면 다른 플레이어들도 기본적으로 실행.

    // SceneManager.LoadScene("Main"); (X)
    PhotonNetwork.LoadLevel("Main");
  }
}
