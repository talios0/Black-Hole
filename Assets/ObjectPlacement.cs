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

    public bool isMeteor;

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
                if (isMeteor)
                {
                    foreach (Transform c in transform) {
                        c.GetComponent<SphereCollider>().isTrigger = true;
                        c.GetComponent<Gravity>().enabled = false;
                    }
                }
                else
                {
                    //col.enabled = false;
                    col.isTrigger = true;
                    g.enabled = false;
                }
            }
            catch {
                Debug.LogWarning("Unity execution order error");
            }
        }
        else {
            CellestialManager.movingObj = false;
            if (isMeteor)
            {
                foreach (Transform c in transform)
                {
                    c.GetComponent<SphereCollider>().isTrigger = false;
                    c.GetComponent<Gravity>().enabled = true;
                }
            }
            else
            {
                col.isTrigger = false;
                g.enabled = true;
            }
        }
    }


    private void Trajectory() {
        Debug.DrawRay(initMousePos, CellestialManager.GetMousePos());
        if (Input.GetAxisRaw("Place") == 0)
        {
            trajectory = false;
            finalMousePos = CellestialManager.GetMousePos();
            float distance = Vector3.Distance(finalMousePos, initMousePos);
            if (distance / maxTrajectory >= 1)
            {
                distance = maxTrajectory;
            }
            SetMovable(false);
            transform.LookAt(initMousePos);
            rb.AddForce(transform.forward * (distance / maxTrajectory) * maxTrajectoryForce, ForceMode.Acceleration);
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
