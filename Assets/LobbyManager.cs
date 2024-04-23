using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum eTeam { redTeam, blueTeam, terminator}
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;
    public GameObject thisClientsPlayerPawn;

    public SpawnPoint[] redTeamSpawnPoints;
    public SpawnPoint[] blueTeamSpawnPoints;

    private bool setupPerformed = false;
    void Update()
    {
        if(GameManager.instance.masterLoadingProgress == 1 && !setupPerformed)
        {
            instance = this;
            setupPerformed = true;
            thisClientsPlayerPawn = PhotonNetwork.Instantiate(GameManager.instance.playerPawn.name, Vector3.zero, Quaternion.identity);

            PlayerMaster playerRef = thisClientsPlayerPawn.GetComponent<PlayerMaster>();
            playerRef.playerID = playerRef.photonView.Owner.ActorNumber;
            UpdateMyPlayerTeams();
            GetRandomAvailableSpawnPoint(playerRef.team).SpawnPlayer(playerRef);
        }

        foreach (var item in PhotonNetwork.PlayerList)
        {
            Debug.Log(item.CustomProperties["Team"]);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if(targetPlayer != null && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            UpdateMyPlayerTeams();
        }
    }
    private void UpdateMyPlayerTeams()
    {
        ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable();


        int numRedTeam = 0;
        int numBlueTeam = 0;

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
            if (PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("Team"))
            {
                if ((int)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == 0)
                {
                    numRedTeam++;
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["Team"] == 1)
                {
                    numBlueTeam++;
                }
            }
            }

            if (numRedTeam < numBlueTeam)
            {
                newProperties["Team"] = 1;
            }
            else
            {
                newProperties["Team"] = 0;
            }
            PhotonNetwork.SetPlayerCustomProperties(newProperties); 
    }

    public SpawnPoint GetRandomAvailableSpawnPoint(eTeam _playerTeam)
    {
        if(_playerTeam == eTeam.redTeam)
        {
            List<int> availableSpawnPoints = new List<int>();
            for (int i = 0; i < redTeamSpawnPoints.Length; i++)
            {
                if (redTeamSpawnPoints[i].availableForSpawning) availableSpawnPoints.Add(i);
            }

            if(availableSpawnPoints.Count > 0)
            {
                return redTeamSpawnPoints[availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)]];
            }
            else
            {
                return redTeamSpawnPoints[Random.Range(0, redTeamSpawnPoints.Length)];
            }
        }
        else
        {
            List<int> availableSpawnPoints = new List<int>();
            for (int i = 0; i < blueTeamSpawnPoints.Length; i++)
            {
                if (blueTeamSpawnPoints[i].availableForSpawning) availableSpawnPoints.Add(i);
            }

            if (availableSpawnPoints.Count > 0)
            {
                return blueTeamSpawnPoints[availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)]];
            }
            else
            {
                return blueTeamSpawnPoints[Random.Range(0, blueTeamSpawnPoints.Length)];
            }
        }
    }
}
