using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MARandomBrickGenerator : MonoBehaviour
{
    public GameObject brickPrefab;
    public int amount;
    public float density;
    public float rotationRandomness;
    public float zRandomness;

    void Start()
    {
        for (int xIndex = 0; xIndex < this.amount; xIndex++)
        {
            for (int zIndex = 0; zIndex < this.amount; zIndex++)
            {

                Vector3 position = new Vector3(xIndex / this.density, Random.Range(0, zRandomness) , zIndex / this.density);
                Quaternion rotation = Quaternion.Euler(Random.Range(0, this.rotationRandomness), Random.Range(0, this.rotationRandomness), Random.Range(0, this.rotationRandomness));
                GameObject newBrick = Instantiate(this.brickPrefab, position, rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
