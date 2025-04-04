using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayAreaManager : NetworkBehaviour
{
    public const int MAX_SCORE_COUNT = 2;

    public static PlayAreaManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnPlayerScoreUpdate;

    [SerializeField] private Transform playerPrefab;
    [SerializeField] private List<Transform> spawnPointsList;

    [SerializeField] private Dictionary<ulong, int> playersScore;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);

    private void Awake()
    {
        Instance = this;
        playersScore = new Dictionary<ulong, int>();
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            AddPlayerInPlayerScoreClientRpc(clientId);
        }
        state.Value = State.CountdownToStart;
    }

    [ClientRpc]
    private void AddPlayerInPlayerScoreClientRpc(ulong clientId)
    {
        playersScore.Add(clientId, 0);
        OnPlayerScoreUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetSpawnPosition(int id)
    {
        return spawnPointsList[id].position;
    }

    public Vector3 GetRandomSpwanPosition()
    {
        return GetSpawnPosition(UnityEngine.Random.Range(0, spawnPointsList.Count));
    }

    private void Update()
    {
        if (!IsServer) return;

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if(countdownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                break;
            case State.GameOver:
                break;
        }
    }

    public void AddPointInPlayer(ulong clientId)
    {
        AddPointInPlayerServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddPointInPlayerServerRpc(ulong clientId)
    {
        int count = playersScore[clientId];
        AddPointInPlayerClientRpc(clientId);
        if (count + 1 >= MAX_SCORE_COUNT)
        {
            state.Value = State.GameOver;
        }
    }

    [ClientRpc]
    private void AddPointInPlayerClientRpc(ulong clientId)
    {
        playersScore[clientId] += 1;
        OnPlayerScoreUpdate?.Invoke(this, EventArgs.Empty);
    }

    public Dictionary<ulong, int> GetPlayerScoreList()
    {
        return playersScore;
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public float GetCountdownStartTimer()
    {
        return countdownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
}
