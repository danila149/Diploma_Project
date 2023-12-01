using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.UI;

namespace GooglyEyesGames.FusionBites 
{

    public class FusionConnector : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionConnector instance;
        public bool coonectOnAwake = false;
        public NetworkRunner runner;

        [SerializeField] NetworkObject playerPrefabs;

        public string _playerName = null;

        [Header("Session List")]
        public GameObject roomListCanvas;
        private List<SessionInfo> _sessions = new List<SessionInfo>();
        public Button refrechButton;
        public Transform sessionListCounter;
        public GameObject sessionEntyPrefabs;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        public void ConnectToLobby(string playerName)
        {
            roomListCanvas.SetActive(true);

            _playerName = playerName;

            if(runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared);


        }

        public async void OnConnectedToSession(string sessionName)
        {
            roomListCanvas.SetActive(false);

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            { 
                GameMode = GameMode.Shared,
                SessionName = sessionName,
            });
        }

        public async void CreateSession()
        {
            roomListCanvas.SetActive(false);

            int randomInt = UnityEngine.Random.Range(1000, 9999);
            string randomSessionName = "Room-" + randomInt.ToString();

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = randomSessionName,
                PlayerCount = 10,
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("ServerStart");
            NetworkObject playerObject = runner.Spawn(playerPrefabs, Vector3.zero);

            runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        
        
        }


        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _sessions.Clear();
            _sessions = sessionList;

        }

        public void RefreshSessionListUI()
        {
            foreach(Transform child in sessionListCounter)
            {
                Destroy(child.gameObject);
            }

            foreach(SessionInfo session in _sessions)
            {
                if (session.IsVisible)
                {
                    GameObject enty = GameObject.Instantiate(sessionEntyPrefabs, sessionListCounter);
                    SessionEntryPrefabs scripts = enty.GetComponent<SessionEntryPrefabs>();
                    scripts.sessionName.text = session.Name;
                    scripts.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;
                
                    if(session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                    {
                        scripts.joinButton.interactable = false;
                    }
                    else
                    {
                        scripts.joinButton.interactable = true;
                    }
                }
            }
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("Player go to server");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
           
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
           
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}

