using UnityEngine;

public class Absorbption : MonoBehaviour
{
    private Rigidbody rb;
    private Gravity gravity;
    private Animator anim;
    public float sizeIncrease;
    public float maxSize;

    private float initMass;
    private float finalMass;

    private float Cmass;

    private bool blackHole = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        gravity = GetComponent<Gravity>();
        anim = GetComponent<Animator>();
        initMass = gravity.GetMass();
        finalMass = initMass + maxSize;
        Cmass = initMass;

    }

    void OnCollisionEnter(Collision col) {
            float mass = col.gameObject.GetComponent<Gravity>().GetMass();
            Destroy(col.gameObject);
            rb.mass += mass;
            gravity.AddMass(mass);
            transform.localScale = new Vector3(transform.localScale.x + sizeIncrease, transform.localScale.y + sizeIncrease, transform.localScale.z + sizeIncrease);
            SpaceResources.ppm++;
        Cmass += sizeIncrease;
        if (finalMass <= Cmass && !blackHole) {
            // ANIMATION
            blackHole = true;
            rb.mass += 1500;
            gravity.AddMass(1500);
            SpaceResources.ppm += 100;
            anim.Play("Blackhole");
        }
    }
}
