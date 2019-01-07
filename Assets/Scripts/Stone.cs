using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stone : MonoBehaviour
{
    public delegate void StoneMoved();
    public static event StoneMoved OnStoneMoved;

    public Vector3Int coordinates;
    public GameObject game;

    private bool moving = false;

    // Use this for initialization
    void Start()
    {
    }

    private void OnEnable()
    {
        Stone.OnStoneMoved += CheckIfFreeUnder;
        Stone.OnStoneMoved += EnableCloseToPlayer;
        Game.OnPlayerMoved += EnableCloseToPlayer;
        //Game.OnPlayerMoved += IsPlayerCloseToStone;
        Game.OnSandDestroyed += CheckIfFreeUnder;
    }

    private void OnDisable()
    {
        Stone.OnStoneMoved -= CheckIfFreeUnder;
        Stone.OnStoneMoved -= EnableCloseToPlayer;
        Game.OnPlayerMoved -= EnableCloseToPlayer;
        //Game.OnPlayerMoved -= IsPlayerCloseToStone;
        Game.OnSandDestroyed -= CheckIfFreeUnder;
    }

    // Update is called once per frame
    void Update()
    {
        //if the stone is moving currently
        if (moving)
        {
            //then check if it has already changed position
            Vector3 lpos = transform.localPosition - new Vector3(0, 0.5f, 0);
            Vector3Int lposint = new Vector3Int(Mathf.RoundToInt(lpos.x), Mathf.RoundToInt(lpos.y), Mathf.RoundToInt(lpos.z));
            if (!lposint.Equals(coordinates))
            {
                //and if it has, change the coordinates
                ChangeStoneCoordinates(lposint, false);
            }
        }
    }

    private void ChangeStoneCoordinates(Vector3Int to, bool moveplayer)
    {
        //The original coordinates
        Vector3Int origcoordinates = coordinates;
        //tell it also to the game
        game.GetComponent<Game>().StoneMoved(gameObject, to);
        if (OnStoneMoved != null)
        {
            //and then to everyone, who cares about it
            OnStoneMoved();
        }
        //if the player pushed the stone, move also the player
        if (moveplayer) game.GetComponent<Game>().MovePlayer(origcoordinates);

    }

    /*
    private void IsPlayerCloseToStone()
    {
        Vector3Int playercoord = game.GetComponent<Game>().playercoord;
        //if player is below
        if (coordinates.Equals(playercoord + new Vector3Int(0, 1, 0)))
        {
            Debug.Log("Player under stone " + coordinates.ToString());
            UnFreezeStone();
        }
    }
*/
    private void UnFreezeStone()
    {
        /*waiting = true;
        yield return new WaitForSeconds(wait);
        waiting = false;*/
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        Debug.Log("Stone released " + coordinates.ToString());
        moving = true;
    }

    private void EnableCloseToPlayer()
    {
        Vector3Int playercoord = game.GetComponent<Game>().playercoord;
        bool nextToPlayer = (coordinates - playercoord).magnitude == 1;
        if (nextToPlayer)
        {
            //not possible to move unless null on the other side to
            Vector3Int nulloppositeplayer = coordinates - (playercoord - coordinates);
            gameObject.GetComponent<EventTrigger>().enabled = game.GetComponent<Game>().IsNull(nulloppositeplayer);
        }
        else
        {
            gameObject.GetComponent<EventTrigger>().enabled = false;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log( coordinates.ToString() + ":Colliding started with " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.localPosition = new Vector3(coordinates.x, 0.475f + coordinates.y, coordinates.z);
            moving = false;
        }
    }

    private void CheckIfFreeUnder()
    {
        if (game.GetComponent<Game>().IsNull(coordinates - new Vector3Int(0, 1, 0)))
        {
            Debug.Log("Found free place under: " + coordinates.ToString());
            UnFreezeStone();
        }

    }


    public void MoveStone()
    {
        Vector3Int playercoord = game.GetComponent<Game>().playercoord;
        //check the coordinates in the other end
        Vector3Int newcoord = coordinates - (playercoord - coordinates);
        transform.localPosition = new Vector3(newcoord.x, 0.475f + newcoord.y, newcoord.z);
        gameObject.GetComponent<EventTrigger>().enabled = false;
        ChangeStoneCoordinates(newcoord, true);
    }


}
