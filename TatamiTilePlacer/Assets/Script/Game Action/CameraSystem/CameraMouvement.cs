using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMouvement : MonoBehaviour
{
    [Header("Camera Movement And Setting")]
    public GameObject objectToRotate;
    public GameObject objectToRotateThatFollow;
    [Range(0.0f, 10f)]
    public float sensivity;
    public float scrollSpeed;
    public Vector2 scrollWheellDistanceMinMax = new Vector2(-5, 20);
    public Transform _cam;
    private float _distance = 10f;
    private float _verticalSpeed;
    private float _honrizontalSpeed;
    private float _v;
    private float _h;
    [HideInInspector] public bool direction;
    [Space(10)]
    [Header("DeadZone Settings")]
    public float radiusMaximum = 10;
    private bool _canMove;
    private bool _stopDeadZone;
    private Vector3 _deadZone;

    [Space(10)]
    [Header("Rotation Stuff")]
    public float smothingTheRotation = 20;
    public float smothingRotationMouvement = 50;
    private float _oldSmothingTheRotation;
    private Quaternion _finalDistinationGliding;

    private void Start()
    {
        _verticalSpeed = sensivity;
        _honrizontalSpeed = sensivity;
        _oldSmothingTheRotation = smothingTheRotation;
        _distance = 20;
    }

    private void Update()
    {
        CameraScroll();
        DeadZone();
        InputMouseDrag();
        Lerp();
        FindTheDirection();
        FlipCamera();
    }

    private void FlipCamera()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            FlipCameraCalculation();
        }
    }

    private void InputMouseDrag()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StopMomentum(); 
            smothingTheRotation = smothingRotationMouvement;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            smothingTheRotation = _oldSmothingTheRotation;
            _canMove = false;
            _stopDeadZone = false;
        }

        if (!Input.GetKey(KeyCode.Mouse0) || _canMove != true) return;
        _v = -_verticalSpeed * Input.GetAxis("Mouse Y");
        _h = _honrizontalSpeed * Input.GetAxis("Mouse X");
        objectToRotate.transform.Rotate(_v, 0, 0);
            
        if (direction)
        {
            objectToRotate.transform.Rotate(0, -_h, 0, Space.World);
        }
        else
        {
            objectToRotate.transform.Rotate(0, _h, 0, Space.World);
        }
    }

    private void StopMomentum()
    {
        var eulerAngles = objectToRotateThatFollow.transform.eulerAngles;
        objectToRotate.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, !direction ? 0 : 180);
    }

    private void FlipCameraCalculation()
    {
        var eulerAngles = objectToRotate.transform.eulerAngles;
        if (!direction)
        {
            if (objectToRotate.transform.localEulerAngles.x > 300)
            {
                objectToRotate.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 180);
                direction = true;
            }
            else
            {
                objectToRotate.transform.rotation = Quaternion.Euler(eulerAngles.x * -1, eulerAngles.y, 180);
                direction = true;
            }
        }
        else
        {
            if (objectToRotate.transform.localEulerAngles.x < 70)
            {
                objectToRotate.transform.rotation = Quaternion.Euler(eulerAngles.x , eulerAngles.y, 0);
                direction = false;
            }
            else
            {
                objectToRotate.transform.rotation = Quaternion.Euler(eulerAngles.x * -1, eulerAngles.y, 0);
                direction = false;
            }
        }
    }

    private void FindTheDirection()
    {
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;
        var eulerAngles = objectToRotate.transform.eulerAngles;
        direction = eulerAngles.z >= 170 && eulerAngles.z <= 190;
    }

    private void Lerp()
    {
        _finalDistinationGliding = objectToRotate.transform.rotation;
        objectToRotateThatFollow.transform.rotation = Quaternion.Slerp(objectToRotateThatFollow.transform.rotation, _finalDistinationGliding, Time.deltaTime * smothingTheRotation);
    }

    private void DeadZone()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            _deadZone = Input.mousePosition;

        if (!Input.GetKey(KeyCode.Mouse0)) return;
        Vector2 radius = Input.mousePosition - _deadZone;
        if (!(radius.magnitude > radiusMaximum) || _stopDeadZone != false) return;
        _canMove = true;
        _stopDeadZone = true;
    }

    private void CameraScroll()
    {
        _distance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        _distance = Mathf.Clamp(_distance, scrollWheellDistanceMinMax.x, scrollWheellDistanceMinMax.y);
        _cam.transform.localPosition = new Vector3(0, 0, _distance - 30);
    }
}