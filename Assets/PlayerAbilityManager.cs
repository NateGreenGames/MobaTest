using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerAbilityManager : MonoBehaviourPunCallbacks
{
    
    public GameObject projectile;
    public Transform projectileSpawnPoint;

    public float primaryRateOfFire = 50f;
    private float timeSinceLastShot = 0;
    private PlayerMaster myPlayerInfo;
    private void Start()
    {
        myPlayerInfo = gameObject.GetComponent<PlayerMaster>();
    }

    public void PrimaryFire(bool _isFiring)
    {
        if(timeSinceLastShot > 1/ primaryRateOfFire)
        {
            if (_isFiring)
            {
                timeSinceLastShot = 0;
                GameObject newBullet = PhotonNetwork.Instantiate(projectile.name, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                newBullet.GetComponent<ProjectileBehavior>()._ownerInformation = myPlayerInfo;
            }
        }
        else
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }
}
