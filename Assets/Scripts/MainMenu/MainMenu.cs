using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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
    private bool isMatchmaking;
    private bool isCanceling;
    void Start()
    {
        if(ClientSingleton.Instance == null) return;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        queueStatusText.text = string.Empty;
        queueTimerText.text = string.Empty;
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public async void FindMatchPressed()
    {
        if(isCanceling) return;
        if(isMatchmaking)
        {
            queueStatusText.text = "Canceling...";
            isCanceling = true;
            //Cancel matchmaking
            isCanceling = false;
            isMatchmaking = false;
            findMatchButtonText.text = "Find Match";
            queueStatusText.text = string.Empty;
;            return;
        }
        //start queue
        findMatchButtonText.text = "Cancel";
        queueStatusText.text = "Searching...";
        isMatchmaking = true;
    }
    private async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }
    private async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
}
