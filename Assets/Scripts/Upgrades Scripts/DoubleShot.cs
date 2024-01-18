using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleShot : MonoBehaviour
{
    public Move player;
    public GameManager points;
    private int cost = 5;
    private bool doubleShotActive = false;
    private float nextFireTime;
    private float fireRate;
    public float spreadAngle = 10f;
    public Button DoubledShotButton;
    public GameObject bulletObject; // Reference to the bullet prefab
    public Transform spawnPoint;
    void Start()
    {
        DoubledShotButton.onClick.AddListener(ActivateDoubleShot);
        nextFireTime = Time.time + player.fireInterval;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Fire1") && doubleShotActive && Time.time >= nextFireTime && player.canShootRL)
        {
            if(player.Ammo > 0){
            Shoot();
            nextFireTime = Time.time + player.fireInterval;
            player.Ammo = player.Ammo - 2;
            }
            else{
                StartCoroutine(player.Reloading());
                nextFireTime = Time.time + player.ReloadSpeed;
            }
            
        }
    }
     void ActivateDoubleShot()
    {   if(points.UpgradePoints >= cost){
        doubleShotActive = true;
        player.canShoot = false;
        points.UpgradePoints -= cost;
         DoubledShotButton.interactable = false;

    }
    
        
    }
    void Shoot()
    {
        float offsetAngle = spreadAngle / 2f;

        Quaternion leftSpreadRotation = Quaternion.Euler(0f, -offsetAngle, 0f);
        var bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation * leftSpreadRotation);

        Quaternion rightSpreadRotation = Quaternion.Euler(0f, offsetAngle, 0f);
        bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation * rightSpreadRotation);
        
    }
    }

