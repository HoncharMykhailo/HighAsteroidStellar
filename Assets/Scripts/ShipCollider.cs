using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public float impactForceThreshold = 10f; 
    public float collisionDamage = 10f;
    public GameObject explosionEffect; 
    private HealthSystem health;

    void Start()
    {
        health = GetComponent<HealthSystem>(); 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > impactForceThreshold)
        {
            health.TakeDamage(collision.relativeVelocity.magnitude*collisionDamage);

            AsteroidCTRL asteroid = collision.gameObject.GetComponent<AsteroidCTRL>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(collision.relativeVelocity.magnitude*collisionDamage);
            }

            if (explosionEffect){
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
