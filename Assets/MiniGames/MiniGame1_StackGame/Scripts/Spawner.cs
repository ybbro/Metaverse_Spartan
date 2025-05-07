using UnityEngine;
namespace MiniGame1
{
    public class Spawner : MonoBehaviour
    {
        const float
            boundSize = 3.5f, // 블럭의 사이즈 (블럭 프리팹의 x,z 스케일이 3.5f)
            movingBoundsSize = 3f, // 이동량?
            stackSpeed = 5f,
            blockSpeed = 3.5f,
            errorMargin = 0.5f; // 성공으로 취급할 마진

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

            // 초기의 랜덤한 컬러값들
            prevColor = GetRandomColor();
            nextColor = GetRandomColor();

            // 2개를 생성하여 하나는 가만히, 두번째 생성한 블럭은 움직이게끔
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
                    // 게임오버
                    //Debug.Log("게임 오버");
                    // 끝났을 때 한번만
                    UpdateRecord();
                    isGameOver = true;
                    GameOverEffect();
                    // 결과 UI 출력
                    UIManager.Instance.SetScoreUI();
                }
            }

            MoveBlock();

            // Spawn_Block() 이후 desiredPosition이 변하면 움직임이 보이게끔
            // 클릭 안할때는 연산하지 않도록 보통은 클릭할 때 코루틴으로 빼긴 했는데
            // 클릭하면 코루틴 멈추고 다시 경신된 desiredPosition으로 Lerp 하는 코루틴을 돌린다던지
            // 이렇게 쓰는 건 코드 작성할 때는 편하네요
            transform.position = Vector3.Lerp(transform.position, desiredPosition, stackSpeed * Time.deltaTime);
        }

        bool Spawn_Block()
        {
            if (lastBlock)
                prevBlockPosition = lastBlock.localPosition;

            // 이렇게 주면 2번째 파라미터는 월드 좌표계인가? 부모를 기준으로 한 로컬 좌표계인가? >> 실험 해보니 월드 포지션
            // 여기서는 로컬 좌표인지 결정하는 bool 값을 넣을 수 없기에 몇개의 변수는 밖으로 빼야할 듯
            // Transform newTrans = Instantiate(block, prevBlockPosition + Vector3.up, Quaternion.identity, transform);
            Transform newTrans = Instantiate(block, transform);
            newTrans.localPosition = prevBlockPosition + Vector3.up;
            newTrans.localRotation = Quaternion.identity;
            newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.z); // 위에 쌓이는 스텍 블럭의 스케일 조정
            ColorChange(newTrans.gameObject);

            // 1스텍 적립
            stackCount++;

            // 스텍이 쌓여도 높이가 올라가는 게 아닌 기존 블럭 전체를 1칸씩 내려준다는 느낌으로 부모 오브젝트를 1칸 내려주기
            desiredPosition = Vector3.down * stackCount;
            blockTransition = 0f;

            lastBlock = newTrans;

            // 이동 시작
            isMovingX = !isMovingX;

            // 점수 표시 변경
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
            // 값이 낮으면 딥다크한 색이 나오기에 적당히 100부터 시작!
            return Random.Range(100f, 250f) / 255f;
        }

        void ColorChange(GameObject go)
        {
            // 10 단위로 색이 바뀌고 점점 그 색상으로 바뀌게끔
            Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

            if (go.TryGetComponent(out Renderer rn))
            {
                rn.material.color = applyColor;
                // 카메라 배경 컬러도 변경. 그러나 완전히 같으면 배경색과 구분이 되지 않기에 약간의 차이 두기
                Camera.main.backgroundColor = applyColor - new Color() * 0.1f;

                // 10단계를 쌓았다면 기존의 nextColor 값이 되므로, 다음 랜덤한 컬러가 나오게끔
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
            // 0~length(2번째 매개변수)까지 왔다갔다 하는 값을 리턴하는 함수 
            // 자체적으로 증가하는 t(1번째 매개변수) 필요

            // 블럭이 기존의 쌓인 블럭 위를 왔다갔다 하게끔 하기 위해 핑퐁을 사용
            // length의 절반을 빼서 양수 음수를 왔다갔다하는 직선 그래프(사인 그래프와는 모양, 움직임이 다름)
            float movePos = Mathf.PingPong(blockTransition, boundSize) - boundSize / 2;

            if (isMovingX)
                lastBlock.localPosition = new Vector3(movePos * movingBoundsSize, stackCount, secondaryPosition);
            else
                lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, movePos * movingBoundsSize);
        }

        bool PlaceBlock()
        {
            // 이번 블럭의 로컬 위치
            Vector3 lastPos = lastBlock.localPosition;

            // 어느 축 방향으로 움직이는지
            // xz평면, y축은 높이
            // x == 0, z == 2
            // 기본은 z축 방향 이동값
            int axisIndex = 2;
            // x축 방향 이동하면 값 변경
            if (isMovingX)
                axisIndex = 0;

            // 이번에 블록을 쌓을 때 오차의 양
            // lastPos[0] == lastPos.x, lastPos[2] == lastPos.z
            float delta = Mathf.Abs(prevBlockPosition[axisIndex] - lastPos[axisIndex]);
            // 오차가 음수인지 >> 떨어지는 조각 생성 방향
            bool isNegative = delta < 0;

            // 정확히 쌓기에 실패
            if (delta > errorMargin)
            {
                float stackBounds_orgin = stackBounds[axisIndex];
                // 오차만큼 다음에 생성될 블럭의 사이즈 조정
                stackBounds[axisIndex] -= delta;
                // 잘린 이후 해당 축의 사이즈가 0 이하가 되면 게임오버
                if (stackBounds[axisIndex] <= 0)
                    return false;

                // 잘려서 쌓일 블럭의 새로운 중점
                float middle = (prevBlockPosition[axisIndex] + lastPos[axisIndex]) / 2f;
                // 쌓은 블럭의 스케일을 조정(플레이어는 여기서 잘렸다고 느낌)
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.z);

                // 스케일 변화에 맞춰 이번에 쌓은 블럭의 좌표 경신
                Vector3 tempPos = lastBlock.localPosition;
                tempPos[axisIndex] = middle;
                lastBlock.localPosition = lastPos = tempPos;

                // 잘린 부분처럼 보이는 조각 생성
                // 조각의 중심점
                Vector3 rubblePos = (isMovingX ? Vector3.right : Vector3.forward)
                                  * (lastPos[axisIndex] + (isNegative ? -stackBounds_orgin / 2 : +stackBounds_orgin / 2)) // 잘린 부분으로 이동해주기 위한 좌표
                                  + Vector3.up * lastPos.y
                                  + (isMovingX ? Vector3.forward : Vector3.right) * lastPos[Mathf.Abs(axisIndex - 2)];

                Vector3 rubbleScale = isMovingX ? new Vector3(delta, 1, stackBounds.z) : new Vector3(stackBounds.x, 1, delta);
                CreateRubble(rubblePos, rubbleScale);

                // 콤보 초기화
                comboCount = 0;
            }
            // 쌓기에 성공
            else
            {
                // 콤보 체크
                ComboCheck();
                // y축 좌표 +1(블록 두께는 1 고정)
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }

            // 다음 블럭 움직임의 중심점
            secondaryPosition = lastBlock.localPosition[axisIndex];

            return true;
        }

        // 잘려서 떨어지는 연출용 블럭 생성
        // 사실 왔다갔다 하는 블럭을 자르는 게 아니라
        // 눈속임으로 크기를 줄이고, 남은 크기만큼 블럭 하나를 더 만들어 떨어뜨리는 것!
        void CreateRubble(Vector3 pos, Vector3 scale)
        {
            // 직전에 쌓은 블럭을 복제
            Transform go = Instantiate(lastBlock, transform);

            // 잘린 것처럼 보이게 위치, 스케일 적용(회전값은 그냥 동일하게끔)
            go.localPosition = pos;
            go.localScale = scale;
            go.localRotation = Quaternion.identity;

            // 자유낙하가 가능하도록 리지드바디 컴포넌트 추가
            go.gameObject.AddComponent<Rigidbody>();
            // 오브젝트 이름 변경
            go.name = Rubble_Name;
        }

        void ComboCheck()
        {
            if (++comboCount > maxCombo)
                maxCombo = comboCount;

            // 5콤보에 성공할 때마다 다음 블럭 크기를 늘리게끔..
            // 그런데 늘려도 중심을 제대로 못맞추면 똑같지 않나??
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
                //Debug.Log("최고 점수 경신");
                bestScore = stackCount;
                PlayerPrefs.SetInt(key_BestScore, bestScore);
            }

            if (PlayerPrefs.GetInt(BestCombo_Key, 0) < maxCombo)
            {
                //Debug.Log("최고 콤보 경신");
                PlayerPrefs.SetInt(BestCombo_Key, maxCombo);
            }
        }

        void GameOverEffect()
        {
            // 자식 오브젝트 수
            int childCount = transform.childCount;

            // 왜 20개지?? >> 상단 20개 블럭 오브젝트를 대상
            for (int i = 1; i < 20; i++)
            {
                // i보다 자식 수가 적다면 더 진행할 필요가 없기에 루프 종료
                if (childCount < i) break;
                GameObject go = transform.GetChild(childCount - i).gameObject;

                // 떨어져나간 조각들은 대상으로 하지 않음
                // 리지드바디를 가지고 있는지 여부로 판단
                if (!go.TryGetComponent(out Rigidbody rigid))
                    // 쌓은 블럭들이면 리지드바디를 붙이고 힘을 줘서 확실히 날리도록
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