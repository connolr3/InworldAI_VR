using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSmoothly : MonoBehaviour
{
    public Animator anim;
  //  public CharacterController player;
    public Transform Player;
    public string TriggerTag = "PlayerHands";
    Coroutine smoothMove = null;

    // Use this for initialization
    void Start()
    {
        Debug.Log("starting script");
      //  player = GameObject.FindObjectOfType<CharacterController>();
        anim = GetComponent<Animator>();

       // Vector3 lookAt = Player.position;
       // lookAt.y = transform.position.y;
       // smoothMove = StartCoroutine(LookAtTargetSmoothly(transform, lookAt, 2f));

    }
    private Boolean turning = false;
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance < 2f && !turning)
        {
            turning = true;
            LookSmoothly();

        }
        else {
            turning = false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == TriggerTag)
        {
            Debug.Log("Hello THE PLAYER HAS ARRIVED");
            anim.Play("Hello");
            LookSmoothly();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == TriggerTag)
        {
            anim.Play("Idle");
        }
    }

    public void LookSmoothly()
    {
        float time = 1f;

        Vector3 lookAt = Player.position;
        lookAt.y = transform.position.y;

        //Start new look-at coroutine
        if (smoothMove == null)
            smoothMove = StartCoroutine(LookAtTargetSmoothly(transform, lookAt, time));
        else
        {
            //Stop old one then start new one
            StopCoroutine(smoothMove);
            smoothMove = StartCoroutine(LookAtTargetSmoothly(transform, lookAt, time));
        }
    }

    IEnumerator LookAtTargetSmoothly(Transform objectToMove, Vector3 worldPosition, float duration)
    {
        Quaternion currentRot = objectToMove.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPosition -
            objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToMove.rotation =
                Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }
}
