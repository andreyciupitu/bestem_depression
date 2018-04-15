using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.

    public float damageAmmount = 20f;

	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 2);
	}

	void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.

		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...
		if(col.tag == "Player")
		{
            // ... find the Enemy script and call the Hurt function.
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmmount, gameObject.tag);


			// Call the explosion instantiation.
			OnExplode();

			// Destroy the rocket.
			Destroy (gameObject);
		}
		// Otherwise if it hits a bomb crate...
		else if(col.tag == "BombPickup")
		{
			// ... find the Bomb script and call the Explode function.
			col.gameObject.GetComponent<Bomb>().Explode();

			// Destroy the bomb crate.
			Destroy (col.transform.root.gameObject);

			// Destroy the rocket.
			Destroy (gameObject);
		}
		// Otherwise if the player manages to shoot himself...
		else
		{
			// Instantiate the explosion and destroy the rocket.
			OnExplode();
			Destroy (gameObject);
		}
	}
}
