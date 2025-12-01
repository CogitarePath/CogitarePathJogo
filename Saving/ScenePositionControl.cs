using UnityEngine;

public class ScenePositionControl : MonoBehaviour
{
    public static Vector3 sceneStartPosition;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = sceneStartPosition;
    }

}
