using UnityEngine;

public class CamMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Update()
    {
        transform.position = new Vector3(player.position.x, 0f, -10f);
    }
}
