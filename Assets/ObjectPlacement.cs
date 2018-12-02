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
    }

    private void FollowMouse() {
        //transform.position = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        Vector3 pos = Input.mousePosition;
        pos.z = Camera.main.transform.position.y;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

    private void CheckInput() {
        if (Input.GetAxisRaw("Place") != 0 && !inCollision) {
            SetMovable(false);
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

    void OnTriggerEnter()
    {
        inCollision = true;
    }

    void OnTriggerExit() {
        inCollision = false;
    }
}
