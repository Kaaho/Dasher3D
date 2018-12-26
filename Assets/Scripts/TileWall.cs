using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileWall : MonoBehaviour
{

    public Vector3Int coordinates;
    public GameObject game;


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
        Game.OnPlayerMoved += IsPlayerClose;
        Game.OnSandDestroyed += IsPlayerClose;
    }


    void OnDisable()
    {
        Game.OnPlayerMoved -= IsPlayerClose;
        Game.OnSandDestroyed -= IsPlayerClose;
    }



    private void IsPlayerClose()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        //if player is next to this cube activate correct ladders
        Vector3Int diff = coordinates - game.GetComponent<Game>().playercoord;
        //if (diff == new Vector3Int(-1, 0, 0)) transform.GetChild(2).gameObject.SetActive(true);
        //if there is empty next to this tilewall
        if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(1, 0, 0))
            // and the player is in below the empty cube
            && (diff.x == -1 && diff.y == 1 && diff.z == 0))
        {
            //then activate the ladders
            transform.GetChild(2).gameObject.SetActive(true);
        }
        // do the same for other 3 directions
        if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(-1, 0, 0))
            && (diff.x == 1 && diff.y == 1 && diff.z == 0))
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, 1))
            && (diff.x == 0 && diff.y == 1 && diff.z == -1))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, -1))
            && (diff.x == 0 && diff.y == 1 && diff.z == 1))
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }

        /*        //if there is empty next to this tilewall
                if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(-1, 0, 0))
                    // and the player is in the next empty cube, or above or below it
                    && (diff.x == -1 && Mathf.Abs(diff.y) <= 1 && diff.z == 0))
                {
                    //then activate the ladders
                    Debug.Log("Activating ladders 2");
                    transform.GetChild(2).gameObject.SetActive(true);
                }
                // do the same for other 3 directions
                if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(1, 0, 0))
                    && (diff.x == 1 && Mathf.Abs(diff.y) <= 1 && diff.z == 0))
                {
                    Debug.Log("Activating ladders 3");
                    transform.GetChild(3).gameObject.SetActive(true);
                }
                if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, -1))
                    && (diff.x == 0 && Mathf.Abs(diff.y) <= 1 && diff.z == -1))
                {
                    Debug.Log("Activating ladders 0");
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, 1))
                    && (diff.x == 0 && Mathf.Abs(diff.y) <= 1 && diff.z == 1))
                {
                    Debug.Log("Activating ladders 1");
                    transform.GetChild(1).gameObject.SetActive(true);
                }*/
        //if player is next to the ladders, or one floor up or down from them  and the place next is empty
        //if the player is just above the tilewall activete those ladders that can be used
        /*if (newposition.Equals(coordinates + new Vector3Int(0, 1, 0)))
        {
            if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(1, 0, 0))) transform.GetChild(2).gameObject.SetActive(true);
            if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(-1, 0, 0))) transform.GetChild(3).gameObject.SetActive(true);
            if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, 1))) transform.GetChild(0).gameObject.SetActive(true);
            if (game.GetComponent<Game>().IsNull(coordinates + new Vector3Int(0, 0, -1))) transform.GetChild(1).gameObject.SetActive(true);
        }*/
    }

    public void MovePlayer(int direction)
    {
        Vector3Int playercoord = game.GetComponent<Game>().playercoord;
        if (playercoord.y != coordinates.y)
        {
            Vector3Int newposition = playercoord;
            if (direction == 0) newposition = coordinates + new Vector3Int(0, 0, 1);
            else if (direction == 1) newposition = coordinates + new Vector3Int(0, 0, -1);
            else if (direction == 2) newposition = coordinates + new Vector3Int(1, 0, 0);
            else if (direction == 3) newposition = coordinates + new Vector3Int(-1, 0, 0);
            game.GetComponent<Game>().MovePlayer(newposition);

        }
    }
}
