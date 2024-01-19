using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Inworld;
using Inworld.Entities;
using Unity.VisualScripting;

public class TeleportbyRaycast : MonoBehaviour
{
    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;
    public InworldController controller;
        public float proximityDistance = 1.5f;
      //public Transform [] targets ;

    //  InworldCharacter is the class representing your characters
    [SerializeField] InworldCharacter currentCharacter;


    [SerializeField] Transform MainCamera;
  [SerializeField] Transform Head;

  private bool teleportationTriggered = false;
    private Transform nearestTarget;
  public enum TeleportType
    {
        Standard,
        ImmediateRotation,
        ImmediateProximityFix,
         ImmediateRotationProximity
        // Add more teleport types as needed
    }


    // Public variable to choose teleport type in the inspector
    public TeleportType selectedTeleportType = TeleportType.Standard;


   

 void Update()
    {
        if (leftActivate.action.ReadValue<float>() > 0.1f || rightActivate.action.ReadValue<float>() > 0.1f)
        {
            if (!teleportationTriggered)
           {
                teleportationTriggered = true;
                       CheckRaycastHit(leftRayInteractor, leftActivate);
        CheckRaycastHit(rightRayInteractor, rightActivate);
            }
        }
        else
        {
            teleportationTriggered = false;
        }
    }

  
    void CheckRaycastHit(XRRayInteractor rayInteractor, InputActionProperty activationAction)
    {
        // Check if the activation action is triggered
        if (activationAction.action.ReadValue<float>() > 0.1f)
        {
            // Try to get hit information from the ray interactor
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Ray hit something
               // Debug.Log("Hit object: " + hit.collider.gameObject.name);
              //  Debug.Log("Hit point: " + hit.point);
            if(hit.collider.gameObject.tag=="Teleport"){
                var position = hit.point;
                position.y = this.transform.position.y;
                this.transform.position = position;
                    setCurrentCharacterNull();


            }

            else if(hit.collider.gameObject.tag=="NPCZone")
            {
                    nearestTarget= hit.collider.transform.GetChild(0);
                    //     InworldController.CurrentCharacter = “jessica”;
                    setCurrentCharacter(hit.collider.gameObject);
                   // InworldController.CurrentCharacter = currentCharacter;


                  //  Debug.Log(nearestTarget.name);
                    var hitPosition = hit.point;
                hitPosition.y = this.transform.position.y;
                    switch (selectedTeleportType)
                    {
                        case TeleportType.Standard:
                            Debug.Log("Selected Teleport Type: Standard");
                            var position = hit.point;
                            position.y = this.transform.position.y;
                            this.transform.position = position;
                            break;
                        case TeleportType.ImmediateRotation:
                            Debug.Log("Selected Teleport Type: Quick");
                            this.transform.position = hitPosition;
                            this.transform.rotation = GetRotationtoTarget(nearestTarget);
                            break;
                        case TeleportType.ImmediateProximityFix:
                            Debug.Log("Selected Teleport Type: ImmediateProximityFix");
                            this.transform.position = GetProximityDistance(hitPosition, nearestTarget);
                            break;
                        case TeleportType.ImmediateRotationProximity:
                            Debug.Log("Selected Teleport Type: Immediate Both");
                            this.transform.position = GetProximityDistance(hitPosition, nearestTarget);
                            this.transform.rotation = GetRotationtoTarget(nearestTarget);
                            break;
                            // Add cases for additional teleport types as needed
                    }
                }
            }
        }
    }
    public void setCurrentCharacterNull() {
        InworldController.CurrentCharacter = null;
    }
    public void setCurrentCharacter(GameObject parentTransform) {
        InworldCharacter foundInworldCharacter = null;

        // Loop through all child objects
        foreach (Transform child in parentTransform.transform)
        {
            // Check if the child has the InworldCharacter component
            InworldCharacter inworldCharacterComponent = child.GetComponent<InworldCharacter>();
            Debug.Log("setting current character to:" + child.name);
            if (inworldCharacterComponent != null)
            {
                // Found a child with the InworldCharacter component
                foundInworldCharacter = inworldCharacterComponent;
                break; // Exit the loop since we found what we need
            }
        }

        if (foundInworldCharacter != null)
        {
            // Do something with the found InworldCharacter
          
            InworldController.CurrentCharacter = foundInworldCharacter;
        }
        else
        {
            // Handle the case where no child with InworldCharacter component was found
            Debug.LogError("No child with InworldCharacter component found.");
        }
    }
    public Vector3 GetProximityDistance(Vector3 hitPosition, Transform targetTransform)
    {
        // Gets a vector that points from the target to hit point position
        var heading = hitPosition - targetTransform.position;

        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        var position = targetTransform.position + direction * proximityDistance;
        position.y = transform.position.y; // we don't want to change height
        return position;
    }

    public Quaternion GetRotationtoTarget(Transform targetTransform)
    {
        Debug.Log("Rotate to target immediately");
        // Assuming the current object's position is the reference position

        // Calculate the direction from the origin to the target
        Vector3 directionToTarget = targetTransform.position - MainCamera.position;

        // Discount the local rotation of MainCamera
        directionToTarget = Quaternion.Inverse(Head.rotation) * directionToTarget;

        // Create the rotation based on the direction
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        // Extract only the Y-axis rotation
        float yRotation = lookRotation.eulerAngles.y;
        Quaternion yRotationQuaternion = Quaternion.Euler(0f, yRotation, 0f);

        // Apply the Y-axis rotation to the xrOrigin
        return yRotationQuaternion;
    }


}