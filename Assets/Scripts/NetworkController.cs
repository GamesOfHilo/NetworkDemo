using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkController : MonoBehaviour
{

    private bool isHost = false;
    private string ip = "IP";
    private GameObject playerobj = null;

    public GameObject Spawn;
    public GameObject PlayerPrefab;


    public bool isConnected { get; protected set; }

    // Use this for initialization
    void Start()
    {
        playerobj = Instantiate(PlayerPrefab, Spawn.transform.position, Spawn.transform.rotation) as GameObject;
        playerobj.GetComponentInChildren<Camera>().enabled = true;
        playerobj.GetComponentInChildren<AudioListener>().enabled = true;
        isConnected = false;
    }

    void OnGUI()
    {
        if (!isConnected)
        {
            if (!isHost)
            {
                if (GUI.Button(new Rect(10, 10, 150, 100), "Host a server"))
                {
                    print("Host!");
                    isHost = true;
                    Network.InitializeServer(10, 8000, false);
                }
                ip = GUI.TextField(new Rect(10, 130, 100, 25), ip, 25);
                if (GUI.Button(new Rect(120, 130, 100, 25), "Connect"))
                {
                    print("Connecting");
                    print(Network.Connect(ip, 8000));
                }
            }
            else
            {
                GUI.Label(new Rect(10, 10, 150, 100), "Waiting for player...");
            }
        }
        else
        {
            GUI.Label(new Rect(10, 10, 150, 100), "Connected!");
        }
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        ConnectionReady();
    }
    void OnConnectedToServer()
    {
        ConnectionReady();
    }


    private void ConnectionReady()
    {
        Destroy(playerobj);
        isConnected = true;
        playerobj = Instantiate(PlayerPrefab, Spawn.transform.position, Spawn.transform.rotation) as GameObject;
        var bodyview = Network.AllocateViewID();
        var colorview = Network.AllocateViewID();
        networkView.RPC("PlayerInstantiate", RPCMode.OthersBuffered, bodyview, colorview, Spawn.transform.position, Spawn.transform.rotation);
        playerobj.GetComponent<NetworkView>().viewID = bodyview;

        var body = playerobj.transform.Find("Body").gameObject;
        print(body);
        body.networkView.viewID = colorview;

        playerobj.GetComponentInChildren<Camera>().enabled = true;
        playerobj.GetComponentInChildren<AudioListener>().enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            playerobj.transform.position = Spawn.transform.position;
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }


    [RPC]
    void PlayerInstantiate(NetworkViewID bodyView, NetworkViewID colorView, Vector3 location, Quaternion rotation)
    {
        var instant = Instantiate(PlayerPrefab, location, rotation) as GameObject;
        instant.GetComponent<NetworkView>().viewID = bodyView;
        var body = instant.transform.Find("Body").gameObject;
        body.networkView.viewID = colorView;
    }
}
