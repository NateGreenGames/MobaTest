using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public enum eGameState { connectingToMaster, mainMenu, searchingForGame, inGame}
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public string[] mapNames;
    public GameObject playerPawn;
    public const int numberOfPlayersRequired = 4;

    private int state = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        if (state == (int)eGameState.searchingForGame)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayersRequired)
            {
                ChangeGameState(eGameState.inGame);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        state = (int)eGameState.mainMenu;
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        ChangeGameState(eGameState.searchingForGame);
    }

    public override void OnLeftRoom()
    {
        ChangeGameState(eGameState.mainMenu);
        SceneManager.LoadScene("SampleScene");
    }
    private void ChangeGameState(eGameState _newState)
    {
        state = (int)_newState;
        if(_newState == eGameState.inGame)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(mapNames[Random.Range(0, mapNames.Length)]);
            }
        }
    }
}
