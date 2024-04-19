using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileBehavior : MonoBehaviour
{
    public PlayerMaster _ownerInformation;
    public float projectileSpeed;
    public int damageDealt;
    public float lifeTime;

    private float currentLifetime = 0;
    private Rigidbody m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if(currentLifetime >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool playerTest = other.TryGetComponent<PlayerMaster>(out PlayerMaster player);
        if (playerTest)
        {
            if(player.team != _ownerInformation.team)
            {
                //if we know that it's a player, we can acess their photonView;
                other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damageDealt);
                Destroy(this.gameObject);
            }
        }else if (!playerTest && other.tag != "PlayerCollisionOnly")
        {
            Destroy(this.gameObject);
        }
    }
}
