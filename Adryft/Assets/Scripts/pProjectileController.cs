﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pProjectileController : MonoBehaviour {

    private int m_charge;
    private Vector3 originalPos;

    public void setCharge(float charge)
    {
        m_charge = (int)charge;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(life());
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Moves forward
        transform.Translate(0f, 0.05f, 0f);
    }

    // Collision detection
    void OnTriggerEnter2D(Collider2D collision)
    {
        // If it hits an enemy
        if (collision.tag == "Enemy")
        {
            // Does damage
            damageController dc = collision.gameObject.GetComponent<damageController>();
            int damage = 2 * m_charge;
            float knockback = 0.5f * m_charge;
            dc.doDamage(damage, "none", originalPos, knockback);
            // Self Destructs
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Wall" || collision.tag == "PowerBlock")
        {
            //spawn particle effect
            Destroy(this.gameObject);
        }
    }

    // Self destructs after a given amount of time
    IEnumerator life()
    {
        yield return new WaitForSeconds(5F);
        Destroy(this.gameObject);
    }
}

