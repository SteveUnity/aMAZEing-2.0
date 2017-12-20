using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {

    public Canvas canvas;
    [Space(5)]
    public Slider mazeSize;
    public InputField mazeSizeText;
    [Space(5)]
    public Slider mazeScale;
    public InputField mazeScaleText;
    [Space(5)]
    public Slider mazeHeight;
    public InputField mazeHeightText;
    [Space(5)]
    public Slider mazeRender;
    public InputField mazeRenderText;
    public Toggle mazeRenderToggle;

    [Space(5)]
    public InputField seedInput;
    public Button randomSeedButton;

    [Space(5)]
    public Button generateMaze;
    public Button endGame;

    [Space(15)]
    public GameObject player;


    private MazeGenerator mazeGen;
    private MazeReveal mazeR;
    private bool inGame = false;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        mazeGen = GetComponent<MazeGenerator>();
        mazeR = GetComponent<MazeReveal>();

        mazeSize.onValueChanged.AddListener(SetMazeSize);
        mazeScale.onValueChanged.AddListener(SetMazeScale);
        mazeHeight.onValueChanged.AddListener(SetMazeHeight);
        mazeRender.onValueChanged.AddListener(SetMazeRenderDistance);

        mazeRenderToggle.onValueChanged.AddListener(SetDisableRenderDistance);
        seedInput.onEndEdit.AddListener(SetSeed);
        randomSeedButton.onClick.AddListener(SetRandomSeed);

        generateMaze.onClick.AddListener(CreateMaze);
        endGame.onClick.AddListener(EndGame);

        SetMazeSize(mazeSize.value);
        SetMazeScale(mazeScale.value);
        SetMazeHeight(mazeHeight.value);
        SetMazeRenderDistance(mazeRender.value);
        seedInput.text = mazeGen.Seed.ToString();

    }
	
	// Update is called once per frame
	void Update () {

       
        if (inGame)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                canvas.enabled = !canvas.enabled;
            }
            if(endGame.interactable == false)
            endGame.interactable = true;
        }
        else
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (endGame.interactable == true)
                endGame.interactable = false;
        }
        if (canvas.enabled && !seedInput.isFocused)
        {
            seedInput.text = mazeGen.Seed.ToString();
        }

    }
    void SetMazeSize(float value)
    {
        mazeGen.mazeSize = (int)value;
        mazeSizeText.text = ((int)value).ToString();
    }
    void SetMazeScale(float value)
    {
        mazeGen.mazeScale = value / 4.0f;
        mazeScaleText.text = (value / 4.0f).ToString();
    }
    void SetMazeHeight(float value)
    {
        mazeGen.mazeHeight = value / 4.0f;
        mazeHeightText.text = (value / 4.0f).ToString();
    }
    void SetMazeRenderDistance(float value)
    {
        mazeR.renderDist = (int)(value*8);
        mazeRenderText.text = (value*8).ToString();
    }

    void SetDisableRenderDistance(bool value)
    {
        if (!value)
        {
            mazeR.renderDist = 0;
            mazeRenderText.text = "0";
            mazeRender.interactable = false;
        }
        else
        {
            mazeR.renderDist = (int)mazeRender.value * 8;
            mazeRenderText.text = (mazeRender.value*8).ToString();
            mazeRender.interactable = true;
        }
    }

    void SetSeed(string value)
    {
        mazeGen.Seed = int.Parse(value);
    }
    void SetRandomSeed()
    {
        mazeGen.Seed = Random.Range(int.MinValue+5,int.MaxValue-5);
    }

    void CreateMaze()
    {
        inGame = true;
        print("mazeGen.GenerateMaze from UI");
        mazeGen.GenerateMaze();
        player.SetActive(true);
        canvas.enabled = false ;
    }
    void EndGame()
    {
        inGame = false;
        player.SetActive(false);
        mazeGen.DestroyMaze();
    }

}
