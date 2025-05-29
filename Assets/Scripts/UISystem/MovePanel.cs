using UnityEngine;

namespace UISystem
{
    public class MovePanel : MonoBehaviour
    {
        public float speed = 1.0f; // 你可以根据需要调整速度

        private void Update()
        {
            transform.Translate(Vector3.up * (speed * Time.unscaledDeltaTime));
        }
    }
}
