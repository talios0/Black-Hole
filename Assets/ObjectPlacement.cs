using UnityEngine;

[RequireComponent(typeof(Gravity))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ObjectPlacement : MonoBehaviour
{
    private bool movable = false;

    private Gravity g;
    private SphereCollider col;
    private Rigidbody rb;

    private bool inCollision;
    private bool trajectory = false;
    private Vector3 initMousePos;
    private Vector3 finalMousePos;
    private int maxTrajectory = 125;
    private Vector2 trajectoryForce = new Vector2(0,500);
    private float maxTrajectoryForce = 7000;

    public GameObject place;

    void Start() {
        g = GetComponent<Gravity>();
        col = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        inCollision = false;
    }

    void Update() {
        if (!movable)
            return;
        FollowMouse();
        CheckInput();
        if (trajectory) {
            Trajectory();
        }
    }

    private void FollowMouse() {
        //transform.position = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        transform.position = CellestialManager.GetMousePos();
    }

    private void CheckInput() {
        if (Input.GetAxisRaw("Place") != 0 && !inCollision && !trajectory) {

            initMousePos = CellestialManager.GetMousePos();
            trajectory = true;
        }
    }

    public void SetMovable(bool _movable) {
        movable = _movable;
        if (movable)
        {
            CellestialManager.movingObj = true;
            try
            {
                //col.enabled = false;
                col.isTrigger = true;
                g.enabled = false;
            }
            catch {
                Debug.LogWarning("Unity execution order error");
            }
        }
        else {
            CellestialManager.movingObj = false;
            col.isTrigger = false;
            g.enabled = true;
        }


    }


    private void Trajectory() {
        Debug.DrawRay(initMousePos, CellestialManager.GetMousePos());
        if (Input.GetAxisRaw("Place") == 0)
        {
            trajectory = false;
            finalMousePos = CellestialManager.GetMousePos();
            Vector3 direction = (finalMousePos - initMousePos);
            float distance = Vector3.Distance(finalMousePos, initMousePos);
            Debug.Log(distance);
            Debug.Log(distance / maxTrajectory);
            if (distance / maxTrajectory >= 1)
            {
                distance = maxTrajectory;
            }
            SetMovable(false);
            transform.LookAt(initMousePos);
            rb.AddForce(transform.forward * (distance / maxTrajectory) * maxTrajectoryForce, ForceMode.Acceleration);
            //Debug.Log(transform.forward * (distance / maxTrajectory) * trajectoryForce * GetComponent<Rigidbody>().mass);
            //Debug.Log("VELOCITY: " + rb.velocity);
            //Debug.Log("FORCE: " + transform.forward * (distance / maxTrajectory) * maxTrajectoryForce * rb.mass);
        }
    }

    void OnTriggerEnter()
    {
        inCollision = true;
    }

    void OnTriggerExit() {
        inCollision = false;
    }
}
