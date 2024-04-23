using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPoint : MonoBehaviour, IPunObservable
{
    public bool availableForSpawning = true;
    [SerializeField] private float lockoutTimeAfterSpawn;
    private float timeSinceLastSpawn;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(timeSinceLastSpawn < lockoutTimeAfterSpawn) timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= lockoutTimeAfterSpawn)
            {
                timeSinceLastSpawn = 0;
                availableForSpawning = true;
            }
        }
    }

    public void SpawnPlayer(PlayerMaster _playerInstance)
    {
        _playerInstance.transform.position = gameObject.transform.position;
        _playerInstance.transform.rotation = gameObject.transform.rotation;
        timeSinceLastSpawn = 0;
        availableForSpawning = false;
        //Call function to reactivate the player.
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.timeSinceLastSpawn);
            stream.SendNext(this.availableForSpawning);
        }
        else
        {
            // Network player, receive data
            this.timeSinceLastSpawn = (float) stream.ReceiveNext();
            this.availableForSpawning = (bool)stream.ReceiveNext();

        }
    }
}
