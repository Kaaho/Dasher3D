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
        Game.OnPlayerMoved += IsPlayerCloseToStone;
        Game.OnSandDestroyed += CheckIfFreeUnder;
    }

    private void OnDisable()
    {
        Stone.OnStoneMoved -= CheckIfFreeUnder;
        Stone.OnStoneMoved -= EnableCloseToPlayer;
        Game.OnPlayerMoved -= EnableCloseToPlayer;
        Game.OnPlayerMoved -= IsPlayerCloseToStone;
        Game.OnSandDestroyed -= CheckIfFreeUnder;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lpos = transform.localPosition - new Vector3(0, 0.5f, 0);
        Vector3Int lposint = new Vector3Int(Mathf.RoundToInt(lpos.x), Mathf.RoundToInt(lpos.y), Mathf.RoundToInt(lpos.z));
        if (moving & !lposint.Equals(coordinates))
        {
            StoneHasMoved(lposint);
        }
    }

    private void StoneHasMoved(Vector3Int to)
    {
        Vector3Int origcoordinates = coordinates;
        Debug.Log("StoneHasMoved to: " + to.ToString());
        coordinates = to;
        game.GetComponent<Game>().StoneMoved(origcoordinates, to);
        if (OnStoneMoved != null)
        {
            OnStoneMoved();
        }
        game.GetComponent<Game>().MovePlayer(origcoordinates);

    }

    private void IsPlayerCloseToStone()
    {
        Vector3Int playerto = game.GetComponent<Game>().playercoord;
        //if player is below
        if (coordinates.Equals(playerto + new Vector3Int(0, 1, 0)))
        {
            Debug.Log("Player under stone " + coordinates.ToString());
            UnFreezeStone();
        }
    }

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
        Vector3Int playerto = game.GetComponent<Game>().playercoord;
        bool nextToPlayer = (coordinates - playerto).magnitude == 1;
        if (nextToPlayer)
        {
            //not possible to move unless null next to
            Vector3Int isnullnext = coordinates - (playerto - coordinates);
            gameObject.GetComponent<EventTrigger>().enabled = game.GetComponent<Game>().IsNull(isnullnext);
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
        Vector3Int newcoord = coordinates - (playercoord - coordinates);
        transform.localPosition = new Vector3(newcoord.x, 0.475f + newcoord.y, newcoord.z);
        gameObject.GetComponent<EventTrigger>().enabled = false;
        StoneHasMoved(newcoord);
    }


}
