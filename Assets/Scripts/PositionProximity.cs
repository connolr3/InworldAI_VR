using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PositionProximity : MonoBehaviour
{
   [SerializeField] Transform Target;
    [SerializeField] Transform MainCamera;
  [SerializeField] Transform Head;
    [SerializeField] Transform sphere;
[SerializeField] float ProximityDistance ;




  public InputActionProperty Activate;

//testing purposes
 /*   void Update() { 
if(Activate.action.ReadValue<float>() > 0.1f)    
       PositionTowards();
    }*/

 public void SetPositionTowards(){
       Debug.Log("Position to target immediately");
    // Gets a vector that points from the target to player position
var heading = MainCamera.position-Target.position;

var distance = heading.magnitude;
var direction = heading / distance; // This is now the normalized direction.

var position = Target.position + direction*ProximityDistance;
position.y = transform.position.y;//we don't want  to change height
transform.position = position;
sphere.position = position;
 }


 public Vector3 GetProximityDistance(){
        // Gets a vector that points from the target to player position
var heading = MainCamera.position-Target.position;

var distance = heading.magnitude;
var direction = heading / distance; // This is now the normalized direction.

var position = Target.position + direction*ProximityDistance;
position.y = transform.position.y;//we don't want  to change height
return position;
 }

 public void RotateImmediately()
{
    Debug.Log("Rotate to target immediately");

    // Calculate the direction from the origin to the target
    Vector3 directionToTarget = Target.position - MainCamera.position;

    // Discount the local rotation of MainCamera
    directionToTarget = Quaternion.Inverse(Head.rotation) * directionToTarget;

    // Create the rotation based on the direction
    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

    // Extract only the Y-axis rotation
    float yRotation = lookRotation.eulerAngles.y;
    Quaternion yRotationQuaternion = Quaternion.Euler(0f, yRotation, 0f);

    // Apply the Y-axis rotation to the xrOrigin
    transform.rotation = yRotationQuaternion;
}

 


}
