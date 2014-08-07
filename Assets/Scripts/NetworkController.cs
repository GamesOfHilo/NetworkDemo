using UnityEngine;
using System.Collections;

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
        isConnected = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        print("Player connected");
        ConnectionReady();
    }
    void OnConnectedToServer()
    {
        print("Connected");
        ConnectionReady();
    }


    private void ConnectionReady()
    {
        Destroy(playerobj);
        isConnected = true;
        var player = Network.Instantiate(PlayerPrefab, Spawn.transform.position, Spawn.transform.rotation, 0) as GameObject;
        player.GetComponentInChildren<Camera>().enabled = true;
    }
}
