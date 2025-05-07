using UnityEngine;
namespace MiniGame1
{
    public class Spawner : MonoBehaviour
    {
        const float
            boundSize = 3.5f, // ���� ������ (�� �������� x,z �������� 3.5f)
            movingBoundsSize = 3f, // �̵���?
            stackSpeed = 5f,
            blockSpeed = 3.5f,
            errorMargin = 0.5f; // �������� ����� ����

        [SerializeField] Transform block;

        Vector3
            prevBlockPosition,
            desiredPosition,
            stackBounds = new Vector3(boundSize, 0, boundSize);

        Transform lastBlock;
        float
            blockTransition = 0,
            secondaryPosition = 0;

        int
            stackCount = -1,
            bestScore = 0,
            comboCount = 0,
            maxCombo = 0;

        public int Score { get => stackCount; }
        public int BestScore { get => bestScore; }
        public int Combo { get => comboCount; }
        public int MaxCombo { get => maxCombo; }

        Color
            prevColor,
            nextColor;

        bool isMovingX = true,
             isGameOver = true;
        const string
            key_BestScore = "Best_MiniGame1",
            BestCombo_Key = "BestCombo_MiniGame1",
            Rubble_Name = "Rubble";

        // Start is called before the first frame update
        void Start()
        {
            if (!block)
            {
                Debug.LogError("block is Null");
                return;
            }

            bestScore = PlayerPrefs.GetInt(key_BestScore, 0);
            maxCombo = PlayerPrefs.GetInt(BestCombo_Key, 0);

            // �ʱ��� ������ �÷�����
            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            // 2���� �����Ͽ� �ϳ��� ������, �ι�° ������ ���� �����̰Բ�
            prevBlockPosition = Vector3.down;
            Spawn_Block();
            Spawn_Block();
        }

        // Update is called once per frame
        void Update()
        {
            if (isGameOver)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceBlock())
                {
                    Spawn_Block();
                }
                else
                {
                    // ���ӿ���
                    //Debug.Log("���� ����");
                    // ������ �� �ѹ���
                    UpdateRecord();
                    isGameOver = true;
                    GameOverEffect();
                    // ��� UI ���
                    UIManager.Instance.SetScoreUI();
                }
            }

            MoveBlock();

