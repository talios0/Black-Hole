using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpaceResources : MonoBehaviour
{
    [Header("Points")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI ppmText;

    public int points;
    public int ppm;

    [Header("Resource Settings")]
    public string[] resourceName;
    public int[] cost;
    public Button[] buttons;

    [Header("Planets")]
    public GameObject basePlanet;
    public Material[] planetMats;
    public Vector2 planetSize;
    public Vector2 planetMass;

    [Header("Moons")]
    public GameObject baseMoon;
    public Material[] moonMats;
    public Vector2 moonSize;
    public Vector2 moonMass;

    [Header("Meteors")]
    public GameObject baseMeteor;
    public Material[] meteorMats;

    [Header("Gravity Manipulator")]
    public int gravityModifier;

    [Header("Mining Probe")]
    public GameObject baseProbe;
    public Material[] probeMats;
    public Vector2 probeMass;

    private bool selectingObjects;
    private Gravity[] objects;
    private bool trajectory;

    private Dictionary<Gravity, Gravity> assignedObjects;

    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(resourceName[i] + " - " + cost[i]);
            //buttons[i].onClick.AddListener(delegate { this.BuyResource(i); });
        }
        InvokeRepeating("UpdatePoints", 0, 1);
    }

    void Update()
    {
        pointsText.SetText(points.ToString());
        ppmText.SetText(ppm.ToString());
        for (int i = 0; i < cost.Length; i++)
        {
            if (cost[i] <= points && !buttons[i].interactable && !CellestialManager.movingObj && !selectingObjects)
                buttons[i].interactable = true;
            else if ((cost[i] > points && buttons[i].interactable) || CellestialManager.movingObj || selectingObjects)
                buttons[i].interactable = false;
        }
        if (selectingObjects) {
            SelectObjects();
        }
    }

    public void UpdatePoints()
    {
        points += ppm;
    }

    public void BuyResource(int item)
    {
        if (points < cost[item])
            return;

        points -= cost[item];

        switch (item)
        {
            case 0:
                Moon();
                break;
            case 1:
                Planet();
                break;
            case 2:
                MeteorShower();
                break;
            case 3:
                GravitationManipulator();
                break;
            case 4:
                MiningProbe();
                break;
            default:
                Debug.LogWarning("Requested item not found.");
                break;
        }
    }

    private void Planet()
    {
        Material mat = planetMats[Random.Range(0, planetMats.Length)];
        float size = Random.Range(planetSize.x, planetSize.y);
        float mass = Random.Range(planetMass.x, planetMass.y);

        GameObject obj = Instantiate(basePlanet);
        obj.GetComponent<ObjectPlacement>().SetMovable(true);
        obj.GetComponent<SphereCollider>().isTrigger = true;
        obj.GetComponent<Gravity>().enabled = false;
        obj.GetComponent<MeshRenderer>().material = mat;
        obj.transform.localScale = new Vector3(size, size, size);
        obj.GetComponent<Rigidbody>().mass = mass;
    }

    private void Moon()
    {
        Material mat = moonMats[Random.Range(0, planetMats.Length)];
        float size = Random.Range(moonSize.x, moonSize.y);
        float mass = Random.Range(moonMass.x, moonMass.y);

        GameObject obj = Instantiate(baseMoon);
        obj.GetComponent<ObjectPlacement>().SetMovable(true);
        obj.GetComponent<SphereCollider>().isTrigger = true;
        obj.GetComponent<Gravity>().enabled = false;
        obj.GetComponent<MeshRenderer>().material = mat;
        obj.transform.localScale = new Vector3(size, size, size);
        obj.GetComponent<Rigidbody>().mass = mass;
    }

    private void MeteorShower()
    {
        GameObject obj = Instantiate(baseMeteor);
        foreach (Transform c in obj.transform) {
            c.GetComponent<MeshRenderer>().material = moonMats[Random.Range(0, moonMats.Length)];
        }
        obj.GetComponent<ObjectPlacement>().SetMovable(true);
    }

    private void GravitationManipulator()
    {
        selectingObjects = true;
        objects = new Gravity[2];
    }

    private void MiningProbe()
    {

    }

    private void SelectObjects() {
        // Overlay
    }


    public void Trajectory() {

    }

    public void ObjectSelected(Gravity g) {
        if (selectingObjects) {
            if (objects[0] == null)
                objects[0] = g;
            else if (objects[1] == null && objects[0] != g)
            {
                objects[1] = g;
                selectingObjects = false;
                objects[0].SetManipulate(objects[1], gravityModifier);
                //objects[1].SetManipulate(objects[0], gravityModifier);
            }
        }
    }


}
