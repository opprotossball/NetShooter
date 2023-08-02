using Mono.Cecil.Cil;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    private static readonly int CLIENTS_NUMBER = 1;
    private string createdCode = null;
    private async void Start() {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public string GetRelayCode() {
        return createdCode;
    }

    public async Task CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(CLIENTS_NUMBER);
            createdCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(createdCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            NetworkManager.Singleton.StartHost();
            MainSceneUI.instance.SetGameCode(createdCode);
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public async Task JoinRelay(string joinCode) {
        try {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            MainSceneUI.instance.SetGameCode(joinCode);
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
}
 