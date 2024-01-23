using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class RotateTowards : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Transform MainCamera;
  [SerializeField] Transform Head;
  [SerializeField] Transform Pointer;
 [SerializeField]  XROrigin xrOrigin;
  public InputActionProperty Activate;


//testing purposes
    void Update() { 
if(Activate.action.ReadValue<float>() > 0.1f)    
       RotateImmediately();
    }
public void matchRotation(){
 xrOrigin.MatchOriginUpCameraForward(Pointer.rotation * Vector3.up, Pointer.rotation * Vector3.forward);

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
