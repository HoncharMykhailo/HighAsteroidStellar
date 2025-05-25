using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float smoothSpeed = 5f;
    private float currentsmoothSpeed = 5f;
    public float rotationSpeed = 3f; 
    private bool isDocked = false; 
    private float currentYaw = 0f;
    private float currentPitch = 0f;

    public float zoomSpeed = 2f; 
    public float minZoom = -2f;  
    public float maxZoom = -10f;

    public float maxFollowDistance = 10f; // Max distance allowed from the ship
    public float maxRotationDeviation = 30f; // Max degrees allowed from ship's rotation

    void FixedUpdate()
    {/*
        if (target == null) return;

        // Handle zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            offset.z = Mathf.Clamp(offset.z + scrollInput * zoomSpeed, maxZoom, minZoom);
        }

        if (isDocked) 
        {
            if (Input.GetMouseButton(2))
            {
                currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
                currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentPitch = Mathf.Clamp(currentPitch, -45f, 45f);
            }

            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 desiredPosition = target.position + rotation * offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
        else 
        {
            Quaternion targetRotation = target.rotation;
            Vector3 desiredPosition = target.position + targetRotation * offset;
            
            float distanceToTarget = Vector3.Distance(transform.position, desiredPosition);
            float dynamicSpeed = Mathf.Lerp(smoothSpeed, smoothSpeed * 5f, distanceToTarget / maxFollowDistance);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, dynamicSpeed * Time.deltaTime);

            float rotationDifference = Quaternion.Angle(transform.rotation, target.rotation);
            if (rotationDifference > maxRotationDeviation)
            {
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, maxRotationDeviation * Time.deltaTime);
            }
            else
            {
           //     transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, dynamicSpeed * Time.deltaTime);
            }

            if (distanceToTarget > maxFollowDistance)
            {
           //     transform.position = desiredPosition;
            }
        }

        */



         if (target == null) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            offset.z = Mathf.Clamp(offset.z + scrollInput * zoomSpeed, maxZoom, minZoom);
        }

        
        if (isDocked) 
        {
            
            if(Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxis("Mouse X");
                currentYaw += mouseX * rotationSpeed; 
                float mouseY = Input.GetAxis("Mouse Y");
                currentPitch -= mouseY * rotationSpeed; 

            }
            
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 desiredPosition = target.position + rotation * offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, currentsmoothSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            if(Input.GetKey(KeyCode.LeftControl)) {currentsmoothSpeed = 20;}
            else {currentsmoothSpeed = smoothSpeed;}

            Vector3 desiredPosition = target.position + target.TransformDirection(offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, currentsmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, currentsmoothSpeed * Time.deltaTime);

            
        }
    }

    public void SetDocked(bool docked)
    {
        isDocked = docked;
    }
}
