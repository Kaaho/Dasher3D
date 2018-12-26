using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public Text diamondText;
    public Game game;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding started with" + collision.gameObject.name);
        if (collision.gameObject.tag == "stone")
        {
            Debug.Log("Destroyed by" + collision.gameObject.GetComponent<Stone>().coordinates);
            game.GameOver("Crashed by stone.");
        }
    }

}
