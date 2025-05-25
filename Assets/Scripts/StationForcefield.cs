using UnityEngine;

public class StationForcefield : MonoBehaviour
{
    public GameObject player;
    public Collider sphereCollider;
    private MeshCollider playerCollider;
    void Start()
    {
        playerCollider = player.GetComponent<MeshCollider>();
        sphereCollider = GetComponentInChildren<SphereCollider>(); 
        Physics.IgnoreCollision(sphereCollider, playerCollider, true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only check for collision with the asteroid tag and within the specified sphere collider
        if (collision.gameObject.GetComponent<AsteroidCTRL>()!=null && collision.collider == sphereCollider)
        {
            
            Rigidbody asteroidRb = collision.gameObject.GetComponent<Rigidbody>();
            if (asteroidRb != null)
            {
                // Apply a force to push the asteroid away from the station
                Vector3 repelDirection = collision.transform.position - transform.position;
                asteroidRb.AddForce(repelDirection.normalized * 1, ForceMode.Impulse);
            }
        }
    }
}