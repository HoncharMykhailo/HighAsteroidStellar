using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float fireRate = 0.2f; 
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
