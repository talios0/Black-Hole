using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour
{
    public float zoomSpeed;

    private Rigidbody rb;
    public Vector2 zoom;

    public Vector3 previousMousePos;
    public Vector3 currentMousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousMousePos = new Vector3(0, 0, 0);
    }

    public void FixedUpdate()
    {
        Zoom();
        ClampZoom();
        Move();
    }

    private void Zoom()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y -  zoomSpeed * Input.GetAxisRaw("Zoom"), transform.position.z);
    }

    private void ClampZoom() {
        float clamped = Mathf.Clamp(transform.position.y, zoom.x, zoom.y);
        transform.position = new Vector3(transform.position.x, clamped, transform.position.z);
    }

    private void Move() {
        //if (Input.GetAxisRaw("Move") != 0) {
            //currentMousePos = CellestialManager.GetMousePos();
            //ransform.position = new Vector3(currentMousePos.x, 0, currentMousePos.y);
            //transform.position = CellestialManager.GetMousePos();transform.position = Vector3.Lerp(previousMousePos, currentMousePos, 0.5f);
            //t
            //previousMousePos = currentMousePos;
        //}
    }

}
