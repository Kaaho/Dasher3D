using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pohja : MonoBehaviour {
    public GameObject game;
    public GameObject player;
    public Vector3Int coordinates;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void MovePlayer()
    {
        if (game.GetComponent<Game>().playercoord.Equals(coordinates)) game.GetComponent<Game>().MovePlayer(coordinates - new Vector3Int(0, 1, 0));
        else game.GetComponent<Game>().MovePlayer(coordinates);
    }

    void OnEnable()
    {
        Game.OnPlayerMoved += EnableCloseToPlayer;
        Stone.OnStoneMoved += EnableCloseToPlayer;
        Game.OnSandDestroyed += EnableCloseToPlayer;
    }


    void OnDisable()
    {
        Game.OnPlayerMoved -= EnableCloseToPlayer;
        Stone.OnStoneMoved -= EnableCloseToPlayer;
        Game.OnSandDestroyed -= EnableCloseToPlayer;
    }

    private void EnableCloseToPlayer()
    {
        Vector3Int playercoord = game.GetComponent<Game>().playercoord;
        //if the cube of this floorcube is empty, the distance to player is 1 and this floor is below or in the same floor with player
        if (game.GetComponent<Game>().IsNull(coordinates) && (coordinates - playercoord).magnitude == 1 && (coordinates.y <= playercoord.y))
        {
            //First floor requires box collider so that the stones don't go through
            gameObject.GetComponent<EventTrigger>().enabled = true;
            if (coordinates.y != 0 ) gameObject.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log("Enabled pohja for " + coordinates.ToString());
        } else
        {
            gameObject.GetComponent<EventTrigger>().enabled = false;
            if (coordinates.y != 0) gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
