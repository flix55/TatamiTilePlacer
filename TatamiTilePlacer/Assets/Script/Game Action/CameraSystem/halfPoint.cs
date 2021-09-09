using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halfPoint : MonoBehaviour {

	[Header("Asign Objects")]
    public GameObject player1;
    public GameObject player2;
    public Transform startPosiiton;

    [Header("Ajustable Variables")]
    [Space(10)]
    [Range(0, 1)]
    public float amount;
    public Vector2 adusjtableHeight;

    CameraMouvement cameraMouvement;
    Vector3 pos1;
    Vector3 pos2;
    Vector3 posStart;
    Vector3 midPointPosPlayer;
    Camera cam;

    public GameObject midPointObject;

    private void Start()
    {
        cameraMouvement = FindObjectOfType<CameraMouvement>();
        cam = Camera.main;
        posStart = startPosiiton.position;
    }

    void Update () {

        if(player1 != null || player2 != null || midPointObject != null)
        {
            if (midPointObject)
            {
                pos2 = player2.transform.position;
                pos1 = player1.transform.position;

                midPointPosPlayer = ((pos1 + pos2) / 2);
                Vector3 midPointPosPlayerWithHeightChange = new Vector3 (midPointPosPlayer.x, midPointPosPlayer.y, midPointPosPlayer.z);
                Vector3 newPos = new Vector3(startPosiiton.position.x, startPosiiton.position.y, startPosiiton.position.z);
                Vector2 posToGo = new Vector3(adusjtableHeight.x + posStart.y, adusjtableHeight.y + posStart.y);
                if (cameraMouvement.direction)
                {
                    newPos.y = posToGo.x;
                }
                else
                {
                    newPos.y = posToGo.y;
                }
                startPosiiton.position = newPos;
                Vector3 finalPosition = ((midPointPosPlayer * amount) / 2) + startPosiiton.position;
                midPointObject.transform.position = Vector3.Lerp(midPointObject.transform.position, finalPosition, Time.deltaTime * 5);
            }
        }
    }
}
