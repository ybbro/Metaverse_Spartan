using UnityEngine;
namespace MiniGame1
{
    public class DestroyZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Rubble")
                Destroy(collision.gameObject); // Destroy�� ������Ʈ �Ӹ��� �ƴ϶� ���µ� ���� �� �ִٰ� �Ѵ�. ��뿡 ����
        }
    }
}