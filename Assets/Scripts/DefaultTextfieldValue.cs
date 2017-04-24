using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DefaultTextfieldValue : MonoBehaviour
{
    public string defaultValue;
    public Toggle useDefaultValue;

    private Text placeHolder;
    private Text text;

    void Start ()
    {
        Text[] textGameObjects = transform.GetComponentsInChildren<Text>();
        foreach (Text textObject in textGameObjects)
        {
            if (textObject.transform.name == "Placeholder")
            {
                placeHolder = textObject;
            }
            if (textObject.transform.name == "Text")
            {
                text = textObject;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        
	    if (useDefaultValue.isOn)
	    {
	        placeHolder.enabled = false;
            Debug.Log("DEFAULT");
	        placeHolder.text = "";
	        text.text = defaultValue;
	    }
	}
}
