using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public GameObject generator;
    public Text heightOfLevelText;
    public Text noOfLevelsText;
    public Text massOfBlockText;
    public Text massOfPlatformText;
    public Text randomFluctuationXText;
    public Text randomFluctuationZText;
    public Text sizeOfBlockText;
    public Text sizeOfPlatformText;

    public Text invalidParameterWarning;
    public Toggle useDefaultParameters;

    private GameObject currentInstanceGenerator;

    [HideInInspector] public int heightOfLevel;
    [HideInInspector] public int noOfLevels;
    [HideInInspector] public float massOfBlock;
    [HideInInspector] public float massOfPlatform;
    [HideInInspector] public float randomFluctuationX;
    [HideInInspector] public float randomFluctuationZ;
    [HideInInspector] public float sizeOfBlock;
    [HideInInspector] public float sizeOfPlatform;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject manager = new GameObject("GameManager");
                manager.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private static GameManager instance;

    void Awake()
    {
        instance = this;
        heightOfLevel = -1;
        noOfLevels = -1;
    }

	// Update is called once per frame
	void Update () {

        if (useDefaultParameters.isOn)
	    {
	        heightOfLevelText.text = "3";
            Debug.Log(heightOfLevelText.text);
	        noOfLevelsText.text = "5";
	        massOfBlockText.text = "1.0";
	        massOfPlatformText.text = "1.0";
	        randomFluctuationXText.text = "0.25";
	        randomFluctuationZText.text = "0.25";
	    }

	    if (Input.GetKeyDown(KeyCode.Return))
	    {
	        bool gotHeightOfLevel = Int32.TryParse(heightOfLevelText.text, out heightOfLevel);
	        bool gotNoOfLevels = Int32.TryParse(noOfLevelsText.text, out noOfLevels);
	        bool gotMassOfBlock = float.TryParse(massOfBlockText.text, out massOfBlock);
	        bool gotMassOfPlatform = float.TryParse(massOfPlatformText.text, out massOfPlatform);
	        bool gotRandomFluctuationX = float.TryParse(randomFluctuationXText.text, out randomFluctuationX);
            bool gotRandomFluctuationZ = float.TryParse(randomFluctuationZText.text, out randomFluctuationZ);
	        bool gotSizeOfBlock = float.TryParse(sizeOfBlockText.text, out sizeOfBlock);
	        bool gotSizeOfPlatform = float.TryParse(sizeOfPlatformText.text, out sizeOfPlatform);
            

            if (gotHeightOfLevel && gotNoOfLevels && gotMassOfBlock && gotMassOfPlatform && gotRandomFluctuationX && gotRandomFluctuationZ && gotSizeOfBlock && gotSizeOfPlatform &&
                heightOfLevel > 0 && noOfLevels > 0 && massOfBlock > 0 && massOfPlatform > 0 && randomFluctuationX > 0 && randomFluctuationZ > 0 && sizeOfBlock > 0 && sizeOfPlatform > 0)
	        {
	            if (GameObject.FindGameObjectsWithTag("StructureGenerator").Length == 0)
	            {
	                currentInstanceGenerator = GameObject.Instantiate(generator);
	            }

	            else
	            {
	                GameObject.Destroy(currentInstanceGenerator);
	                currentInstanceGenerator = GameObject.Instantiate(generator);
	            }

	            invalidParameterWarning.text = "";
	        }

            else
            {
                invalidParameterWarning.text = "Invalid parameter!";
            }
	    }
	}
}
