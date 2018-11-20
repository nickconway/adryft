﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour
{

    // Game Object Variables
    [SerializeField]
    private AudioSource ochSource;
    [SerializeField]
    private GameObject gameOver;
    public CameraController mainCamera;

    private damageController dc;

    //variables
    private float mSpeed;
    private float tSpeed;
    private bool stunned;
    private int dash;
    private int ammo;
    private int maxAmmo = 20;
    private float stamina;
    private float maxStamina = 1f;
    private Vector3 direction;
    private bool isStaminaRegen = false;
    private bool isStaminaUsed = false;
    //private bool isStaminaInDelay = false;

    // Max ammo getter
    public int getMaxAmmo()
    { return maxAmmo; }
    // Ammo getter
    public int getAmmunition()
    { return ammo; }

    // Max Stamina getter
    public float getMaxStamina()
    { return maxStamina; }
    // Stamina getter
    public float getStamina()
    { return stamina; }
    // Sets stamina to zero
    public void zeroStamina()
    { stamina = -1; }

    // Ammo setter
    public void setAmmunition(int ammount)
    {
        ammo = ammount;
        normalizeAmmo();
    }
    // Ammo incramenter
    public void incAmmunition(int ammount)
    {
        ammo += ammount;
        normalizeAmmo();
    }

    // Stunned getter 
    public bool getIsStunned()
    { return stunned; }

    // Use this for initialization
    void Start()
    {
        mSpeed = 2f;
        ochSource = GetComponent<AudioSource>();
        dc = GetComponent<damageController>();
        ammo = maxAmmo;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(mainCamera.cameraShake(0.1f, 0.01f));
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //StartCoroutine(mainCamera.cameraShake(0.1f, 0.001f));
        }

        if (stunned)
        {
            // stun effect
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        //checks if Dash button (Space) was pressed AND
        //1. player wasn't already in dash mode,
        //2. player is stationary
        if (Input.GetButtonDown("Jump") && dash == 0 && (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f))
        {
            dash = 1;
            StartCoroutine(Dash());
        }

        // If dashing move faster and set flying to true
        if (dash == 1)
        {
            tSpeed = mSpeed * Time.deltaTime * 5;
        }
        else
        {
            tSpeed = mSpeed * Time.deltaTime;
        }
        // Move in the desired direction
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        transform.Translate(tSpeed * direction.normalized);
    }

    // Use stamina
    public bool useStamina(float ammount)
    {
        StartCoroutine(staminaRegen());

        if (ammount <= stamina)
        {
            stamina -= ammount;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Dash
    IEnumerator Dash()
    {
        // Dash
        dc.setFlying(true);
        yield return new WaitForSeconds(0.15F);
        dash = 2;
        dc.setFlying(false);

        // Dash cooldown
        yield return new WaitForSeconds(1F);
        dash = 0;
    }

    // Stun
    IEnumerator stun(float time)
    {
        if (!stunned)
        {
            stunned = true;
            yield return new WaitForSeconds(time/2);
            stunned = false;
        }
    }

    // Regenerates stamina
    IEnumerator staminaRegen()
    {
        isStaminaUsed = true;
        // Delays alot if there is no stamina left
        if (stamina <= 0f)
        {
            yield return new WaitForSeconds(1.5f);
        }
        // Delays a little if there is stamina left
        else
        {
            yield return new WaitForSeconds(0.4f);
        }
        isStaminaUsed = false;

        // Makes sure not to double up on regen
        if (isStaminaRegen || isStaminaUsed)
        {
            yield break;
        }

        isStaminaRegen = true;
        yield return new WaitForSeconds(0.01f);

        // Loops until stamina is full
        while (stamina <= maxStamina)
        {
            yield return new WaitForSeconds(0.01f);
            // Increases stamina
            stamina += 0.005f;

            // Stops if stamina is used
            if (isStaminaUsed)
            {
                isStaminaRegen = false;
                yield break;
            }
        }
        isStaminaRegen = false;
        normalizeStamina();
    }

    // Normalizes stamina
    void normalizeStamina()
    {
        if (stamina > maxStamina) stamina = maxStamina;
        else if (stamina < 0) stamina = 0;
    }

    // Normalizes Ammo
    void normalizeAmmo()
    {
        if (ammo > maxAmmo) ammo = maxAmmo;
        else if (ammo < 0) ammo = 0;
    }

    public void SaveTo(PlayerData pd)
    {
        pd.p_ammo = ammo;
        pd.p_maxAmmo = ammo;
        pd.p_maxStamina = maxStamina;

        dc.SaveTo(pd);
    }

    public void LoadFrom(PlayerData pd)
    {
        ammo = pd.p_ammo;
        maxAmmo = pd.p_maxAmmo;
        maxStamina = pd.p_maxStamina;
        stamina = maxStamina;

        dc.LoadFrom(pd);
    }
}

        