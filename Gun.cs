using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
    public Rigidbody2D gigaRocket;
	public float speed = 20f;				// The speed the rocket will fire at.

    public string fireButton;

    public string bulletInstanceTag = "P1Rocket";

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.

    private Rigidbody2D currentRocket;

    private float nextShot = 0.0f;
    public float cooldown = 1.0f;

    public GameObject shotSpawn;

    void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
	}

	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButton(fireButton) && Time.time > nextShot && !playerCtrl.isSpawning)
		{
            nextShot = Time.time + cooldown;
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();

            if (transform.parent.gameObject.GetComponent<PlayerHealth>().isPumped)
            {
                currentRocket = gigaRocket;
            } else
            {
                currentRocket = rocket;
            }

            Vector3 rocketSpawnPos = shotSpawn.transform.position;

            // If the player is facing right...
            if (playerCtrl.facingRight)
			{
                // ... instantiate the rocket facing right and set it's velocity to the right.
                rocketSpawnPos.x += 1.0f;
                Rigidbody2D bulletInstance = Instantiate(currentRocket, rocketSpawnPos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
                bulletInstance.tag = bulletInstanceTag;
			}
			else
			{
                // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                Rigidbody2D bulletInstance = Instantiate(currentRocket, rocketSpawnPos, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
                bulletInstance.tag = bulletInstanceTag;
            }
		}
	}
}
