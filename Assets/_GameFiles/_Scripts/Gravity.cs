using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    public static float gravitationalConstant = 0.05f;
    public bool isSun;
    public bool isMoon;
    public Gravity linkedPlanet;

    //private static List<Gravity> masses;
    private Rigidbody rb;
    private Gravity sun;
    private static SpaceResources resourceManager;
    private float mass;
    private bool waitingForSun = false;

    private Gravity manipulate;
    private float manipulateGravity;
    private bool gravityManipulation = false;

    public bool useStartForce;

    void Start()
    {
        if (resourceManager == null)
            resourceManager = GameObject.Find("ResourceManager").GetComponent<SpaceResources>();

        if (CellestialManager.masses == null)
            CellestialManager.masses = new List<Gravity>();

        if (transform.tag == "Sun")
            isSun = true;
        else if (transform.tag == "Moon")
            isMoon = true;

        else if (!isMoon)
            linkedPlanet = null;


        // RIGIDBODY SETTINGS
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // MASS SETTINGS
        mass = rb.mass;
        CellestialManager.masses.Add(this);

        if (CheckSun())
            InitialForce();
    }

    private bool CheckSun()
    {
        foreach (Gravity g in CellestialManager.masses)
        {
            if (g.IsSun())
            {
                sun = g;
                break;
            }
        }

        if (sun == null)
        {
            waitingForSun = true;
            return false;
        }
        else
        {
            waitingForSun = false;
            return true;
        }
    }

    private void InitialForce()
    {
        if (useStartForce)
        {
            if (isSun)
                return;

            transform.LookAt(sun.transform);

            float velocity = Mathf.Pow((gravitationalConstant * sun.mass) / Vector3.Distance(transform.position, sun.transform.position), 0.5f);

            Vector3 perpendicular = -transform.right;
            Vector3 direction = new Vector3(transform.position.x - sun.transform.position.x, 0, transform.position.z - sun.transform.position.z).normalized;
            rb.velocity += velocity * perpendicular;
        }
        /*foreach (Gravity g in CellestialManager.masses)
        {
            if (g != this)
            {
                //float force = (gravitationalConstant * mass * g.mass) / (Mathf.Pow(Vector3.Distance(transform.position, g.transform.position), 2));
                
                float velocity = Mathf.Pow((gravitationalConstant * g.mass) / Vector3.Distance(transform.position, g.transform.position), 0.5f);

                Vector3 perpendicular;
                if (g == sun)
                {
                    perpendicular = -transform.right;
                }
                else {
                    Vector3 direction = new Vector3(transform.position.x - g.transform.position.x, 0, transform.position.z - g.transform.position.z).normalized;
                    perpendicular = new Vector3(direction.y, 0, -direction.x);

                }
                rb.velocity += velocity * perpendicular;
            }
        }*/

        /*Gravity anchor;
        if (isMoon)
            anchor = linkedPlanet;
        else
            anchor = sun;

        float velocity = Mathf.Pow((gravitationalConstant * anchor.mass) / Vector3.Distance(transform.position, anchor.transform.position), 0.5f);
        //if (isMoon)
        //velocity += Mathf.Pow((gravitationalConstant * sun.GetComponent<Gravity>().mass) / Vector3.Distance(transform.position, sun.transform.position), 0.5f);
        Vector3 direction = new Vector3(transform.position.x - anchor.transform.position.x, 0, transform.position.z - anchor.transform.position.z).normalized;
        Vector3 perpendicular = new Vector3(direction.y, 0, -direction.x);
        if (isMoon)
            velocity += Mathf.Pow((gravitationalConstant * sun.mass) / Vector3.Distance(transform.position, sun.transform.position), 0.5f);
        rb.velocity = velocity * perpendicular;*/
    }

    void FixedUpdate()
    {
        if (waitingForSun)
            if (CheckSun())
                InitialForce();

        CellestialForces();
    }



    private void CellestialForces()
    {
        transform.LookAt(sun.transform);
        foreach (Gravity g in CellestialManager.masses)
        {
            if (g != this)
            {
                float multiplier = 1;
                if (g == manipulate && gravityManipulation) {
                    multiplier = manipulateGravity;
                    //Debug.Log("MANIPULATING, " + multiplier);
                }

                float force = (gravitationalConstant * multiplier * mass * g.mass) / (Mathf.Pow(Vector3.Distance(transform.position, g.transform.position), 2));

                Vector3 direction = new Vector3(transform.position.x - g.transform.position.x, 0, transform.position.z - g.transform.position.z).normalized;
                rb.AddForce(force * -direction);
                
            }
        }
    }

    public void SetManipulate(Gravity g, float gravityModifier)
    {
        manipulate = g;
        manipulateGravity = gravityModifier;
        gravityManipulation = true;
    }

    public void AddMass(float mass) {
        this.mass += mass;
    }

    public float GetMass()
    {
        return mass;
    }

    public bool IsSun()
    {
        return isSun;
    }

    void OnDisable()
    {
        CellestialManager.masses.Remove(this);
    }

    void OnMouseOver() {
        if (Input.GetAxisRaw("Place") != 0) {
            resourceManager.ObjectSelected(this);
        }
    }
}