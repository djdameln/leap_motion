using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    static bool anyButtonPressed; // flag indicating if any button instance is currently pressed
    bool thisButtonPressed; // flag indicating if this button instance is currently pressed

    public Material defaultColor; // colors of button, public to allow changing from editor
    public Material pressedColor;

    // Use this for initialization
    void Start () {
        anyButtonPressed = false;
        thisButtonPressed = false;
	}

    // method that changes the material, used when pressed or unpressed
    private void ChangeColor(Material material)
    {
        //Fetch the Renderer from the GameObject
        Renderer rend = GetComponent<Renderer>();

        //Set the main Color of the Material
        rend.material = material;
    }

    // press instance, called from editor
    public void Press()
    {
        if (!anyButtonPressed)
        {
            ChangeColor(pressedColor);
            thisButtonPressed = true;
            anyButtonPressed = true;
            GetComponent<ButtonActionManager>().StartSceneCountDown();
        }
    }

    // unpress instance, called from editor
    public void Unpress()
    {
        if (thisButtonPressed)
        {
            ChangeColor(defaultColor);
            thisButtonPressed = false;
            anyButtonPressed = false;
            GetComponent<ButtonActionManager>().CancelInvoke();
        }
    }
}
