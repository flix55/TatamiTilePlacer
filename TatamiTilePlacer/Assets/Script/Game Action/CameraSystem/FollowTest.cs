using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTest : MonoBehaviour
{
    public GameObject objectToMove;
    public GameObject objeftThatFolllow;
    [Space(10)]
    [Range(0.0f, 2f)]
    public float sensivity;
    public float smoothingMouvement = 100;
    public float secondActorThatFollow;
    public float glidingAmount = 3;

    float secondActorThatFollowStart;
    float verticalSpeed;
    float honrizontalSpeed;
    float v;
    float h;
    Vector3 velocity;
    Vector3 previousVelocity;
    Vector3 vecMouseVelocityPlus;
    Vector3 finalDistinationGliding;
    bool glidingMouvement;

    private void Start()
    {
        verticalSpeed = sensivity;
        honrizontalSpeed = sensivity;
        secondActorThatFollowStart = secondActorThatFollow;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            velocity = (objeftThatFolllow.transform.position - previousVelocity) / Time.deltaTime;

            v = verticalSpeed * Input.GetAxis("Mouse Y");
            h = honrizontalSpeed * Input.GetAxis("Mouse X");
            objectToMove.transform.position = (objectToMove.transform.position + new Vector3(h,v,0));

            previousVelocity = objeftThatFolllow.transform.position;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            secondActorThatFollow = secondActorThatFollowStart;
            glidingMouvement = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            secondActorThatFollow = smoothingMouvement;
            glidingMouvement = false;
            //stop any mouvement 
            objectToMove.transform.position = objeftThatFolllow.transform.position;
        }

        if (!glidingMouvement)
        {
            objeftThatFolllow.transform.position = Vector3.Lerp(objeftThatFolllow.transform.position, objectToMove.transform.position, Time.deltaTime * secondActorThatFollow);
        }

        else
        {
            finalDistinationGliding = (((velocity / 5) * glidingAmount) + objectToMove.transform.position);
            objeftThatFolllow.transform.position = Vector3.Lerp(objeftThatFolllow.transform.position, finalDistinationGliding, Time.deltaTime * secondActorThatFollow);
        }

        // -------------------does not work for now

        // or take the velocity of the object
        //vecMouseVelocityPlus = new Vector3(h, v,0) * glidingAmount;

        //clamp maxSpeed
        //objeftThatFolllow.transform.position = Vector3.ClampMagnitude(objeftThatFolllow.transform.position, 50f);
        //Debug.Log(objeftThatFolllow.transform.position.magnitude);

        //smooth line
        /*Vector3 offset = objeftThatFolllowClamp.transform.position - objectToMove.transform.position;
        objeftThatFolllowClamp.transform.position = Vector3.Lerp(objeftThatFolllowClamp.transform.position, objectToMove.transform.position, Time.deltaTime * secondActorThatFollow * 2);
        Debug.Log(offset);
        objeftThatFolllowClamp.transform.position = objectToMove.transform.position + Vector3.ClampMagnitude(offset, 100);*/
    }
}
