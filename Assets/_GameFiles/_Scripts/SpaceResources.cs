using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Vector2 meteors;
    public Vector2 meteorMass;

    [Header("Gravity Manipulator")]
    public int gravityModifier;

    [Header("Mining Probe")]
    public GameObject baseProbe;
    public Material[] probeMats;
    public Vector2 probeMass;

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
            if (cost[i] <= points && !buttons[i].interactable)
                buttons[i].interactable = true;
            else if (cost[i] > points && buttons[i].interactable)
                buttons[i].interactable = false;
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

    }

    private void MeteorShower()
    {

    }

    private void GravitationManipulator()
    {

    }

    private void MiningProbe()
    {

    }


}
