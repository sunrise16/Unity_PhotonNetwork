using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text infoText;
    public Button connectButton;
    private string gameVersion = "";

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }

    // 연결 시도
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        infoText.text = "마스터 서버에 접속중...";
        connectButton.interactable = false;
    }

    // 연결에 성공한 경우
    public override void OnConnectedToMaster()
    {
        infoText.text = "온라인 : 마스터 서버와 연결됨";
        connectButton.interactable = true;
    }

    // 연결이 끊긴 경우
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
        infoText.text = "오프라인 : 마스터 서버와 연결 실패 \n 접속 재시도 중...";
        connectButton.interactable = false;
    }

    // 방에 입장 시도
    public void OnConnect()
    {
        connectButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            infoText.text = "랜덤방에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            infoText.text = "오프라인 : 마스터 서버와 연결 실패 \n 접속 재시도 중...";
        }
    }

    // 방에 참가가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        infoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("GameScene");
    }

    // 방이 없어 랜덤 입장에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoText.text = "빈 방이 없어 새로운 방 생성중...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}