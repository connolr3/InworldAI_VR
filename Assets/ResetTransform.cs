using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
public class ResetTransform : MonoBehaviour
{
    public GameObject environment; // Reference to the environment object
    public Transform targetRotationMarker; // Target, which is a child of the environment
    public Transform MainCam; // Target, which is a child of the environment
    public Transform targetPositionMarker; // Target, which is a child of the environment
    public Transform customPivot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            alignPosition();
            //RotateEnvironmentTowardsTarget();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(ContinuousRotation());
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            shouldRotate = true;
        }


    }
    private void alignPosition() {
        Vector3 targetPosition = new Vector3(targetPositionMarker.position.x, transform.position.y, targetPositionMarker.position.z);
        Debug.Log(targetPosition);
        transform.position = targetPosition;

    }
    private bool shouldRotate = true;
    private void StartContinuousRotation()
    {
        shouldRotate = true;
        StartCoroutine(ContinuousRotation());
    }

    private void StopContinuousRotation()
    {
        shouldRotate = false;
    }



    private void RotateEnvironmentAroundPivot()
    {
        // Calculate the direction from the pivot to the target
        Vector3 directionToTarget = targetRotationMarker.position - customPivot.position;

        // Calculate the rotation angle based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Rotate the environment around the custom pivot
        environment.transform.RotateAround(customPivot.position, Vector3.up, targetRotation.eulerAngles.y);

        // Optionally, you can reset the environment's local rotation to avoid accumulation
        // environment.transform.localRotation = Quaternion.identity;
    }

    private bool IsTargetInFront()
    {

        float yRotationDifference = Mathf.Abs(MainCam.eulerAngles.y - targetRotationMarker.eulerAngles.y);
        Debug.Log(yRotationDifference);
        // Adjust the threshold angle based on your specific needs
        return yRotationDifference < 1f; // Change 1f to your desired threshold angle


      //  return Mathf.Abs(angle) < 1f; // Change 10f to your desired threshold angle
    }



    private IEnumerator ContinuousRotation()
    {
        while (shouldRotate)
        {
            // Rotate the environment around the custom pivot
            environment.transform.RotateAround(customPivot.position, Vector3.up, 0.1f);
            if (IsTargetInFront()) {
                shouldRotate = false;
                break;
            }
          
            // Optionally, you can yield to the next frame or adjust rotation speed
            yield return null;
        }
    }
}
