using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;      // 玩家对象
    private Vector3 offset;        // 相机与玩家的初始偏移

    void Start()
    {
        // 记录相机与玩家之间的初始偏移
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // 使用初始偏移，使相机保持固定跟随玩家
        transform.position = player.transform.position + offset;
    }
}
