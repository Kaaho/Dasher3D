using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hiekka : MonoBehaviour
{
    public GameObject game;
    public Vector3Int coordinates;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        Game.OnPlayerMoved += EnableCloseToPlayer;
        Stone.OnStoneMoved += EnableCloseToPlayer;
    }


    void OnDisable()
    {
        Game.OnPlayerMoved -= EnableCloseToPlayer;
        Stone.OnStoneMoved -= EnableCloseToPlayer;
    }

    private void EnableCloseToPlayer()
    {
        Vector3Int playerto = game.GetComponent<Game>().playercoord;
        //enable every cube next to player
        if ((coordinates - playerto).magnitude == 1)
        {
            //Debug.Log("Enabling sand for " + coordinates);
            gameObject.GetComponent<EventTrigger>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<EventTrigger>().enabled = false;
        }
    }

    public void MovePlayer()
    {
        //if we the player is in the same floor or above the sand that was touched, then move player
        if (game.GetComponent<Game>().playercoord.y - coordinates.y == 0 || game.GetComponent<Game>().playercoord.y - coordinates.y == 1) game.GetComponent<Game>().MovePlayer(coordinates);
        //otherwise just destroy the sand
        else game.GetComponent<Game>().DestroySand(gameObject);
    }
}
