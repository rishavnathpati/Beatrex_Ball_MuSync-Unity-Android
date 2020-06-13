using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothTime = 0.3f;
    public float yOffset;

    Vector2 velocity = Vector2.zero;

    void Update()
    {
        Vector2 targetPos = player.transform.TransformPoint(new Vector3(0, yOffset));

        if (targetPos.y < transform.position.y)
        {
            return;
        }

        targetPos = new Vector2(0, targetPos.y);
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
