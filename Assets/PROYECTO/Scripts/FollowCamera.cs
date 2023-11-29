using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    [Header("Camaras")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Camera _panningCamera;
    [Space(5)]
    [Header("Objetivos/Targets")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _travelTarget;
    [Space(5)]
    [Header("Velocidades")]
    [SerializeField] private float _speed;
    [Space(5)]
    [Header("Suavizados")]
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _smoothSpeedFocal;
    [SerializeField] private float _smoothSpeedFocalRotation;
    [Space(5)]
    [Header("Rotaciones")]
    [SerializeField] private Quaternion _rot;
    [SerializeField] private Quaternion _rotation;

    [Space(12)]
    [Header("Offset")]
    [SerializeField] private Vector3 _offset;
    [Space(5)]
    [Header("Focal")]
    [SerializeField] private float _focalLensDef;
    [SerializeField] private float _focalLens = 150.0f;
    [Space(5)]
    [Header("Booleanas")]
    [SerializeField] private bool _tps = false;
    [SerializeField] private bool _travelCam = false;
    [SerializeField] private bool _panningCam = false;
    [Space(5)]
    [Header("Tiempos")]
    [SerializeField] private float t;
    [SerializeField] private float tf;
    [SerializeField] private float timeRot;
    [SerializeField] private float rotStep;
    // Start is called before the first frame update
    void Start()
    {
        _tps = false;
        _travelCam = false;
        _cam = Camera.main;

        
        _cam.usePhysicalProperties = true;
        _focalLensDef = _cam.focalLength;

        t = _focalLensDef;
        tf = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //PanoramicToTps();
        //TravelCamera();
        //PanningCamera();

        if (_tps) { PanoramicToTps(); }

        if (_travelCam) { TravelCamera(); }

        if (_panningCam)
        {
            Panning();
            /*tf += rotStep * Time.deltaTime;

            if (tf <= 101.0f)
            {
                _panningCamera.transform.Rotate(0.0f, -rotStep * Time.deltaTime, 0.0f);
            }*/
        }
    }
    //PRIVATES----
    private void TravelCamera()
    {
        Vector3 desiredPosition = _travelTarget.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;

        Quaternion desiredRotation = _rot;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, desiredRotation, _smoothSpeedFocalRotation / Time.deltaTime);

        transform.rotation = smoothRotation;
        if (t < _focalLens)
        {
            t += _smoothSpeedFocal;

            _cam.focalLength = t;
        }
    }

    private void PanoramicToTps()
    {
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;

        Quaternion desiredRotation = _rotation;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, desiredRotation, _smoothSpeed * Time.deltaTime);

        transform.rotation = smoothRotation;
        Camera.main.focalLength = _focalLensDef;
        if (t > _focalLensDef)
        {
            t -= _smoothSpeedFocal;

            _cam.focalLength = t;
        }
    }
    private void Panning()
    {
        tf += rotStep * Time.deltaTime;

        if (tf <= 101.0f)
        {
            _panningCamera.transform.Rotate(0.0f, -rotStep * Time.deltaTime, 0.0f);
        }
    }
    //PUBLICS-----
    public void TPSButton()
    {
        _cam.enabled = true;
        _tps = true;
        _travelCam = false;
        _panningCamera.enabled = false;
        _panningCam = false;
    }
    public void TravellingButton()
    {
        _cam.enabled = true;
        _tps = false;
        _travelCam = true;
        _panningCamera.enabled = false;
        _panningCam = false;
    }
    public void PanningButton()
    {
        _panningCam = true;
        _cam.enabled = false;
        _panningCamera.enabled = true;
    }

    public void DefaultCamera()
    {
        _panningCam = false;
        _cam.enabled = true;
        _panningCamera.enabled = false;
    }
}
