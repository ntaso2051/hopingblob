using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public static PlayerCamera Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private Quaternion vRotation;
    [SerializeField] private Quaternion _hRotation;
    public Quaternion hRotation
    {
        get
        {
            return _hRotation;
        }
    }
    [SerializeField] private float turnSpeed;

    private bool isCaputured = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void SetUpPlayerCamera()
    {
        target = PNetworkManager.Instance.localPlayer.transform;
        vRotation = Quaternion.Euler(30, 0, 0);
        _hRotation = target.transform.rotation;

        this.transform.rotation = _hRotation * vRotation;
        this.transform.position = target.position - transform.rotation * Vector3.forward * distance;
        isCaputured = true;
    }

    public void SetUpGodCamera()
    {
        vRotation = Quaternion.Euler(90, 0, 0);
        this.transform.position = new Vector3(0.0f, 14.0f, 0.0f);
        this.transform.rotation = vRotation;
        isCaputured = false;
    }

    private void LateUpdate()
    {
        if (isCaputured)
        {
            if (Input.GetMouseButton(0)) _hRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * turnSpeed, 0);
            this.transform.rotation = _hRotation * vRotation;
            this.transform.position = Vector3.Slerp(this.transform.position, target.position - this.transform.rotation * Vector3.forward * distance, turnSpeed);
        }
    }
}
