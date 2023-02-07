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
    private Rigidbody2D rb;
    private Vector2 interpolateVel;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        var permission = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        networkState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
        playerScript = GetComponent<PlayerScript>();
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(transform.GetComponent<PlayerController>());
        } else {
            Camera.main.gameObject.GetComponent<CameraFollow>().followed = transform;
        }
    }

    private void Update() {
        if (IsOwner) {
            SendState();
        } else {
            SetPosition();
        }
        SetState();
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
        networkState.Value = state;
    }

    private void SetState() {
        playerScript.isRunning = networkState.Value.isRunning;
        playerScript.isCrouchnig = networkState.Value.isCrouching;
        playerScript.isJumping = networkState.Value.isJumping;
        playerScript.Direction = networkState.Value.Direction;
    }

    private void SetPosition() {
        //rb.MovePosition(Vector2.SmoothDamp(rb.position, networkState.Value.Position, ref interpolateVel, interpolationTime));
        rb.position = networkState.Value.Position;
    }
    
}
 