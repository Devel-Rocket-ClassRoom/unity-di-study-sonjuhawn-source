using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using VContainer;

public class UISingletone : MonoBehaviour
{
    public TMP_Text m_InfoText;
    public Button m_AddButton;
    public Button m_SceneButton;

    public string m_TargetSceneName;

    private GlobalCounterService m_Counter;

    [Inject]
    public void Construct(GlobalCounterService counter)
    {
        m_Counter = counter;
    }
    private void Start()
    {
        m_InfoText.text = $"{m_Counter.Id} : {m_Counter.Count}";

        m_AddButton.onClick.AddListener(OnClickAdd);
        m_SceneButton.onClick.AddListener(OnClickScene);
    }

    public void OnClickAdd()
    {
        m_Counter.Add(1);
        m_InfoText.text = $"{m_Counter.Id} : {m_Counter.Count}";
    }
    public void OnClickScene()
    {
        SceneManager.LoadScene(m_TargetSceneName);
    }
}
