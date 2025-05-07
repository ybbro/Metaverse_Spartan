using UnityEngine;
namespace MiniGame1
{
    public class DestroyZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Rubble")
                Destroy(collision.gameObject); // Destroy로 오브젝트 뿐만이 아니라 에셋도 지울 수 있다고 한다. 사용에 주의
        }
    }
}