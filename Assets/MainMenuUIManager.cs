using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MainMenuUIManager : MonoBehaviourPunCallbacks
{
    public GameObject WaitingToConnect, MainMenu;
    public TextMeshProUGUI searchingForGame, queueTimer;

    private float timeInQueue;
    // Start is called before the first frame update
    void Start()
    {
        WaitingToConnect.SetActive(true);
        MainMenu.SetActive(false);
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        WaitingToConnect.SetActive(false);
        MainMenu.SetActive(true);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        searchingForGame.gameObject.SetActive(true);
        timeInQueue = 0;
        queueTimer.text = $"{timeInQueue}";
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        searchingForGame.gameObject.SetActive(false);
    }

    void Update()
    {
        if(searchingForGame.gameObject.activeInHierarchy == true)
        {
            searchingForGame.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{GameManager.numberOfPlayersRequired} players found";
            float newTimerValue = timeInQueue += Time.deltaTime;


            string minutes = (Mathf.Floor(newTimerValue / 60)).ToString();
            string seconds = (Mathf.Floor(newTimerValue % 60)).ToString();

            if (newTimerValue / 60 < 10)
            {
                minutes = "0" + minutes;
            }
            if(newTimerValue % 60 < 10)
            {
                seconds = "0" + seconds;
            }

            string formattedTime = $"Time In Queue: {minutes}:{seconds}";

            queueTimer.text = formattedTime;
        }
    }




    public void EnterQueue()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void LeaveQueue()
    {
        PhotonNetwork.LeaveRoom();
    }
}
