using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ResourceCTRL : MonoBehaviour
{
    public int value;
    private Vector3 rotationAxis;
    public float rotationSpeed = 50f;
    public InventoryManager inventoryManager;
    public GameObject player;


    public float attractionTime = 5f;
    public float shrinkTime = 2f;
  //  private Transform playerTr;

    public int type = 1;

    private Coroutine _resourceCollection;

    public float collectionDistance = 3;

    void Start()
    {
        rotationAxis = Random.onUnitSphere;
        inventoryManager = player.GetComponent<InventoryManager>();
        collectionDistance = value+5;
    }

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }



    private bool isCollecting = false;

    private IEnumerator CollectResource()
    {
        isCollecting = true;

        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        if (inventoryManager.currentInventory + value <= inventoryManager.maxInventory) 
        {
            Debug.Log("Gold collected: " + value);
            inventoryManager.resourses[type - 1] += value;
            inventoryManager.UpdateUI();
            inventoryManager.currentInventory += value;

            while (elapsedTime < attractionTime) 
            {
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsedTime / shrinkTime);
                transform.position = Vector3.Lerp(transform.position, player.transform.position, elapsedTime / attractionTime);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
        //    Debug.Log("Destroy!");
            Destroy(gameObject);
        } 
        else 
        {
            Debug.Log("Inventory FULL! " + inventoryManager.currentInventory + "+" + value + "/" + inventoryManager.maxInventory);
        }
        isCollecting = false;
      //  Debug.Log("STOP!");
      //  StopCoroutine(_resourceCollection);
    }

    private void OnTriggerEnter(Collider other) 
    {
         if (other.GetComponent<ShipController>() != null && !isCollecting) 
        {
            Debug.Log("COLLECT!");
            StartCoroutine(CollectResource());
        }
    }

}
