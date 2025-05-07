using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_MiniGame : MonoBehaviour
{
    const string
        // 미니게임 씬 이름은 "MiniGame + 숫자"
        name_targetScene = "MiniGame",
        // 미니게임 최고점 기억해놓은 key 이름은 "Best_MiniGame + 숫자"
        key_BestScore_common = "Best_MiniGame";

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI bestScore;

    int sceneIndex_Selected;

    private void OnEnable()
    {
        // 수직 스크롤을 가장 위로
        scrollRect.verticalNormalizedPosition = 1;

        // 음수 값으로 잘못된 시작을 하지 않도록
        sceneIndex_Selected = -1;
    }

    // 미니 게임 UI 끄기
    public void Button_Exit()
    {
        gameObject.SetActive(false);
    }

    public void Button_SelectGame(int index)
    {
        // 다시 선택하면 해당 미니게임 선택 해제
        if (index == sceneIndex_Selected)
        {
            // 현재 선택된 UI 요소 강제로 선택 해제
            EventSystem.current.SetSelectedGameObject(null);
            bestScore.text = "0";
            sceneIndex_Selected = -1;
        }
        else
        {
            // 해당 미니게임의 최고점 표시
            bestScore.text = PlayerPrefs.GetInt(key_BestScore_common + index.ToString(), 0).ToString();
            // 선택한 씬 기억
            sceneIndex_Selected = index;
        }
    }

    // 시작 버튼 : 선택한 미니게임 시작
    public void SceneChange()
    {
        if(sceneIndex_Selected >= 0)
            SceneManager.LoadScene(name_targetScene + sceneIndex_Selected.ToString());
    }
}
