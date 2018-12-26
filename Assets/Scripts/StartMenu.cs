using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    public Button DayDreamButton;
    public Button AddFloorButton;
    public Button SubtractFloorButton;
    public Text FloorText;
    public Button AddTilesButton;
    public Button SubtractTilesButton;
    public Text TilesText;
    public Button AddStonesButton;
    public Button SubtractStonesButton;
    public Text StonesText;
    public Button AddDiamondsButton;
    public Button SubtractDiamondsButton;
    public Text DiamondsText;
    public Button AddTimeButton;
    public Button SubtractTimeButton;
    public Text TimeText;

    // Use this for initialization
    void Start () {
        FloorText.text = "Floors: " + GameVariables.Floors;
        TilesText.text = "LadderTiles: " + GameVariables.Tiles;
        StonesText.text = "Stones: " + GameVariables.Stones;
        DiamondsText.text = "Diamonds: " + GameVariables.Diamonds;
        TimeText.text = "Time: " + GameVariables.Time + "s";
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
 
    public void LoadScene(string mode)
    {
        GameVariables.Device = mode;
        Debug.Log("Starting with " + GameVariables.Device);
        SceneManager.LoadScene("Main");
    }

    private void OnEnable()
    {
        if (!GameVariables.DaydreamSupported) DayDreamButton.interactable = false;
    }

    public void DiamondsAdd(int amount)
    {
        GameVariables.Diamonds += amount;
        DiamondsText.text = "Diamonds: " + GameVariables.Diamonds;
        if (GameVariables.Diamonds == 1)
        {
            SubtractDiamondsButton.interactable = false;
        }
        else
        {
            SubtractDiamondsButton.interactable = true;
        }
        if (GameVariables.Diamonds == 10)
        {
            AddDiamondsButton.interactable = false;
        }
        else
        {
            AddDiamondsButton.interactable = true;
        }
    }

    public void StonesAdd(int amount)
    {
        GameVariables.Stones += amount;
        StonesText.text = "Stones: " + GameVariables.Stones;
        if (GameVariables.Stones == 1)
        {
            SubtractStonesButton.interactable = false;
        }
        else
        {
            SubtractStonesButton.interactable = true;
        }
        if (GameVariables.Stones == 10*GameVariables.Floors)
        {
            AddStonesButton.interactable = false;
        }
        else
        {
            AddStonesButton.interactable = true;
        }
    }

    public void TilesAdd(int amount)
    {
        GameVariables.Tiles += amount;
        TilesText.text = "LadderTiles: " + GameVariables.Tiles;
        if (GameVariables.Tiles == 1)
        {
            SubtractTilesButton.interactable = false;
        }
        else
        {
            SubtractTilesButton.interactable = true;
        }
        if (GameVariables.Tiles == 10 * GameVariables.Floors)
        {
            AddTilesButton.interactable = false;
        }
        else
        {
            AddTilesButton.interactable = true;
        }
    }


    public void FloorAdd(int amount)
    {
        GameVariables.Floors += amount;
        FloorText.text = "Floor: " + GameVariables.Floors;
        if (GameVariables.Floors == 1)
        {
            SubtractFloorButton.interactable = false;
        }
        else
        {
            SubtractFloorButton.interactable = true;
        }
        if (GameVariables.Floors == 10)
        {
            AddFloorButton.interactable = false;
        }
        else
        {
            AddFloorButton.interactable = true;
        }
    }

    public void TimeAdd(int amount)
    {
        GameVariables.Time += amount;
        TimeText.text = "Time: " + GameVariables.Time + "s";
        if (GameVariables.Time == 0)
        {
            SubtractTimeButton.interactable = false;
            TimeText.text = "Time: none";
        }
        else
        {
            SubtractTimeButton.interactable = true;
        }
    }
}
