using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    List<MenuButton> menuButtons;
    public static Deck deck;
    public static MenuAreas areas;

    void Awake()
    {
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Menu Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        //GameObject deckObject = GameObject.Instantiate(Resources.Load("Prefabs/Deck") as GameObject, transform.position, transform.rotation);
        //deck = deckObject.GetComponent<Deck>();
        GameObject areasObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuAreas") as GameObject, transform.position, transform.rotation);
        areas = areasObject.GetComponent<MenuAreas>();

        GameObject.Instantiate(Resources.Load("Prefabs/Logo") as GameObject, new Vector3(0f, 0f, 0f), transform.rotation);

        menuButtons = new List<MenuButton>(){
                makeButton("Battle", () => areas.getButtonLocFromBot(-1), loadGame, true),
            makeButton("Library", () => areas.getButtonLocFromBot(0), loadGame, true),
            makeButton("Settings", () => areas.getButtonLocFromBot(1), loadGame, true),
        };
    }

    MenuButton makeButton(string name, Func<Vector3> centerFunction, Action buttonAction, bool visible)
    {
        GameObject targetGameObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuButton") as GameObject, new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0));
        MenuButton baseButton = targetGameObject.GetComponent<MenuButton>();
        baseButton.setUp(name, centerFunction, buttonAction, visible);
        return baseButton;
    }

    void loadGame()
    {
        Debug.Log("Load Game!");
    }

    void Update()
    {
        Vector3 mouseRelativePosition = new Vector3(0f, 0f, 0f);
        if (Mouse.current != null)
        {
            mouseRelativePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        mouseRelativePosition.z = 0f;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (MenuButton button in menuButtons)
            {
                if (button.ContainsMouse(mouseRelativePosition) && button.isVisible())
                {
                    button.buttonAction.Invoke();
                    break;
                }
            }
        }
    }
}
