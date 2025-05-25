using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidCTRL : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;    
    public int type;

    //public InventoryManager inventoryManager;
    public GameObject explosionEffect;   
    public GameObject player; 
    public GameObject gold;
    public GameObject gem;
    public GameObject hydro;
    public GameObject platinum;




    public void InitializeHealth(float baseHealth, float scaleMultiplier)
    {
        maxHealth = baseHealth * scaleMultiplier;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        GameObject resource;
        if (type != 0)
        {
            GameObject res = gold;
            switch (type)
            {
                case 1: res = gold; break;
                case 2: res = gem; break;
                case 3: res = hydro; break;
                case 4: res = platinum; break;
            }
            
            Vector3 dropPosition = transform.position + Random.insideUnitSphere * 2f; 
            resource = Instantiate(res, dropPosition, Quaternion.identity);
            ResourceCTRL resourceCTRL = resource.GetComponent<ResourceCTRL>();
            resourceCTRL.type = type;
            resourceCTRL.transform.localScale = transform.lossyScale;
            resourceCTRL.value = (int)transform.lossyScale.x;
        //    resourceCTRL.inventoryManager = inventoryManager;
            resourceCTRL.player = player;
        }

        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
