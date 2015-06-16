using UnityEngine;
using System.Collections.Generic;

[SelectionBase]
public class Character : MonoBehaviour {

    public KeyCode KeyCode;
    public bool Go;
    public float Strength = 1;

    public Renderer Head;
    public Renderer Body;
    public Renderer Legs;

    private Rigidbody RigidBody;
    public bool IsDead;
    public bool IsReadyForNextRound;

	void Start ()
    {
        RigidBody = GetComponent<Rigidbody>();
        Body.material.color = new Color(Random.value, Random.value, Random.value);
	}
	
	void Update () 
    {
        if (IsDead)
        {
            if (Input.GetKeyDown(KeyCode))
            {
                IsReadyForNextRound = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode) && transform.position.y <= 0.1)
            {
                RigidBody.AddForce(Vector3.up * Strength, ForceMode.Impulse);
            }
        }
	}

    public void Bang()
    {
        IsDead = true;

        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Destroy(RigidBody);

        
        Head.gameObject.AddComponent<Rigidbody>();
        Body.gameObject.AddComponent<Rigidbody>();
        Legs.gameObject.AddComponent<Rigidbody>();

        List<TrailRenderer> trails = new List<TrailRenderer>(GetComponentsInChildren<TrailRenderer>());
        foreach (TrailRenderer t in trails)
        {
            t.enabled = true;
        }
    }

    public GUISkin Skin;
    public Transform Label;
    
    void OnGUI()
    {
        if (Skin)
        {
            GUI.skin = Skin;
        }

        Vector2 screenPos = GUIUtility.ScreenToGUIPoint(Camera.main.WorldToScreenPoint(Label.position));
        Rect labelRect = new Rect(screenPos.x - 50, Screen.height - screenPos.y, 100, 50);

        string text;
        if (IsReadyForNextRound)
        {
            text = ":)";
        }
        else if (IsDead)
        {
            text = ":(";
        }
        else
        {
            text = KeyCode.ToString();
        }

        DrawOutline(labelRect, text, 2);
        GUI.color = Color.white;
        GUI.Label(labelRect, text);
    }

    void DrawOutline(Rect r, string t, int strength)
    {
        GUI.color = new Color(0, 0, 0, 1);
        int i;
        for (i = -strength; i <= strength; i++)
        {
            GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t);
            GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t);
        }
        for (i = -strength + 1; i <= strength - 1; i++)
        {
            GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t);
            GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t);
        }
    }
}
