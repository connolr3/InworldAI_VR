using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class ActivateTeleportationRay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

//required if holding objects is enabled in your project
   // public InputActionProperty leftDeactivate;
  //  public InputActionProperty RightDeactivate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() { 
//only activate      
          leftTeleportation.SetActive(leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportation.SetActive(rightActivate.action.ReadValue<float>() > 0.1f);
        
        //required if holding objects is enabled in your project... otherwise use above
             //check that two values are at 0 to enable teleportation.. make sure that we are not holding an object in order to show ray
      //  leftTeleportation.SetActive(leftDeactivate.action.ReadValue<float>()==0&& leftActivate.action.ReadValue<float>()>0.1f);
       // rightTeleportation.SetActive(RightDeactivate.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
        


     //   if((leftActivate).GetType() is (Vector2))
     //   Debug.Log(leftActivate.action.ReadValue<Vector2>()[1]);
    }
}
