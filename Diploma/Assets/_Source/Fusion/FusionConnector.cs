using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;


namespace GooglyEyesGames.FusionBites 
{

    public class FusionConnector : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionConnector instance;
        public bool coonectOnAwake = false;
        public NetworkRunner runner;

        [SerializeField] NetworkObject playerPrefabs;

        public string _playerName = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            if(coonectOnAwake == true)
            {
                OnConnectedToRunner("Anonimus");
            }
        }

        public async void OnConnectedToRunner(string playerName)
        {
            _playerName = playerName;


            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            { 
                GameMode = GameMode.Shared,
                SessionName = "test",
                PlayerCount = 2,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("ServerStart");
            NetworkObject playerObject = runner.Spawn(playerPrefabs, Vector3.zero);

            runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        
        
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

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
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

