using System.Collections;
using UnityEngine;

public class StationAnim : MonoBehaviour
{
    public Vector3 approachOffset;
    public float approachDuration = 2f;
    public float dockingDuration = 2f;
    public float rotationDuration = 1.5f;
    public float undockingDuration = 2f;
    public float colliderEnableDelay = 3f; // Extra time before reactivating collider

    private Coroutine _docking;
    public bool isDocked = false;
    private float exitKeyHoldTime = 0f;
    private Transform dockedShip;
    private BoxCollider stationCollider;

    public Animator stationAnimator;
    public CameraFollow cam;

    public StationManager stationManager;
    public GameObject player;
    public InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = player.GetComponent<InventoryManager>();
        
        stationAnimator = GetComponent<Animator>();
        stationAnimator.Play("ender", 0, 1f);
        stationCollider = GetComponent<BoxCollider>(); // Get the BoxCollider
        cam = Camera.main.GetComponent<CameraFollow>();

        stationAnimator.SetBool("shipEntered", false);
        stationManager = GetComponent<StationManager>();
    }

    private IEnumerator DockShip(Transform shipTransform)
    {
        
        

        ShipController shipController = shipTransform.GetComponent<ShipController>();
        if (shipController != null)
        {
            shipController.enabled = false;
        }

        dockedShip = shipTransform;
        isDocked = true;

        Vector3 approachPos = transform.position + transform.TransformDirection(approachOffset);
        Quaternion approachRot = Quaternion.LookRotation(transform.position - approachPos, transform.up);

        Vector3 startPos = shipTransform.position;
        Quaternion startRot = shipTransform.rotation;

        float elapsed = 0f;
        stationAnimator.SetBool("shipEntered", false);

        

        while (elapsed < approachDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / approachDuration;

            shipTransform.position = Vector3.Lerp(startPos, approachPos, t);
            shipTransform.rotation = Quaternion.Slerp(startRot, approachRot, t);

            yield return null;
        }




        shipTransform.position = approachPos;
        shipTransform.rotation = approachRot;

        if (stationAnimator != null)
        {
            Debug.Log("START");
            stationAnimator.Play("start",0,0);
        }

        elapsed = 0f;
        startPos = shipTransform.position;

        while (elapsed < dockingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dockingDuration;

            shipTransform.position = Vector3.Lerp(startPos, transform.position, t);

            yield return null;
        }

        shipTransform.position = transform.position;

        Debug.Log("DOCKING COMPLETE!");

        elapsed = 0f;
        startRot = shipTransform.rotation;
        Quaternion finalRot = shipTransform.rotation * Quaternion.Euler(0, 180, 0);
        

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;

            shipTransform.rotation = Quaternion.Slerp(startRot, finalRot, t);

            yield return null;
        }

        stationManager.ShowStationUI();

        shipTransform.rotation = finalRot;

        Debug.Log("CD");
       // yield return new WaitForSeconds(1);
        Debug.Log("ENTERED");
        stationAnimator.SetBool("shipEntered", true);


       //  yield return new WaitForSeconds(3);
        stationAnimator.SetBool("isShipMaintaining", true);

        if (cam) cam.SetDocked(true); // Enable docking camera mode
    }

    private void OnTriggerEnter(Collider other)
    {
        ShipController ship = other.GetComponent<ShipController>();

        if (ship != null)
        {
            Debug.Log("DOCKING!");

            foreach (ParticleSystem thruster in ship.thrusterEffects)
            {
                if (thruster.isPlaying)
                    thruster.Stop();
            }

            ship.enabled = false;
            StartCoroutine(DockShip(other.transform));
        }
    }

    private void Update()
    {
        if (isDocked && Input.GetKey(KeyCode.W))
        {
            exitKeyHoldTime += Time.deltaTime;
            if (exitKeyHoldTime >= 2f)
            {
                StartCoroutine(UndockShip());
                exitKeyHoldTime = 0f; // Reset hold timer
            }
        }
        else
        {
            exitKeyHoldTime = 0f;
        }
    }

    private IEnumerator UndockShip()
    {
        inventoryManager.UpdateUI();
        stationManager.HideStationUI();
        stationManager.victoryText.SetActive(false);
        if (cam) cam.SetDocked(false); // Disable docking camera mode


        if (dockedShip == null) yield break;
        stationAnimator.SetBool("isShipMaintaining", false);
        yield return new WaitForSeconds(stationAnimator.GetCurrentAnimatorStateInfo(0).length);
      // yield return new WaitForSeconds(3);
        stationAnimator.SetBool("shipEntered", false);

   //     Debug.Log("ENDER");
      //  stationAnimator.Play("ender",0,0);
      //  stationAnimator.Play("ender");

    //    Debug.Log("UNDOCKING!");
        isDocked = false;
        stationManager.transferComplete = false;

        Vector3 exitPos = dockedShip.position + transform.TransformDirection(approachOffset);

        float elapsed = 0f;
        Vector3 startPos = dockedShip.position;

        if (stationAnimator != null)
        {
          //  stationAnimator.Play("end");
        }

        if (stationCollider != null)
        {
            stationCollider.enabled = false; // Disable collider when undocking starts
        }

        while (elapsed < undockingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / undockingDuration;

            dockedShip.position = Vector3.Lerp(startPos, exitPos, t);

            yield return null;
        }

        dockedShip.position = exitPos;

        ShipController shipController = dockedShip.GetComponent<ShipController>();
        if (shipController != null)
        {
            shipController.enabled = true;
        }

        dockedShip = null;

        yield return new WaitForSeconds(colliderEnableDelay); // Wait before reactivating collider

        if (stationCollider != null)
        {
            stationCollider.enabled = true; // Reactivate collider
            Debug.Log("Station Collider Reactivated");
        }
    }
}
