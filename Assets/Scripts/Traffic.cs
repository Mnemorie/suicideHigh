using UnityEngine;
using System.Collections;

public class Traffic : MonoBehaviour 
{
    public GameObject CarTemplate;

    public void SpawnCar()
    {
        Instantiate(CarTemplate, transform.position, transform.rotation);
    }
}
