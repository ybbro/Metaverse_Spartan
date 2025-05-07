using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_MiniGame : MonoBehaviour
{
    const string
        // �̴ϰ��� �� �̸��� "MiniGame + ����"
        name_targetScene = "MiniGame",
        // �̴ϰ��� �ְ��� ����س��� key �̸��� "Best_MiniGame + ����"
        key_BestScore_common = "Best_MiniGame";

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI bestScore;

    int sceneIndex_Selected;

    private void OnEnable()
    {
        // ���� ��ũ���� ���� ����
        scrollRect.verticalNormalizedPosition = 1;

        // ���� ������ �߸��� ������ ���� �ʵ���
        sceneIndex_Selected = -1;
    }

    // �̴� ���� UI ����
    public void Button_Exit()
    {
        gameObject.SetActive(false);
    }

    public void Button_SelectGame(int index)
    {
        // �ٽ� �����ϸ� �ش� �̴ϰ��� ���� ����
        if (index == sceneIndex_Selected)
        {
            // ���� ���õ� UI ��� ������ ���� ����
            EventSystem.current.SetSelectedGameObject(null);
            bestScore.text = "0";
            sceneIndex_Selected = -1;
        }
        else
        {
            // �ش� �̴ϰ����� �ְ��� ǥ��
            bestScore.text = PlayerPrefs.GetInt(key_BestScore_common + index.ToString(), 0).ToString();
            // ������ �� ���
            sceneIndex_Selected = index;
        }
    }

    // ���� ��ư : ������ �̴ϰ��� ����
    public void SceneChange()
    {
        if(sceneIndex_Selected >= 0)
            SceneManager.LoadScene(name_targetScene + sceneIndex_Selected.ToString());
    }
}
