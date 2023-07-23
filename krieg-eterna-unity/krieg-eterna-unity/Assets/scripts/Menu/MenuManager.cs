using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    MenuPage mainMenu;
    MenuPage pvpMenu;

    List<MenuPage> menuPages;
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

        mainMenu = new MenuPage(true, new List<MenuButton>(){
            makeButton("Campaign", () => areas.getButtonLocFromBot(1), () => areas.getMouseOverLocFromBot(1), loadGame, true),
            makeButton("PvP", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), showPvPOptions, true),
            makeButton("Library", () => areas.getButtonLocFromBot(-1), () => areas.getMouseOverLocFromBot(-1), showCardLibrary, true),
            makeButton("Settings", () => areas.getButtonLocFromBot(-2), () => areas.getMouseOverLocFromBot(-2), showSettings, true),
        });
        pvpMenu = new MenuPage(false, new List<MenuButton>(){
            makeButton("Start Lobby", () => areas.getButtonLocFromBot(1), () => areas.getMouseOverLocFromBot(1), startLobby, false),
            makeButton("Customize Rules", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), showPvPCustomizationOptions, false),
            makeButton("Back", () => areas.getButtonLocFromBot(-1), () => areas.getMouseOverLocFromBot(-1), backButton, false),
        });

        menuPages = new List<MenuPage>(){
            mainMenu,
            pvpMenu
        };
    }

    MenuButton makeButton(string name, Func<Vector3> centerFunction, Func<Vector3> mouseOverFunction, Action buttonAction, bool visible)
    {
        GameObject targetGameObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuButton") as GameObject, new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0));
        MenuButton baseButton = targetGameObject.GetComponent<MenuButton>();
        baseButton.setUp(name, centerFunction, mouseOverFunction, buttonAction, visible);
        return baseButton;
    }

    void setAllNotVisible()
    {
        foreach (MenuPage page in menuPages)
        {
            page.active = false;
            foreach (MenuButton button in page.buttons)
            {
                button.setButtonNotVisible();
            }
        }
    }
    void goToMenuPage(MenuPage page)
    {
        setAllNotVisible();
        foreach (MenuButton button in page.buttons)
        {
            button.setButtonVisible();
        }
        page.active = true;
    }

    void loadGame()
    {
        Debug.Log("Load Game!");
    }

    void showPvPOptions()
    {
        Debug.Log("Pvp!");
        goToMenuPage(pvpMenu);
    }

    void showCardLibrary()
    {
        Debug.Log("Library!");
    }

    void showSettings()
    {
        Debug.Log("Settings!");
    }
    void startLobby()
    {
        Debug.Log("StartLobby!");
    }

    void showPvPCustomizationOptions()
    {
        Debug.Log("PVPRules!");
    }

    void backButton()
    {
        Debug.Log("Back!");
        goToMenuPage(mainMenu);
    }

    void Update()
    {
        Vector3 mouseRelativePosition = new Vector3(0f, 0f, 0f);
        if (Mouse.current != null)
        {
            mouseRelativePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        mouseRelativePosition.z = 0f;

        foreach (MenuPage page in menuPages)
        {
            if (page.active)
            {
                foreach (MenuButton button in page.buttons)
                {
                    bool mouseOver = button.ContainsMouse(mouseRelativePosition) && button.isVisible();
                    if (mouseOver)
                    {
                        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            button.buttonAction.Invoke();
                            return;
                        }
                    }
                    button.mouseOver(mouseOver);
                }
            }
        }
    }
}
