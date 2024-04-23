using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerMaster : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerID;
    public eTeam team;
    public Camera playerCamera;
    public TextMeshProUGUI nameTag;
    public Slider playerHealthBar;

    [SerializeField] private int health = 100;
    [SerializeField] private int currentHealth;
    private bool isFiring;
    [SerializeField] float durationOfUIPopup;
    private float uiActiveTimer;

    private PlayerAbilityManager myAbilities;

    void Awake()
    {
        currentHealth = health;
        myAbilities = GetComponent<PlayerAbilityManager>();
        nameTag.enabled = false;
        playerHealthBar.gameObject.SetActive(false);
        if (!photonView.IsMine)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
            nameTag.text = this.photonView.Owner.NickName;
        }
    }
    private void Update()
    {
        CollectInput();
        UpdateUI();
        myAbilities.PrimaryFire(isFiring);
    }

    private void UpdateUI()
    {
        if (!photonView.IsMine)
        {
            if (uiActiveTimer > 0)
            {
                playerHealthBar.gameObject.SetActive(true);
                nameTag.enabled = true;
                playerHealthBar.gameObject.transform.LookAt(LobbyManager.instance.thisClientsPlayerPawn.transform.position);
                nameTag.gameObject.transform.LookAt(LobbyManager.instance.thisClientsPlayerPawn.transform.position);

                uiActiveTimer -= Time.deltaTime;
            }
            else
            {
                playerHealthBar.gameObject.SetActive(false);
                nameTag.enabled = false;
            }
        }
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
        currentHealth -= _damageTaken;
        uiActiveTimer = durationOfUIPopup;
        playerHealthBar.value = Mathf.InverseLerp(0, health, currentHealth);
        if (currentHealth - _damageTaken > 0)
        {
            //die and start respawn sequence
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.isFiring);
            stream.SendNext(this.team);
            stream.SendNext(this.playerID);
        }
        else
        {
            // Network player, receive data
            this.isFiring = (bool)stream.ReceiveNext();
            this.team = (eTeam)stream.ReceiveNext();
            this.playerID = (int)stream.ReceiveNext();

        }
    }
}
