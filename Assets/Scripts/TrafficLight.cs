using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {

    public Renderer GoLight;
    public Renderer WaitLight;
    public Renderer StopLight;

    public float RedLightDuration = 2;
    public float YellowLightDuration = 2;
    public float GreenLightDuration = 2;

    public float TimeScale = 1;

    public Traffic Traffic;

    private Game Game;

    public enum ELight
    {
        Red,
        Yellow,
        Green
    }
    ELight Light;

    public bool Go;

    void Start()
    {
        Light = ELight.Red;

        GoLight.material.color = Color.white;
        WaitLight.material.color = Color.white;
        StopLight.material.color = Color.red;

        Game = FindObjectOfType<Game>();
    }

    private float CycleTime;

    void Update()
    {
        if (!Go)
        {
            return;
        }

        CycleTime += Time.deltaTime * TimeScale;

        if ((Game.Characters.FindAll(c => c.IsDead).Count == Game.Characters.Count - 1) ||
            (Game.Characters.FindAll(c => c.IsDead).Count == Game.Characters.Count))
        {
            Go = false;

            GoLight.material.color = Color.white;
            WaitLight.material.color = Color.white;
            StopLight.material.color = Color.red;
            Light = ELight.Red;
            return;
        }

        if (CycleTime < RedLightDuration && Light != ELight.Red)
        {
            GoLight.material.color = Color.white;
            WaitLight.material.color = Color.white;
            StopLight.material.color = Color.red;
            Light = ELight.Red;
        }
        else if (CycleTime > RedLightDuration && CycleTime < RedLightDuration + YellowLightDuration && Light != ELight.Yellow)
        {
            GoLight.material.color = Color.white;
            WaitLight.material.color = Color.yellow;
            StopLight.material.color = Color.white;
            Light = ELight.Yellow;
        }
        else if (CycleTime > RedLightDuration + YellowLightDuration && CycleTime < RedLightDuration + YellowLightDuration + GreenLightDuration && Light != ELight.Green)
        {
            GoLight.material.color = Color.green;
            WaitLight.material.color = Color.white;
            StopLight.material.color = Color.white;
            Light = ELight.Green;
            
            Traffic.SpawnCar();
        }
        
        if (CycleTime > RedLightDuration + YellowLightDuration + GreenLightDuration)
        {
            CycleTime = 0;
        }
    }

}
