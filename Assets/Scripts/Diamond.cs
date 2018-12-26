using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Diamond : MonoBehaviour {

    public Vector3Int coordinates;
    public GameObject game;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MovePlayer()
    {
        //if we are in the same floor, then move player
        if (game.GetComponent<Game>().playercoord.y - coordinates.y == 0) game.GetComponent<Game>().MovePlayer(coordinates);
        //otherwise just collect the diamond
        else game.GetComponent<Game>().DiamondFound(gameObject, true);
    }


    private void EnableCloseToPlayer(Vector3Int playerto)
    {
        bool nextToPlayer = (coordinates - playerto).magnitude == 1;
        //enable every cube next to player
        gameObject.GetComponent<EventTrigger>().enabled = nextToPlayer;
    }

}
