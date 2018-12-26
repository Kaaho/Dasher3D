using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public delegate void PlayerMoved();
    public static event PlayerMoved OnPlayerMoved;
    public delegate void SandDestroyed();
    public static event SandDestroyed OnSandDestroyed;


    //public static int diamondsToBeFound;
    public Text timerText;
    public Text[] infoText;

    public GameObject player;
    public GameObject floorcube;
    public GameObject sand;
    public GameObject stone;
    public GameObject diamond;
    public GameObject seina;
    public GameObject tile;
    public GameObject smallcube;

    public GameObject upArrow;
    public GameObject downArrow;

    private GameObject[,,] allplaces; //all the places and the GameObjects in them (null if nothing there, except player)
    private GameObject[,,] floorcubes; //bases for each floor
    private GameObject[,,] smallcubes; //all the smallcubes
    private GameObject[] smallcubelayers; //layers where the smallcubes are added

    private List<Vector3Int> sands = new List<Vector3Int>();
    private List<Vector3Int> stones = new List<Vector3Int>();
    private List<Vector3Int> diamonds = new List<Vector3Int>();
    private List<Vector3Int> tiles = new List<Vector3Int>();

    public GameObject[] diamondsToCollect;

    private int currenSmallCubeLayer = 1;
    public Vector3Int playercoord;

    private float playerheight = 0.6f;

    private static int width = 10;
    private static int depth = 10;

    private float timer;


    // Use this for initialization
    void Start()
    {
        timer = GameVariables.Time;
        allplaces = new GameObject[width, GameVariables.Floors, depth];
        smallcubes = new GameObject[width, GameVariables.Floors, depth];
        player.GetComponent<Player>().game = GetComponent<Game>();
        CreateSmallCubeLayers();
        CreateWalls();
        CreateFloorCubes();
        SelectPlayerStart();
        CreateDiamonds();
        CreateStones(); //only after diamonds and playerstartplace has been selected
        CreateTiles();
        CreateSands(); //this must be the last one
        //And finally add player to the "sandy" place
        MovePlayer(playercoord);
    }

    //create walls around the scene;
    private void CreateWalls()
    {
        //"up and down lines" for each floor
        for (int i = -1; i < width + 1; i++)
        {
            for (int j = 0; j < GameVariables.Floors; j++)
            {
                GameObject s = Instantiate(seina);
                s.transform.SetParent(this.transform, false);
                s.transform.localPosition = new Vector3(i, s.transform.localPosition.y + j, -1);
                s = Instantiate(seina);
                s.transform.SetParent(this.transform, false);
                s.transform.localPosition = new Vector3(i, s.transform.localPosition.y + j, depth);
            }
        }
        //then "left and right" lines
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < GameVariables.Floors; j++)
            {
                GameObject s = Instantiate(seina);
                s.transform.SetParent(this.transform, false);
                s.transform.localPosition = new Vector3(-1, s.transform.localPosition.y + j, i);
                s = Instantiate(seina);
                s.transform.SetParent(this.transform, false);
                s.transform.localPosition = new Vector3(depth, s.transform.localPosition.y + j, i);
            }
        }
        //finally the "ceiling"
        for (int i = -1; i < width + 1; i++)
        {
            for (int j = -1; j < depth + 1; j++)
            {
                GameObject s = Instantiate(seina);
                s.transform.SetParent(this.transform, false);
                s.transform.localPosition = new Vector3(i, s.transform.localPosition.y + GameVariables.Floors, j);
            }
        }

    }

    private void CreateSmallCubeLayers()
    {
        //create the layers for smallcubes and put the first one visible (plus the one with the small diamonds)
        smallcubelayers = new GameObject[GameVariables.Floors + 1];
        for (int i = 0; i < GameVariables.Floors + 1; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(player.transform, false);
            if (i == 0) go.transform.localPosition += new Vector3(0f, 0.1f, 0f);
            else go.transform.localPosition += new Vector3(0f, 0.05f, 0f);
            //go.transform.localRotation = Quaternion.Euler(-10, 0, 0);
            if (i > 1) go.SetActive(false);
            smallcubelayers[i] = go;
        }

    }

    private void CreateFloorCubes()
    {
        floorcubes = new GameObject[width, GameVariables.Floors, depth];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < GameVariables.Floors; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    GameObject fcube = Instantiate(floorcube);
                    fcube.transform.SetParent(transform, false);
                    fcube.transform.localPosition = new Vector3(i, fcube.transform.localPosition.y + j, k);
                    fcube.GetComponent<Pohja>().game = gameObject;
                    fcube.GetComponent<Pohja>().player = player;
                    fcube.GetComponent<Pohja>().coordinates = new Vector3Int(i, j, k);
                    floorcubes[i, j, k] = fcube;
                    if (j == 0) // show the floor only in the first "floor"
                    {
                        fcube.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }

    }

    //note: must be called in the beginning
    private void SelectPlayerStart()
    {
        //select one of all possible places randomly
        playercoord = new Vector3Int(Random.Range(0, width), 0, Random.Range(0, depth)); //but only from the first floor
        playercoord = new Vector3Int(0, 0, 0);
        //Debug.Log("player added to " + playercoord.ToString());
    }

    //note: to be called after selecting playerstart
    private void CreateDiamonds()
    {
        diamondsToCollect = new GameObject[GameVariables.Diamonds];
        for (int i = 0; i < GameVariables.Diamonds; i++) //add diamonds to be found
        {
            Vector3Int coord = new Vector3Int(Random.Range(0, width), Random.Range(0, GameVariables.Floors), Random.Range(0, depth));
            //if we hit the player reserved place already...
            if (playercoord.Equals(coord) || allplaces[coord.x, coord.y, coord.z] != null)
            {
                i--;
            }
            else
            {
                GameObject smallcub = Instantiate(smallcube);
                smallcub.transform.SetParent(smallcubelayers[coord.y + 1].transform, false);
                smallcub.transform.localPosition = new Vector3((coord.x * 1f - width / 2f) * 0.05f, smallcub.transform.localPosition.y, (coord.z * 1f - depth / 2f) * 0.05f);
                smallcubes[coord.x, coord.y, coord.z] = smallcub;
                //mark it as diamond
                smallcub.transform.GetChild(3).gameObject.SetActive(true);
                //create diamond
                GameObject diamondcube = Instantiate(diamond);
                diamondcube.transform.SetParent(transform, false);
                diamondcube.transform.localPosition = new Vector3(coord.x, diamondcube.transform.localPosition.y + coord.y, coord.z);
                diamondcube.GetComponent<Diamond>().coordinates = coord;
                diamondcube.GetComponent<Diamond>().game = gameObject;
                diamonds.Add(coord);
                allplaces[coord.x, coord.y, coord.z] = diamondcube;
                //create small cubes to represent diamonds
                GameObject smalldiamond = Instantiate(smallcube);
                smalldiamond.transform.SetParent(smallcubelayers[0].transform, false);
                smalldiamond.transform.localPosition = new Vector3((-1f - width / 2f) * 0.05f, smallcub.transform.localPosition.y, (depth / 2f - i - 1) * 0.05f);
                smalldiamond.transform.Rotate(new Vector3(90, 0, 0));
                smalldiamond.transform.GetChild(3).gameObject.SetActive(true);
                smalldiamond.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.gray;
                diamondsToCollect[i] = smalldiamond;

            }
        }
    }

    //only after diamonds have been created and playerstartplace has been selected
    private void CreateStones()
    {
        for (int i = 0; i < GameVariables.Stones; i++)
        {
            //select one of all possible randomly
            Vector3Int coord = new Vector3Int(Random.Range(0, width), Random.Range(0, GameVariables.Floors), Random.Range(0, depth)); //but only from the first floor
            // check that we are not creating to player's place or above it
            bool playertooclose = playercoord.Equals(coord);
            if (coord.y - 1 >= 0) playertooclose = playertooclose || playercoord.Equals(coord - new Vector3Int(0, 1, 0));
            if (coord.z - 1 >= 0) playertooclose = playertooclose || playercoord.Equals(coord - new Vector3Int(0, 0, 1));
            if (playertooclose || allplaces[coord.x, coord.y, coord.z] != null)
            {
                i--;
            }
            else
            {
                //create the smallcube and add it to correct layer
                GameObject smallcub = Instantiate(smallcube);
                smallcub.transform.SetParent(smallcubelayers[coord.y + 1].transform, false);
                smallcub.transform.localPosition = new Vector3((coord.x * 1f - width / 2f) * 0.05f, smallcub.transform.localPosition.y, (coord.z * 1f - depth / 2f) * 0.05f);
                smallcubes[coord.x, coord.y, coord.z] = smallcub;
                //mark it as stone
                smallcub.transform.GetChild(2).gameObject.SetActive(true);
                //create the stone
                GameObject stonecube = Instantiate(stone);
                stonecube.transform.SetParent(transform, false);
                stonecube.transform.localPosition = new Vector3(coord.x, stonecube.transform.localPosition.y + coord.y, coord.z);
                stonecube.GetComponent<Stone>().coordinates = coord;
                stonecube.GetComponent<Stone>().game = gameObject;
                stones.Add(coord);
                allplaces[coord.x, coord.y, coord.z] = stonecube;
                floorcubes[coord.x, coord.y, coord.z].GetComponent<EventTrigger>().enabled = false;

            }
        }

    }

    private void CreateTiles()
    {
        int tilesperfloor = GameVariables.Tiles / GameVariables.Floors + 1;
        //Debug.Log(tilesperfloor);
        for (int i = 0; i < GameVariables.Tiles; i++) //add tiles
        {
            Vector3Int coord = new Vector3Int(Random.Range(0, width), i/tilesperfloor, Random.Range(0, depth));
            //Debug.Log(i+ " creatings tiles:" + coord.ToString() + ",with i/tilesperfloor:" + i / tilesperfloor);
            //if we hit the player reserved place already...
            if (playercoord.Equals(coord) || allplaces[coord.x, coord.y, coord.z] != null)
            {
                i--;
            }
            else
            {
                GameObject smallcub = Instantiate(smallcube);
                smallcub.transform.SetParent(smallcubelayers[coord.y + 1].transform, false);
                smallcub.transform.localPosition = new Vector3((coord.x * 1f - width / 2f) * 0.05f, smallcub.transform.localPosition.y, (coord.z * 1f - depth / 2f) * 0.05f);
                smallcubes[coord.x, coord.y, coord.z] = smallcub;
                smallcub.transform.GetChild(4).gameObject.SetActive(true);
                GameObject tilecube = Instantiate(tile);
                tilecube.transform.SetParent(transform, false);
                tilecube.transform.localPosition = new Vector3(coord.x, tilecube.transform.localPosition.y + coord.y, coord.z);
                tilecube.GetComponent<TileWall>().coordinates = coord;
                tilecube.GetComponent<TileWall>().game = gameObject;
                tiles.Add(coord);
                allplaces[coord.x, coord.y, coord.z] = tilecube;
                floorcubes[coord.x, coord.y, coord.z].GetComponent<EventTrigger>().enabled = false;

            }
        }

    }
    //this must be the last one
    private void CreateSands()
    {
        for (int i = 0; i < width; i++) //add tiles
        {
            for (int j = 0; j < GameVariables.Floors; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    if (allplaces[i, j, k] == null)
                    {
                        GameObject smallcub = Instantiate(smallcube);
                        smallcub.transform.SetParent(smallcubelayers[j + 1].transform, false);
                        smallcub.transform.localPosition = new Vector3((i * 1f - width / 2f) * 0.05f, smallcub.transform.localPosition.y, (k * 1f - depth / 2f) * 0.05f);
                        smallcubes[i, j, k] = smallcub;
                        smallcub.transform.GetChild(1).gameObject.SetActive(true);
                        //create sand

                        GameObject sandcube = Instantiate(sand);
                        sandcube.transform.SetParent(this.transform, false);
                        sandcube.transform.localPosition = new Vector3(i, sandcube.transform.localPosition.y + j, k);
                        sandcube.GetComponent<Hiekka>().game = gameObject;
                        sandcube.GetComponent<Hiekka>().coordinates = new Vector3Int(i, j, k);
                        sands.Add(new Vector3Int(i, j, k));
                        allplaces[i, j, k] = sandcube;

                    }
                    else
                    {
                        //Debug.Log(allplaces[i, j, k].tag + " found");
                    }
                }
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartScene");
        }
        if (diamonds.Count > 0)
        {
            if (GameVariables.Time > 0)
            {
                timer -= Time.deltaTime;
                if (timer > 0)
                {
                    timerText.text = "Time left: " + Mathf.RoundToInt(timer) + "s";
                }
                else
                {
                    GameOver("Timeout.");
                }
            }
            else
            {
                timer += Time.deltaTime;
                timerText.text = "Time spent: " + Mathf.RoundToInt(timer) + "s";
            }
        }

    }

    public void MovePlayer(Vector3Int newcoordinates)
    {
        //Debug.Log("Move player to " + newcoordinates.ToString());
        if (diamonds.Count > 0)
        {
            //check the new place
            GameObject currcube = allplaces[newcoordinates.x, newcoordinates.y, newcoordinates.z];
            //show in small cubes the playerplace
            for (int i = 0; i < GameVariables.Floors; i++)
            {
                //remove what has been shown
                if (i == playercoord.y) smallcubes[playercoord.x, i, playercoord.z].transform.GetChild(0).gameObject.SetActive(false);
                else smallcubes[playercoord.x, i, playercoord.z].transform.GetChild(5).gameObject.SetActive(false);
                //and show the new ones
                if (i == newcoordinates.y) smallcubes[newcoordinates.x, i, newcoordinates.z].transform.GetChild(0).gameObject.SetActive(true);
                else smallcubes[newcoordinates.x, i, newcoordinates.z].transform.GetChild(5).gameObject.SetActive(true);

            }
            //move the player (i.e. camera) to correct place
            player.transform.localPosition = newcoordinates + new Vector3(0, playerheight, 0);
            //and mark the new place
            playercoord = newcoordinates;
            if (currcube != null)
            {
                //check if we landed on diamond and collect it
                if (currcube.CompareTag("diamond"))
                {
                    DiamondFound(currcube, false);
                }
                else if (currcube.CompareTag("sand"))
                {
                    DestroySand(currcube);
                }
            }
            //send info on everyone who is interested
            if (OnPlayerMoved != null)
            {
                OnPlayerMoved();
            }
            //if we went to a place where there is nothing to hang on and there is nothing below... then move the player
            if (newcoordinates.y > 0)
            {
                //if there is nothing below or a diamond!
                if (IsNull(newcoordinates - new Vector3Int(0,1,0)) || IsTagged(newcoordinates - new Vector3Int(0, 1, 0), "diamond"))
                {
                    //and nothing to hang on next to ourselves, then move player down
                    if (!TileNext(newcoordinates)) MovePlayer(newcoordinates - new Vector3Int(0, 1, 0));
                }
            }

        }
    }

    public void DestroySand(GameObject cube)
    {
        Vector3Int coord = cube.GetComponent<Hiekka>().coordinates;
        //delete sand and set null
        allplaces[coord.x, coord.y, coord.z] = null;
        smallcubes[coord.x, coord.y, coord.z].transform.GetChild(1).gameObject.SetActive(false);
        Destroy(cube);
        if (OnSandDestroyed != null)
        {
            OnSandDestroyed();
        }
    }
    //for checking if place is next to a tile
    private bool TileNext(Vector3Int coord)
    {
        if (coord.x > 0) if (allplaces[coord.x - 1, coord.y, coord.z] != null) if (allplaces[coord.x - 1, coord.y, coord.z].tag == "tiles") return true;
        if (coord.x < width - 1) if (allplaces[coord.x + 1, coord.y, coord.z] != null) if (allplaces[coord.x + 1, coord.y, coord.z].tag == "tiles") return true;
        if (coord.z > 0) if (allplaces[coord.x, coord.y, coord.z - 1] != null) if (allplaces[coord.x, coord.y, coord.z - 1].tag == "tiles") return true;
        if (coord.z < depth - 1) if (allplaces[coord.x, coord.y, coord.z + 1] != null) if (allplaces[coord.x, coord.y, coord.z + 1].tag == "tiles") return true;
        return false;
    }

    //when diamond is found, destroy it and mark it
    public void DiamondFound(GameObject diamond, bool activatePlayerMoved)
    {
        Vector3Int coordinates = diamond.GetComponent<Diamond>().coordinates;
        allplaces[coordinates.x, coordinates.y, coordinates.z] = null;
        diamonds.Remove(coordinates);
        Destroy(diamond);
        //Debug.Log("Diamonds left: " + diamonds.Count);
        smallcubes[coordinates.x, coordinates.y, coordinates.z].transform.GetChild(3).gameObject.SetActive(false);
        diamondsToCollect[diamonds.Count].transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.yellow;
        diamondsToCollect[diamonds.Count].transform.GetChild(3).GetComponent<AnimationScript>().isAnimated = true;
        diamondsToCollect[diamonds.Count].transform.GetChild(3).GetComponent<AnimationScript>().isRotating = true;

        if (activatePlayerMoved && OnPlayerMoved != null)
        {
            OnPlayerMoved();
        }
        if (diamonds.Count == 0) YouWon();
    }

    //whenever a stone is moving, it's place must be changed
    public void StoneMoved(Vector3Int from, Vector3Int to)
    {
        if (diamonds.Count > 0)
        {
            if (allplaces[to.x, to.y, to.z] == null)
            {
                Debug.Log("Moving stone from: " + from.ToString() + " to: " + to.ToString());
                smallcubes[from.x, from.y, from.z].transform.GetChild(2).gameObject.SetActive(false);
                smallcubes[to.x, to.y, to.z].transform.GetChild(2).gameObject.SetActive(true);
                allplaces[from.x, from.y, from.z] = null;
                allplaces[to.x, to.y, to.z] = stone;
            } else
            {
                //There should not be anything where we want to move the stone!
                Debug.Log("Should never come here!");
            }
        }
    }

    /*    public void DeactivateBase(Vector3Int coord)
        {
            //floorcubes[coord.x, coord.y, coord.z].SetActive(false);
        }


        public GameObject GetStone(Vector3Int coord)
        {
            return allplaces[coord.x, coord.y, coord.z];
        }
    */

    public void ChangeSmallCubeFloor(int amount)
    {
        smallcubelayers[currenSmallCubeLayer].SetActive(false);
        currenSmallCubeLayer += amount;
        smallcubelayers[currenSmallCubeLayer].SetActive(true);
        //Debug.Log("Current layer " + currenSmallCubeLayer + ", total layers" + smallcubelayers.Length);
        downArrow.SetActive(currenSmallCubeLayer != 1);
        upArrow.SetActive(currenSmallCubeLayer != (smallcubelayers.Length - 1));
    }

    public void GameOver(string message)
    {
        diamonds.Clear();
        foreach (Text it in infoText) it.text = message + "\nGame Over.";
        timerText.text = "";
        foreach (GameObject go in allplaces) Destroy(go);
        foreach (GameObject go in smallcubes) Destroy(go);
    }

    public void YouWon()
    {
        foreach (Text it in infoText) it.text = "Congratulations! \nYou won!";
        timerText.text = "";
        foreach (GameObject go in allplaces) Destroy(go);
        foreach (GameObject go in smallcubelayers) go.SetActive(false);
        downArrow.SetActive(false);
        upArrow.SetActive(false);
        //create the found diamonds to randomplaces;
        for (int i = 0; i<diamondsToCollect.Length; i++)
        {
            Vector3Int coord = new Vector3Int(Random.Range(0, width), Random.Range(0, GameVariables.Floors), Random.Range(0, depth));
            //if we hit the player reserved place already...
            if (playercoord.Equals(coord))
            {
                i--;
            }
            else
            {
                //create diamond
                GameObject diamondcube = Instantiate(diamond);
                diamondcube.transform.SetParent(transform, false);
                diamondcube.transform.localPosition = new Vector3(coord.x, diamondcube.transform.localPosition.y + coord.y, coord.z);
                diamondcube.GetComponent<Diamond>().enabled = false;
            }
        }
    }

    public bool IsTagged(Vector3Int coord, string tag)
    {
        if (!IsInsideGameArea(coord)) return false;
        if (allplaces[coord.x, coord.y, coord.z] == null) return false;
        if (allplaces[coord.x, coord.y, coord.z].tag == tag) return true;
        return false;
    }

    public bool IsInsideGameArea(Vector3Int coord)
    {
        if (coord.x < 0 || coord.x >= width) return false;
        if (coord.y < 0 || coord.y >= GameVariables.Floors) return false;
        if (coord.z < 0 || coord.z >= depth) return false;
        return true;
    }

    public bool IsNull(Vector3Int coord)
    {
        if (!IsInsideGameArea(coord)) return false;
        if (allplaces[coord.x, coord.y, coord.z] == null) return true;
        return false;
    }

}
