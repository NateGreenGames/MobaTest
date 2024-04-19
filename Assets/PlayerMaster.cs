using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMaster : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerID;
    public eTeam team;
    public Camera playerCamera;
    public TextMeshProUGUI nameTag;

    [SerializeField] private int health = 100;
    private bool isFiring;

    private PlayerAbilityManager myAbilities;

    void Awake()
    {
        myAbilities = GetComponent<PlayerAbilityManager>();
        if (!photonView.IsMine)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
            nameTag.text = this.photonView.Owner.NickName;
        }
        else
        {
            nameTag.enabled = false;
        }
    }

    private void Update()
    {
        CollectInput();
        myAbilities.PrimaryFire(isFiring);
    }

    private void CollectInput()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButton(0))
            {
                isFiring = true;
            }
            else
            {
                isFiring = false;
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int _damageTaken)
    {
        if (health - _damageTaken > 0)
        {
            health -= _damageTaken;
        }
        else
        {
            health -= _damageTaken;
            //die;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.isFiring);
            stream.SendNext(this.health);
            stream.SendNext(this.team);
            stream.SendNext(this.playerID);
        }
        else
        {
            // Network player, receive data
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (int)stream.ReceiveNext();
            this.team = (eTeam)stream.ReceiveNext();
            this.playerID = (int)stream.ReceiveNext();

        }
    }
}
