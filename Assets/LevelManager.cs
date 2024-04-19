using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum eTeam { redTeam, blueTeam, terminator}
public class LevelManager : MonoBehaviourPunCallbacks
{
    public Transform[] redTeamSpawnPoints;
    public Transform[] blueTeamSpawnPoints;
    void Start()
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(GameManager.instance.playerPawn.name, Vector3.zero, Quaternion.identity);

        PlayerMaster playerRef = newPlayer.GetComponent<PlayerMaster>();
        playerRef.playerID = playerRef.photonView.Owner.ActorNumber;
        if (playerRef.playerID % 2 == 1)
        {
            playerRef.team = eTeam.redTeam;
            newPlayer.transform.position = redTeamSpawnPoints[playerRef.playerID - 1].position;
            newPlayer.transform.rotation = redTeamSpawnPoints[playerRef.playerID - 1].rotation;
        }
        else
        {
            playerRef.team = eTeam.blueTeam;
            newPlayer.transform.position = blueTeamSpawnPoints[playerRef.playerID - 1].position;
            newPlayer.transform.rotation = blueTeamSpawnPoints[playerRef.playerID - 1].rotation;
        }
    }
}
