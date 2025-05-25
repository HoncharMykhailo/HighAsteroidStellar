using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Values")]
    public float maxShield = 100f;
    public float maxArmor = 100f;
    public float maxHull = 100f;
    
    private float shield;
    private float armor;
    private float hull;

    [Header("Shield Regeneration")]
    public float shieldRegenRate = 10f; 
    public float shieldRegenDelay = 3f;
    private float lastDamageTime; 

    [Header("UI Elements")]
    public Slider shieldBar;
    public Slider armorBar;
    public Slider hullBar;

    void Start()
    {
        shield = maxShield;
        armor = maxArmor;
        hull = maxHull;

        UpdateUI();
    }

    void Update()
    {
        RegenerateShield();
    }

    void RegenerateShield()
    {
        if (shield < maxShield && Time.time > lastDamageTime + shieldRegenDelay)
        {
            shield += shieldRegenRate * Time.deltaTime;
            shield = Mathf.Clamp(shield, 0, maxShield);
            UpdateUI();
        }
    }

    public void TakeDamage(float damage)
    {
        lastDamageTime = Time.time;

        if (shield > 0)
        {
            float shieldDamage = Mathf.Min(shield, damage);
            shield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0 && armor > 0)
        {
            float armorDamage = Mathf.Min(armor, damage);
            armor -= armorDamage;
            damage -= armorDamage;
        }

        if (damage > 0)
        {
            hull -= damage;
            if (hull <= 0)
            {
                hull = 0;
                Die();
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        shieldBar.value = shield / maxShield;
        armorBar.value = armor / maxArmor;
        hullBar.value = hull / maxHull;
    }

    void Die()
    {
        Debug.Log("Корабель знищено!");
    }
}
