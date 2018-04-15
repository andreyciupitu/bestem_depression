using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public string playerName = "PlayerName";

    public Text winText;
    public float maxHealth = 100f;
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.

    public bool isPumped = false;
    public float lifetime = 10.0f;

    public Button btnQuit;
    public Button btnAgain;

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player

    private float spawnTime;

    public float invulnerability = 2.0f;

	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();
		healthBar = transform.Find("healthDisplay/HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
        spawnTime = Time.time;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			
		}
	}

    public void TakeDamage(float damageAmmount, string tag)
    {
        if (Time.time < spawnTime + invulnerability)
        {
            playerControl.isSpawning = true;
            return;
        }

        playerControl.isSpawning = false;
        // Reduce the player's health by 10.
        health -= damageAmmount;

        // Play a random clip of the player getting hurt.
        //FIXME int i = Random.Range(0, ouchClips.Length);
        //AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);

            //Debug.Log("Taking damage from " + tag);

            // Update what the health bar looks like.
            UpdateHealthBar(tag);
    }

    public void Heal(float healAmmount)
    {
        health += healAmmount;
        Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    public void NotifyHasKilled()
    {
        if (isPumped)
        {
            GetComponentInParent<PlayerControl>().dead = false;
            anim.SetTrigger("Revive");
            anim.SetBool("Evolved", false);
            isPumped = false;

            health = maxHealth;
            UpdateHealthBar();
        }
    }

    public void UpdateHealthBar (string tag="default")
	{
        if (!isPumped)
        {
            // Set the health bar's colour to proportion of the way between green and red based on the player's health.
            healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
            // Set the scale of the health bar to be proportional to the player's health.
            healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);

            if (health < 0)
            {
                GetComponentInParent<PlayerControl>().dead = true;

                anim.SetTrigger("Die");
                anim.SetBool("Evolved", true);

                health = 300f;
                healthBar.material.color = Color.magenta; //Color.Lerp(Color.red, Color.gray, 0.5f);
                healthBar.transform.localScale = new Vector3(healthScale.x, 1, 1);

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.volume = 1.0f;
                audioSource.Play();

                spawnTime = Time.time;

                isPumped = true;

                //ping other player i died
                GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
                foreach(GameObject player in allPlayers)
                {
                    if(player.GetComponent<LayBombs>().bulletInstanceTag == tag && player!=gameObject)
                    {
                        player.GetComponent<PlayerHealth>().NotifyHasKilled();
                    }
                }
            }
        }
        else
        {
            // Set the scale of the health bar to be proportional to the zombie's health.
            healthBar.transform.localScale = new Vector3(healthScale.x * health / 300f, 1, 1);

            // you had your chance, now it's gone!
            if (health < 0)
            {
                anim.SetTrigger("Die");
                anim.SetBool("Evolved", false);

                isPumped = false;

                if(tag == "P1Rocket" && playerName!="PLAYER 1")
                {
                    Debug.Log("Game Over! Player 1 wins !!!!");
                    winText.text = "PLAYER 1 WINS!";
                }
                else if (tag == "P2Rocket" && playerName!="PLAYER 2")
                {
                    Debug.Log("Game Over! Player 2 wins !!!!");
                    winText.text = "PLAYER 2 WINS!";
                }
                else
                {
                    Debug.Log("Game Over! Bad Luck, "+playerName+"! Better luck next time!");
                    winText.text = playerName+" DIED!";
                }

                winText.gameObject.SetActive(true);
                btnAgain.gameObject.SetActive(true);
                btnQuit.gameObject.SetActive(true);

                Time.timeScale = 0.0f; //TODO be careful this breaks games makes them become frozen in time
            }
        }
	}

    public void Update()
    {
        if(transform.position.y < -8.5)
        {
            //instakill if player falls into the pit of doom
            Debug.Log("Game Over! Bad Luck, " + playerName + "! Better luck next time!");
            winText.text = playerName + " DIED!";
            winText.gameObject.SetActive(true);
            btnAgain.gameObject.SetActive(true);
            btnQuit.gameObject.SetActive(true);
            Time.timeScale = 0.0f; //TODO be careful this breaks games makes them become frozen in time
        }

        if (isPumped)
        {
                TakeDamage(Time.deltaTime * health / lifetime, "bad luck");
        }    
    }
}
