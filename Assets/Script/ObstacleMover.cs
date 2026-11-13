using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float destroyXPosition = -15f; // 画面左端の外側

    void Update()
    {
        // 毎フレーム左へ移動
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 画面外に出たらメモリ解放のために削除
        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }
}