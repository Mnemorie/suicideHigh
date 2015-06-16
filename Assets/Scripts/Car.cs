using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour 
{
    public float Speed = 10; // meters per second

    private Rigidbody RigidBody;
    public bool StopAtRedLight;
    public float StopLinePosition; // on X axis

	// Use this for initialization
	void Start () 
    {
        RigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!StopAtRedLight ||
            (StopAtRedLight && transform.position.x > StopLinePosition))
        {
            transform.Translate(Speed * Vector3.forward * Time.fixedDeltaTime);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character)
        {
            character.Bang();
        }
    }
}
