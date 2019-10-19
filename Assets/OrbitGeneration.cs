using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGeneration : MonoBehaviour
{

    public int planetCount = 500;   // Changes the number of planets in our solor system
    public int maxRadius = 200;      // Changes available spawn distance from sun
    public GameObject[] planets;
    public Material[] materials;

    void Awake()
    {
        planets = new GameObject[planetCount];
    }

    // Start is called before the first frame update
    void Start()
    {
        planets = CreatePlanets(planetCount, maxRadius);
    }

    public GameObject[] CreatePlanets(int count, int radius)
    {
        var p = new GameObject[count];
        var pCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = pCopy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        for (int i = 0; i < count; i++)
        {
            var plnt = GameObject.Instantiate(pCopy);
            plnt.transform.position = this.transform.position +
                new Vector3(Random.Range(-maxRadius, maxRadius),
                            Random.Range(-maxRadius, maxRadius),
                            Random.Range(-maxRadius, maxRadius));
            plnt.transform.localScale *= Random.Range(0.5f, 1);
            plnt.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            TrailRenderer tr = plnt.AddComponent<TrailRenderer>();
            tr.time = 1.0f;
            tr.startWidth = 0.1f;
            tr.endWidth = 0;
            tr.startColor = new Color(1, 1, 0, 0.1f);
            tr.endColor = new Color(0, 0, 0, 0);
            planets[i] = plnt;
        }

        GameObject.Destroy(pCopy);

        return planets;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
