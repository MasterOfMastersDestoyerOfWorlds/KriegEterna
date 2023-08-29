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
    MenuPage lobbyMenu;

    public bool sceneLoaded;

    List<MenuPage> menuPages;
    public static Deck deck;
    public static MenuAreas areas;
    public static SteamManager steamManager;
    GameObject logoObject;

    LoadingScreen loadingScreen;

    protected Callback<LobbyChatUpdate_t> m_LobbyChatUpdate_t;
    protected Callback<SteamNetworkingMessagesSessionRequest_t> m_SteamNetworkingMessagesSessionRequest_t;



    void Awake()
    {
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Menu Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        //GameObject deckObject = GameObject.Instantiate(Resources.Load("Prefabs/Deck") as GameObject, transform.position, transform.rotation);
        //deck = deckObject.GetComponent<Deck>();
        GameObject areasObject = GameObject.Instantiate(Resources.Load("Prefabs/MenuAreas") as GameObject, transform.position, transform.rotation);
        areas = areasObject.GetComponent<MenuAreas>();

        logoObject = GameObject.Instantiate(Resources.Load("Prefabs/Logo") as GameObject, new Vector3(0f, 0f, 0f), transform.rotation);

        Areas.scaleToScreenSize(logoObject.transform);

        GameObject steamManagerObj = GameObject.Instantiate(Resources.Load("Prefabs/SteamManager") as GameObject, transform.position, transform.rotation);
        steamManager = steamManagerObj.GetComponent<SteamManager>();

        GameObject loadingScreenObj = GameObject.Instantiate(Resources.Load("Prefabs/LoadingScreen") as GameObject, new Vector3(0f, 0f, -10f), transform.rotation);
        loadingScreen = loadingScreenObj.GetComponent<LoadingScreen>();
        loadingScreen.setVisibile(false);
        loadingScreen.setTextVisibile(loadingScreen.roundText, false);
        loadingScreen.setTextVisibile(loadingScreen.displayRowText, false);
        Areas.scaleToScreenSize(loadingScreen.transform);

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
        lobbyMenu = new MenuPage(false, new List<MenuButton>(){
            makeButton("Waiting . . .", () => areas.getButtonLocFromBot(1), () => areas.getMouseOverLocFromBot(1), () => {return null;}, false, true),
            makeButton("Customize Rules", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), showPvPCustomizationOptions, false, false),
            makeButton("Close Lobby", () => areas.getButtonLocFromBot(0), () => areas.getMouseOverLocFromBot(0), closeLobby, false, true),
        });

        menuPages = new List<MenuPage>(){
            mainMenu,
            pvpMenu,
            lobbyMenu
        };


        m_LobbyChatUpdate_t = Callback<LobbyChatUpdate_t>.Create(OnLobbyStateChange);
        m_SteamNetworkingMessagesSessionRequest_t = Callback<SteamNetworkingMessagesSessionRequest_t>.Create(OnSessionOpen);
         var args = System.Environment.GetCommandLineArgs();

        // we really only care if we have 2 or more if we just want the lobbyid.
        if (args.Length >= 2)
        {
            // loop to the 2nd last one, because we are gonna do a + 1
            // the lobbyID is straight after +connect_lobby
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].ToLower() == "+connect_lobby")
                {
                    if (ulong.TryParse(args[i + 1], out ulong lobbyID))
                    {
                        if (lobbyID > 0)
                        {
                            SteamMatchmaking.JoinLobby(lobbyID);
                        }
                    }
                    break;
                }
            }
        }
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
        steamManager.NetworkingTest.setSeed();
        steamManager.NetworkingTest.isNetworkGame = false;
        setAllNotVisible();

        while (!loadingScreen.FadeIn())
        {
            yield return null;
        }
        var load = SceneManager.LoadSceneAsync("deskScene", LoadSceneMode.Single);
        while (!load.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator startNetworkGame()
    {
        if (steamManager.NetworkingTest.host)
        {
            steamManager.NetworkingTest.setSeed();
            steamManager.NetworkingTest.sendNextMessage(PacketType.SEED, steamManager.NetworkingTest.seed);
        }
        steamManager.NetworkingTest.isNetworkGame = true;
        Debug.Log("Loading Game...");
        setAllNotVisible();
        while (!loadingScreen.FadeIn())
        {
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
        steamManager.NetworkingTest.lobbyUpdated = false;
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        return null;
    }

    IEnumerator closeLobby()
    {
        Debug.Log("LeavingLobby!");
        while (!steamManager.NetworkingTest.lobbyUpdated)
        {
            yield return null;
        }
        SteamMatchmaking.LeaveLobby(steamManager.NetworkingTest.lobbyId);
        goToMenuPage(mainMenu);
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
                            //null exception
                            StartCoroutine(button.buttonAction.Invoke());
                            return;
                        }
                    }
                    button.mouseOver(mouseOver);
                }
            }
        }
    }

    private void OnLobbyStateChange(LobbyChatUpdate_t param)
    {
        if (param.m_rgfChatMemberStateChange == ((uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered))
        {
            Debug.Log("User Joined? " + param.m_ulSteamIDUserChanged);
            StartCoroutine(startNetworkGame());
        }
        else
        {
            Debug.Log("User Left? " + param.m_ulSteamIDUserChanged);
            StartCoroutine(closeLobby());
        }

    }

    private void OnSessionOpen(SteamNetworkingMessagesSessionRequest_t param)
    {
        Debug.Log("StartingGame");
        StartCoroutine(startNetworkGame());
    }
}
