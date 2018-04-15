using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsScript : MonoBehaviour {

    //public GameObject[] pickups;
    public float cooldown = 5f;       // Delay on delivery.
    
    public float highHealthThreshold = 75f;     // The health of the player, above which only bomb crates will be delivered.
    public float lowHealthThreshold = 25f;		// The health of the player, below which only health crates will be delivered.
    public float healthBonus = 25f;
    public int bombsBonus = 5;

    public Text powerupText;

    private PlayerHealth playerHealth;
    private LayBombs playerBomb;
    private float spawnTime;
    private Vector3 initialScale;
    void Awake()
    {
        // Setting up the reference.
        initialScale = gameObject.transform.localScale;
    }

    void Start()
    {
        // Start the first delivery.
        
    }

    void Update()
    {
        if (Time.time >= spawnTime)
        {
            gameObject.transform.localScale = initialScale;
        }
    }

    IEnumerator DisableObjectOnTimer()
    {
        yield return new WaitForSeconds(3);
        powerupText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            playerHealth = col.gameObject.GetComponent<PlayerHealth>();
            playerBomb = col.gameObject.GetComponent<LayBombs>();
            gameObject.transform.localScale = new Vector3();
            spawnTime = Time.time + cooldown;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.Play();
            //print("[" + Time.time + "] New spawn time: " + spawnTime);

            if (playerHealth.health < lowHealthThreshold)
            {
                playerHealth.Heal(healthBonus);
                powerupText.text = playerHealth.playerName + " received health pickup!";
                powerupText.gameObject.SetActive(true);
                StartCoroutine("DisableObjectOnTimer");
                return;
            }

            if (playerHealth.health > highHealthThreshold)
            {
                playerBomb.bombCount = bombsBonus;
                powerupText.text = playerHealth.playerName + " received bombs pickup!";
                powerupText.gameObject.SetActive(true);
                StartCoroutine("DisableObjectOnTimer");
                return;
            }

            int rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                playerHealth.Heal(healthBonus);
                powerupText.text = playerHealth.playerName + " received health pickup!";
                powerupText.gameObject.SetActive(true);
                StartCoroutine("DisableObjectOnTimer");
            }
            else
            {
                playerBomb.bombCount = bombsBonus;
                powerupText.text = playerHealth.playerName + " received bombs pickup!";
                powerupText.gameObject.SetActive(true);
                StartCoroutine("DisableObjectOnTimer");
            }
        }
    }
}
