using UnityEngine;
using System.Collections;

public class Traffic : MonoBehaviour 
{
    public Car CarTemplate;
    public Transform StopLine;

    public void SpawnCar()
    {
        Car car = (Car)Instantiate(CarTemplate, transform.position, transform.rotation);
        car.StopAtRedLight = true;
        car.StopLinePosition = StopLine.position.x;
    }
}
