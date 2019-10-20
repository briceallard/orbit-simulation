using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGeneration : MonoBehaviour
{
    // Reference variables for planetary objects
    public int planetCount = 5000;   // Changes the number of planets in our solor system
    public int maxRadius = 200;     // Changes available spawn distance from sun
    public GameObject[] planets;
    public Material[] materials;

    // Reference variables for Newton's Laws (Planetary physics)
    float gravity = 6.668f;     // Changes the force of gravity
    int gravityMultiplier = 80; // Higher the number, faster the planetary movement

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
        // Create our prefab Spheres
        var p = new GameObject[count];
        var pCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Enable physics interaction with our planets
        Rigidbody rb = pCopy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        // Generate planetCount number of planets
        for (int i = 0; i < count; i++)
        {
            // Create and position planet randomly within maxRadius
            var plnt = GameObject.Instantiate(pCopy);
            plnt.transform.position = this.transform.position +
                new Vector3(Random.Range(-maxRadius, maxRadius),
                            Random.Range(-10, 10),
                            Random.Range(-maxRadius, maxRadius));

            // Determine size of the planet randomly
            plnt.transform.localScale *= Random.Range(0.3f, 1);

            // Add material texture to planet for styling
            plnt.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            
            // Create trail for tracking planet movement
            TrailRenderer tr = plnt.AddComponent<TrailRenderer>();
            tr.time = 0.5f;
            tr.startWidth = 0.05f;
            tr.endWidth = 0;
            tr.startColor = new Color(1, 1, 0, 0.1f);
            tr.endColor = new Color(0, 0, 0, 0);

            // Add generated planet to array of planets
            planets[i] = plnt;
        }

        // Destory prefab sphere (they're now in planets[])
        GameObject.Destroy(pCopy);

        return planets;
    }

    // Update is called once per frame
    void Update()
    {
        // Create simulation of Newtonian physics for planet movement
        // Newton's Law of Universal Gravitation states that:
        // The force of gravity, F of g, is given by
        // F of g = G((M1*M2)/R^2)
        // G = gravitational constant = ~6.668 x 10^-8
        // M1 = mass of object #1
        // M2 = mass of object #2
        // R = distance between the objects

        foreach (GameObject p in planets)
        {
            // Difference in position between our two 3d (xyz) space objects
            Vector3 diff = this.transform.position - p.transform.position;

            // The distance(R) between the two objects in a 2d perspective
            float dist = diff.magnitude;

            // Create a gravitational pull towards the sun.
            Vector3 gravityDir = diff.normalized;

            // Calculate our gravitational force towards the sun
            float force = gravity * (this.transform.localScale.x *
                                    p.transform.localScale.x * gravityMultiplier) /
                                    (dist * dist);

            // Apply the direction of  gravity towards to sun to each planet
            Vector3 gravityVect = (gravityDir * force);

            // Apply directional force to each planet
            // This is because each planet was also moving in their own direction
            // when they encountered the gravitational force.
            p.transform.GetComponent<Rigidbody>()
                .AddForce(p.transform.forward, ForceMode.Acceleration);
            
            // Apply acceleration to gravitational force of each planet
            p.transform.GetComponent<Rigidbody>()
                .AddForce(gravityVect, ForceMode.Acceleration);
        }
    }
}
