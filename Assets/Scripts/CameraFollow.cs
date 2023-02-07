using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followed;
    void Update() {
        if (followed == null) {
            return;
        }
        Vector3 pos = transform.position;
        pos.x = followed.position.x;
        transform.position = pos;
    }
}
