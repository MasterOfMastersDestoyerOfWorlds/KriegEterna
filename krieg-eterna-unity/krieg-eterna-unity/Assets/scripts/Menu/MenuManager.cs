using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Steamworks;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    MenuPage mainMenu;
    MenuPage pvpMenu;

    public bool sceneLoaded;

    List<MenuPage> menuPages;
    public static Deck deck;
    public static MenuAreas areas;
    public static SteamManager steamManager;
    GameObject logoObject;

    LoadingScreen loadingScreen;



    void Awake()
    {
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Menu Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        //GameObject deckObject = GameObject.Instantiate(Resources.Load("Prefabs/Deck") as GameObject, transform.position, transform.rotation);
        //deck = deckObject.GetComponent<Deck>();
        GameObject areasObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuAreas") as GameObject, transform.position, transform.rotation);
        areas = areasObject.GetComponent<MenuAreas>();

        logoObject = GameObject.Instantiate(Resources.Load("Prefabs/Logo") as GameObject, new Vector3(0f, 0f, 0f), transform.rotation);

        GameObject steamManagerObj = GameObject.Instantiate(Resources.Load("Prefabs/SteamManager") as GameObject, transform.position, transform.rotation);
        steamManager = steamManagerObj.GetComponent<SteamManager>();

        GameObject loadingScreenObj =  GameObject.Instantiate(Resources.Load("Prefabs/LoadingScreen") as GameObject, new Vector3(0f, 0f, -10f), transform.rotation);
        loadingScreen = loadingScreenObj.GetComponent<LoadingScreen>();
        loadingScreen.setVisibile(false);

        mainMenu = new MenuPage(true, new List<MenuButton>(){
            makeButton("Campaign", () => areas.getButtonLocFromBot(1), () => areas.getMouseOverLocFromBot(1), loadGame, true, true),
            makeButton("PvP", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), showPvPOptions, true, true),
            makeButton("Library", () => areas.getButtonLocFromBot(-1), () => areas.getMouseOverLocFromBot(-1), showCardLibrary, true, false),
            makeButton("Settings", () => areas.getButtonLocFromBot(-2), () => areas.getMouseOverLocFromBot(-2), showSettings, true, false),
        });
        pvpMenu = new MenuPage(false, new List<MenuButton>(){
            makeButton("Start Lobby", () => areas.getButtonLocFromBot(1), () => areas.getMouseOverLocFromBot(1), startLobby, false, true),
            makeButton("Customize Rules", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), showPvPCustomizationOptions, false, false),
            makeButton("Back", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), backButton, false, true),
        });

        menuPages = new List<MenuPage>(){
            mainMenu,
            pvpMenu
        };
    }

    MenuButton makeButton(string name, Func<Vector3> centerFunction, Func<Vector3> mouseOverFunction, Func<IEnumerator> buttonAction, bool visible, bool implemented)
    {
        GameObject targetGameObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuButton") as GameObject, new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0));
        MenuButton baseButton = targetGameObject.GetComponent<MenuButton>();
        baseButton.setUp(name, centerFunction, mouseOverFunction, buttonAction, visible, implemented);
        return baseButton;
    }

    IEnumerator setAllNotVisible()
    {
        foreach (MenuPage page in menuPages)
        {
            page.active = false;
            foreach (MenuButton button in page.buttons)
            {
                button.setButtonNotVisible();
            }
        }
        return null;
    }
    IEnumerator goToMenuPage(MenuPage page)
    {
        setAllNotVisible();
        foreach (MenuButton button in page.buttons)
        {
            button.setButtonVisible();
        }
        page.active = true;
        return null;
    }

    public IEnumerator loadGame()
    {
        Debug.Log("Loading Game...");
        setAllNotVisible();
        while(!loadingScreen.FadeIn()){
            yield return null;
        }
        var load = SceneManager.LoadSceneAsync("deskScene", LoadSceneMode.Single);
        while (!load.isDone)
        {
            yield return null;
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    IEnumerator showPvPOptions()
    {
        Debug.Log("Pvp!");
        goToMenuPage(pvpMenu);
        return null;
    }

    IEnumerator showCardLibrary()
    {
        Debug.Log("Library!");
        return null;
    }

    IEnumerator showSettings()
    {
        Debug.Log("Settings!");
        return null;
    }
    IEnumerator startLobby()
    {
        Debug.Log("StartLobby!");
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        return null;
    }

    IEnumerator showPvPCustomizationOptions()
    {
        Debug.Log("PVPRules!");
        return null;
    }

    IEnumerator backButton()
    {
        Debug.Log("Back!");
        goToMenuPage(mainMenu);
        return null;
    }

    void Update()
    {
        Debug.Log("Update");
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
                            
                            StartCoroutine(button.buttonAction.Invoke());
                            return;
                        }
                    }
                    button.mouseOver(mouseOver);
                }
            }
        }
    }
}
