using UnityEngine;
using Unity.Netcode;

public class PlayerStateManager : NetworkBehaviour
{
    private struct PlayerNetworkState : INetworkSerializable {
        private float x, y;
        private bool direction;
        public bool isRunning, isCrouching, isJumping;
        public float health;

        internal Vector2 Position {
            get => new(x, y);
            set {
                x = value.x;
                y = value.y;
            }
        }

        internal int Direction {
            get => direction ? 1 : -1;
            set {
                direction = value > 0;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref y);
            serializer.SerializeValue(ref direction);
            serializer.SerializeValue(ref isRunning);
            serializer.SerializeValue(ref isCrouching);
            serializer.SerializeValue(ref health);
        }
    }

    [SerializeField] private bool serverAuth;
    [SerializeField] private float interpolationTime = 0.1f;
    private PlayerScript playerScript;
    private NetworkVariable<PlayerNetworkState> networkState;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Vector2 interpolateVel;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        var permission = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        networkState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
        playerScript = GetComponent<PlayerScript>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(transform.GetComponent<PlayerController>());
        } else {
            Camera.main.gameObject.GetComponent<CameraFollow>().followed = transform;
            RegisterPlayerServerRpc();
            playerScript.nick = LocalPlayerInfo.nick;
            playerScript.SetNickDisplay();
            if (IsClient) {
                RequestSetNickServerRpc(LocalPlayerInfo.nick);
            } else {
                SetNickClientRpc(LocalPlayerInfo.nick);
            }
        }
    }

    private void Update() {
        if (IsOwner) {
            SendState();
        } else {
            SetState();
        }
    }

    public void DestroyIfOwner() {
        if (IsOwner) {
            DestoryServerRpc();
        }
    }

    [ServerRpc]
    private void DestoryServerRpc() {
        Destroy(transform.parent.gameObject);
    }

    [ServerRpc]
    private void RequestSetNickServerRpc(string nick) {
        SetNickClientRpc(nick);
    }

    [ClientRpc]
    private void SetNickClientRpc(string nick) {
        if (!IsOwner) {
            this.playerScript.nick = nick;
            playerScript.SetNickDisplay();
        }
    }

    [ServerRpc]
    private void RegisterPlayerServerRpc() {

    }

    private void SendState() {
        PlayerNetworkState state = new PlayerNetworkState {
            Position = rb.position,
            Direction = playerScript.Direction,
            health = playerScript.Health,
            isRunning = playerScript.isRunning,
            isCrouching = playerScript.isCrouchnig,
            isJumping = playerScript.isJumping
        };

        if (IsServer || !serverAuth) {
            networkState.Value = state;
        } else {
            SendStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void SendStateServerRpc(PlayerNetworkState state) {
        if (gameManager.GetGameState() == GameState.ACTIVE) {
            networkState.Value = state;
        }
    }

    private void SetState() {
        playerScript.isRunning = networkState.Value.isRunning;
        playerScript.isCrouchnig = networkState.Value.isCrouching;
        playerScript.isJumping = networkState.Value.isJumping;
        playerScript.Direction = networkState.Value.Direction;
        playerScript.Health = networkState.Value.health;
        rb.position = networkState.Value.Position;
    }

}
 