using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mono.CSharp.yydebug;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text queueStatusText;
    [SerializeField] private TMP_Text queueTimerText;
    [SerializeField] private TMP_Text findMatchButtonText;
    [SerializeField] private TMP_InputField joinCodeField;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button findMatchButton;
    [SerializeField] private Toggle teamToggle;
    [SerializeField] private Toggle privateToggle;
    private bool isMatchmaking;
    private bool isCanceling;
    private bool isBusy;
    private float timeInQueue;
    void Start()
    {
        if (ClientSingleton.Instance == null) return;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        queueStatusText.text = string.Empty;
        queueTimerText.text = string.Empty;
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        findMatchButton.onClick.AddListener(FindMatchPressed);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMatchmaking)
        {
            timeInQueue += Time.deltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(timeInQueue);
            queueTimerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

        }
    }
    public async void FindMatchPressed()
    {
        if (isCanceling) return;
        if (isMatchmaking)
        {
            queueStatusText.text = "Canceling...";
            isCanceling = true;
            //Cancel matchmaking
            await ClientSingleton.Instance.GameManager.CancelMatchmaking();
            isCanceling = false;
            isMatchmaking = false;
            isBusy = false;
            findMatchButtonText.text = "Find Match";
            queueStatusText.text = string.Empty;
            queueTimerText.text = string.Empty;
            ; return;
        }
        //start queue
        if (isBusy) return;
        ClientSingleton.Instance.GameManager.MatchmakeAsync(teamToggle.isOn,OnMatchMade);
        findMatchButtonText.text = "Cancel";
        queueStatusText.text = "Searching...";
        timeInQueue = 0f;
        isMatchmaking = true;
        isBusy = true;
    }

    private void OnMatchMade(MatchmakerPollingResult result)
    {
        switch (result)
        {
            case MatchmakerPollingResult.Success:
                queueStatusText.text = "Connecting...";
                break;
            case MatchmakerPollingResult.TicketCreationError:
                queueStatusText.text = "TicketCreationError";
                break;
            case MatchmakerPollingResult.TicketCancellationError:
                queueStatusText.text = "TicketCancellationError";
                break;
            case MatchmakerPollingResult.TicketRetrievalError:
                queueStatusText.text = "TicketRetrievalError";
                break;
            case MatchmakerPollingResult.MatchAssignmentError:
                queueStatusText.text = "MatchAssignmentError";
                break;
        }
    }

    private async void StartHost()
    {
        if (isBusy) return;
        isBusy = true;
        await HostSingleton.Instance.GameManager.StartHostAsync(privateToggle.isOn);
        isBusy = false;
    }
    private async void StartClient()
    {
        if (isBusy) return;
        isBusy = true;
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
        isBusy = false;
    }
    public async void JoinAsync(Lobby lobby)
    {
        if (isBusy) return;
        isBusy = true;
        try
        {
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value;
            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        isBusy = false;
    }

}
