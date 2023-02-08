using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerStateManager playerState = animator.gameObject.GetComponent<PlayerStateManager>();
        if (playerState != null) {
            playerState.DestroyIfOwner();
        }
    }
}
