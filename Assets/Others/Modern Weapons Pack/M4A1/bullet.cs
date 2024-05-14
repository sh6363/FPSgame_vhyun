using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject bulletholePrefab;
    public AudioClip gunshotSound; // AudioClip for the gunshot sound
    private AudioSource audioSource; // Reference to the AudioSource component
    private List<GameObject> bulletholes = new List<GameObject>(); // List to store instantiated bulletholes
    private int maxBulletholes = 10; // Maximum number of bulletholes allowed
    public float bulletDamage = 50; // Damage inflicted by the bullet

    void Start()
    {
        // Get the AudioSource component attached to the GameObject or add it if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Play the gunshot sound
            if (gunshotSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(gunshotSound);
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
                // Check if the object hit by the raycast is an enemy drone
                enemyAI enemy = hit.collider.GetComponent<enemyAI>();
                if (enemy != null)
                {
                    // If hit enemy drone, call its Hit function with bullet damage
                    enemy.Hit(bulletDamage);
                }
                else
                {
                    // If not hit an enemy, instantiate bullethole
                    //Impact
                    Debug.Log("Firing");
                    // Instantiate bullethole
                    GameObject bulletHole = GameObject.Instantiate(bulletholePrefab,
                            hit.point + hit.normal * 0.01f,
                            Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject;

                    // Add bullethole to the list
                    bulletholes.Add(bulletHole);

                    // Check if maximum number of bulletholes reached
                    if (bulletholes.Count > maxBulletholes)
                    {
                        // Remove the oldest bullethole
                        Destroy(bulletholes[0]);
                        bulletholes.RemoveAt(0);
                    }
                }
            }
        }
    }
}
