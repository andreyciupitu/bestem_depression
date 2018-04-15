using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LayBombs : MonoBehaviour
{
    public string bulletInstanceTag = "P1Rocket";

	public AudioClip bombsAway;			// Sound for when the player lays a bomb.
	public GameObject bomb;				// Prefab of the bomb.
    public GameObject gigaBomb;				// Prefab of the bomb.
    public int bombCount = 2;

    public string fireButton;

    public Text bombText;
    public string starterText = "PLAYER X BOMBS: ";

    private PlayerControl playerCtrl;

	void Awake ()
	{
        playerCtrl = GetComponentInParent<PlayerControl>();
	}

    private void Start()
    {
        InvokeRepeating("GiveBomb", 3.0f, 3.0f);
    }

    void Update ()
	{
        //update the bomb text
        bombText.text = starterText + bombCount;

        // If the bomb laying button is pressed, the bomb hasn't been laid and there's a bomb to lay...
        if (Input.GetButtonDown(fireButton) && bombCount > 0 && !playerCtrl.isSpawning)
        {
            // Decrement the number of bombs.
            --bombCount;

            // Play the bomb laying sound.
            //AudioSource.PlayClipAtPoint(bombsAway,transform.position);

            // Instantiate the bomb prefab.

            GameObject instantiated;

            if (gameObject.GetComponent<PlayerHealth>().isPumped)
            {
                instantiated = Instantiate(gigaBomb, transform.position, transform.rotation);
            }
            else
            {
                instantiated = Instantiate(bomb, transform.position, transform.rotation);
            }

            instantiated.tag = bulletInstanceTag;
        }
	}

    void GiveBomb()
    {
        if(bombCount < 2)
        {
            ++bombCount;
        }
    }
}
