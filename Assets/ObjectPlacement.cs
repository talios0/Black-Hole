using UnityEngine;

[RequireComponent(typeof(Gravity))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ObjectPlacement : MonoBehaviour
{
    private bool movable = false;

    private Gravity g;
    private SphereCollider col;

    private bool inCollision;
    private bool trajectory = false;
    private Vector3 initMousePos;
    private Vector3 finalMousePos;
    public int maxTrajectory = 5;
    public Vector2 trajectoryForce = new Vector2(100,1000);

    void Start() {
        g = GetComponent<Gravity>();
        col = GetComponent<SphereCollider>();
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
        if (Input.GetAxisRaw("Place") == 0) {
            trajectory = false;
            finalMousePos = CellestialManager.GetMousePos();
            Vector3 direction = (finalMousePos - initMousePos);
            float distance = Vector3.Distance(finalMousePos, initMousePos);
            Debug.Log(distance / maxTrajectory);
            if (distance/maxTrajectory >= 1) {
                distance = maxTrajectory;
            }

            SetMovable(false);
            transform.LookAt(initMousePos);
            GetComponent<Rigidbody>().AddForce(transform.forward * (distance/maxTrajectory) * trajectoryForce * GetComponent<Rigidbody>().mass);
            //Debug.Log(transform.forward * (distance / maxTrajectory) * trajectoryForce * GetComponent<Rigidbody>().mass);
            Debug.Log(GetComponent<Rigidbody>().velocity);
            
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
