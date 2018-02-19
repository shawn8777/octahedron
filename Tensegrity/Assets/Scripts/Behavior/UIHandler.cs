using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Slider LengthChanger;

    [SerializeField] private Text LengthText;

    private List<GameObject> SourcePos;

     float Defualt = 12.25f;

    private void Awake()
    {
        
    }


    // Use this for initialization
	void Start ()
	{
	    LengthChanger.value = Defualt;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    LengthText.text = LengthChanger.value.ToString();
	}

    public float SliderValue()
    {
        return LengthChanger.value;
    }
}
