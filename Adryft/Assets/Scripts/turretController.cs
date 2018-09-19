﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretController : MonoBehaviour {

    // Variables
    public GameObject player;
    public GameObject projectile;

    public float range = 5;
    public float dist;
    public bool canShoot = true;

    //public float projectileSpeed = 5;

    //public int n = 0;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
        FacePlayer();

        // Send out a Raycast and stores the hit result in hit
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        dist = Vector3.Distance(player.transform.position, transform.position);

        if (canShoot && hit.transform.tag == "Player" && dist <= range)
        {
            StartCoroutine(Shoot()); 
        }
	}

    void FacePlayer()
    {
        // Finds where the player is
        Vector3 playerPosition = player.transform.position;

        // Calculates the angle to look at the player
        Vector2 direction = new Vector2(
                    playerPosition.x - transform.position.x,
                    playerPosition.y - transform.position.y);
        // Looks at the player
        transform.right = direction;

    }

    IEnumerator Shoot()
    {
        //n++;
        //Debug.Log("Shoot no." + n);
        var clone = Instantiate(projectile, transform.position, Quaternion.identity);
        clone.transform.up = transform.right;
        canShoot = false;
        yield return new WaitForSeconds(0.75F);
        canShoot = true;
    }
}