            // Spawn_Block() ���� desiredPosition�� ���ϸ� �������� ���̰Բ�
            // Ŭ�� ���Ҷ��� �������� �ʵ��� ������ Ŭ���� �� �ڷ�ƾ���� ���� �ߴµ�
            // Ŭ���ϸ� �ڷ�ƾ ���߰� �ٽ� ��ŵ� desiredPosition���� Lerp �ϴ� �ڷ�ƾ�� �����ٴ���
            // �̷��� ���� �� �ڵ� �ۼ��� ���� ���ϳ׿�
            transform.position = Vector3.Lerp(transform.position, desiredPosition, stackSpeed * Time.deltaTime);
        }

        bool Spawn_Block()
        {
            if (lastBlock)
                prevBlockPosition = lastBlock.localPosition;

            // �̷��� �ָ� 2��° �Ķ���ʹ� ���� ��ǥ���ΰ�? �θ� �������� �� ���� ��ǥ���ΰ�? >> ���� �غ��� ���� ������
            // ���⼭�� ���� ��ǥ���� �����ϴ� bool ���� ���� �� ���⿡ ��� ������ ������ ������ ��
            // Transform newTrans = Instantiate(block, prevBlockPosition + Vector3.up, Quaternion.identity, transform);
            Transform newTrans = Instantiate(block, transform);
            newTrans.localPosition = prevBlockPosition + Vector3.up;
            newTrans.localRotation = Quaternion.identity;
            newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.z); // ���� ���̴� ���� ���� ������ ����
            ColorChange(newTrans.gameObject);

            // 1���� ����
            stackCount++;

            // ������ �׿��� ���̰� �ö󰡴� �� �ƴ� ���� �� ��ü�� 1ĭ�� �����شٴ� �������� �θ� ������Ʈ�� 1ĭ �����ֱ�
            desiredPosition = Vector3.down * stackCount;
            blockTransition = 0f;

            lastBlock = newTrans;

            // �̵� ����
            isMovingX = !isMovingX;

            // ���� ǥ�� ����
            UIManager.Instance.UpdateScore();

            return true;
        }

        Color GetRandomColor()
        {
            float r = GetRandomColorFactor();
            float g = GetRandomColorFactor();
            float b = GetRandomColorFactor();

            return new Color(r, g, b);
        }

        float GetRandomColorFactor()
        {
            // ���� ������ ����ũ�� ���� �����⿡ ������ 100���� ����!
            return Random.Range(100f, 250f) / 255f;
        }

        void ColorChange(GameObject go)
        {
            // 10 ������ ���� �ٲ�� ���� �� �������� �ٲ�Բ�
            Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

            if (go.TryGetComponent(out Renderer rn))
            {
                rn.material.color = applyColor;
                // ī�޶� ��� �÷��� ����. �׷��� ������ ������ ������ ������ ���� �ʱ⿡ �ణ�� ���� �α�
                Camera.main.backgroundColor = applyColor - new Color() * 0.1f;

                // 10�ܰ踦 �׾Ҵٸ� ������ nextColor ���� �ǹǷ�, ���� ������ �÷��� �����Բ�
                if (applyColor.Equals(nextColor))
                {
                    prevColor = nextColor;
                    nextColor = GetRandomColor();
                }
            }
        }

        void MoveBlock()
        {
            blockTransition += Time.deltaTime * blockSpeed;

            // Mathf.PingPong
            // 0~length(2��° �Ű�����)���� �Դٰ��� �ϴ� ���� �����ϴ� �Լ� 
            // ��ü������ �����ϴ� t(1��° �Ű�����) �ʿ�

            // ���� ������ ���� �� ���� �Դٰ��� �ϰԲ� �ϱ� ���� ������ ���
            // length�� ������ ���� ��� ������ �Դٰ����ϴ� ���� �׷���(���� �׷����ʹ� ���, �������� �ٸ�)
            float movePos = Mathf.PingPong(blockTransition, boundSize) - boundSize / 2;

            if (isMovingX)
                lastBlock.localPosition = new Vector3(movePos * movingBoundsSize, stackCount, secondaryPosition);
            else
                lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, movePos * movingBoundsSize);
        }

        bool PlaceBlock()
        {
            // �̹� ���� ���� ��ġ
            Vector3 lastPos = lastBlock.localPosition;

            // ��� �� �������� �����̴���
            // xz���, y���� ����
            // x == 0, z == 2
            // �⺻�� z�� ���� �̵���
            int axisIndex = 2;
            // x�� ���� �̵��ϸ� �� ����
            if (isMovingX)
                axisIndex = 0;

            // �̹��� ����� ���� �� ������ ��
            // lastPos[0] == lastPos.x, lastPos[2] == lastPos.z
            float delta = Mathf.Abs(prevBlockPosition[axisIndex] - lastPos[axisIndex]);
            // ������ �������� >> �������� ���� ���� ����
            bool isNegative = delta < 0;

            // ��Ȯ�� �ױ⿡ ����
            if (delta > errorMargin)
            {
                float stackBounds_orgin = stackBounds[axisIndex];
                // ������ŭ ������ ������ ���� ������ ����
                stackBounds[axisIndex] -= delta;
                // �߸� ���� �ش� ���� ����� 0 ���ϰ� �Ǹ� ���ӿ���
                if (stackBounds[axisIndex] <= 0)
                    return false;

                // �߷��� ���� ���� ���ο� ����
                float middle = (prevBlockPosition[axisIndex] + lastPos[axisIndex]) / 2f;
                // ���� ���� �������� ����(�÷��̾�� ���⼭ �߷ȴٰ� ����)
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.z);

                // ������ ��ȭ�� ���� �̹��� ���� ���� ��ǥ ���
                Vector3 tempPos = lastBlock.localPosition;
                tempPos[axisIndex] = middle;
                lastBlock.localPosition = lastPos = tempPos;

                // �߸� �κ�ó�� ���̴� ���� ����
                // ������ �߽���
                Vector3 rubblePos = (isMovingX ? Vector3.right : Vector3.forward)
                                  * (lastPos[axisIndex] + (isNegative ? -stackBounds_orgin / 2 : +stackBounds_orgin / 2)) // �߸� �κ����� �̵����ֱ� ���� ��ǥ
                                  + Vector3.up * lastPos.y
                                  + (isMovingX ? Vector3.forward : Vector3.right) * lastPos[Mathf.Abs(axisIndex - 2)];

                Vector3 rubbleScale = isMovingX ? new Vector3(delta, 1, stackBounds.z) : new Vector3(stackBounds.x, 1, delta);
                CreateRubble(rubblePos, rubbleScale);

                // �޺� �ʱ�ȭ
                comboCount = 0;
            }
            // �ױ⿡ ����
            else
            {
                // �޺� üũ
                ComboCheck();
                // y�� ��ǥ +1(��� �β��� 1 ����)
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }

            // ���� �� �������� �߽���
            secondaryPosition = lastBlock.localPosition[axisIndex];

            return true;
        }

        // �߷��� �������� ����� �� ����
        // ��� �Դٰ��� �ϴ� ���� �ڸ��� �� �ƴ϶�
        // ���������� ũ�⸦ ���̰�, ���� ũ�⸸ŭ �� �ϳ��� �� ����� ����߸��� ��!
        void CreateRubble(Vector3 pos, Vector3 scale)
        {
            // ������ ���� ���� ����
            Transform go = Instantiate(lastBlock, transform);

            // �߸� ��ó�� ���̰� ��ġ, ������ ����(ȸ������ �׳� �����ϰԲ�)
            go.localPosition = pos;
            go.localScale = scale;
            go.localRotation = Quaternion.identity;

            // �������ϰ� �����ϵ��� ������ٵ� ������Ʈ �߰�
            go.gameObject.AddComponent<Rigidbody>();
            // ������Ʈ �̸� ����
            go.name = Rubble_Name;
        }

        void ComboCheck()
        {
            if (++comboCount > maxCombo)
                maxCombo = comboCount;

            // 5�޺��� ������ ������ ���� �� ũ�⸦ �ø��Բ�..
            // �׷��� �÷��� �߽��� ����� �����߸� �Ȱ��� �ʳ�??
            if (comboCount % 5 == 0)
            {
                Debug.Log("5 Combo Success!");
                stackBounds += new Vector3(0.5f, 0.5f);
                stackBounds.x =
                    (stackBounds.x > boundSize) ? boundSize : stackBounds.x;
                stackBounds.y =
                    (stackBounds.y > boundSize) ? boundSize : stackBounds.y;
            }
        }

        void UpdateRecord()
        {
            if (bestScore < stackCount)
            {
                //Debug.Log("�ְ� ���� ���");
                bestScore = stackCount;
                PlayerPrefs.SetInt(key_BestScore, bestScore);
            }

            if (PlayerPrefs.GetInt(BestCombo_Key, 0) < maxCombo)
            {
                //Debug.Log("�ְ� �޺� ���");
                PlayerPrefs.SetInt(BestCombo_Key, maxCombo);
            }
        }

        void GameOverEffect()
        {
            // �ڽ� ������Ʈ ��
            int childCount = transform.childCount;

            // �� 20����?? >> ��� 20�� �� ������Ʈ�� ���
            for (int i = 1; i < 20; i++)
            {
                // i���� �ڽ� ���� ���ٸ� �� ������ �ʿ䰡 ���⿡ ���� ����
                if (childCount < i) break;
                GameObject go = transform.GetChild(childCount - i).gameObject;

                // ���������� �������� ������� ���� ����
                // ������ٵ� ������ �ִ��� ���η� �Ǵ�
                if (!go.TryGetComponent(out Rigidbody rigid))
                    // ���� �����̸� ������ٵ� ���̰� ���� �༭ Ȯ���� ��������
                    go.AddComponent<Rigidbody>().AddForce((Vector3.up * Random.Range(5f, 10f) + Vector3.right * Random.Range(-5f, 5f)) * 100f);
            }
        }

        public void Restart()
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            isGameOver = false;

            lastBlock = null;
            desiredPosition = Vector3.zero;
            stackBounds = new Vector3(boundSize, 0, boundSize);

            stackCount = -1;
            isMovingX = true;
            blockTransition = 0f;
            secondaryPosition = 0f;

            comboCount = 0;
            maxCombo = 0;

            prevBlockPosition = Vector3.down;

            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            Spawn_Block();
            Spawn_Block();
        }
    }
}