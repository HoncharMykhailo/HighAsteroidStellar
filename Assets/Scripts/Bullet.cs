using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 25f;
    public float lifeTime = 3f;

    public GameObject explosionEffect;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        AsteroidCTRL asteroid = other.GetComponent<AsteroidCTRL>();
        if (asteroid)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            asteroid.TakeDamage(damage); // Deal damage
            Destroy(gameObject); // Bullet disappears on hit
        }
    }
}
