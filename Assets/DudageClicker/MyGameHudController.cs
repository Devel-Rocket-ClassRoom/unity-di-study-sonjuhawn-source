using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace DIStudy.DudageClicker.Student
{
    public class MyGameHudController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_ScoreText;

        [SerializeField]
        private TextMeshProUGUI m_StatusText;

        [SerializeField]
        private TextMeshProUGUI m_TimerText;

        [SerializeField]
        private Button m_ResetButton;

        private MyGameDirector m_Director;

        private IScoreService m_Score;
        private ISaveService m_Save;

        [Inject]
        public void Construct(IScoreService score, ISaveService save, MyGameDirector director) 
        {
            m_Score = score;
            m_Save = save;
            m_Director = director;
        }

        private void Start()
        {
            if (m_Score == null)
            {
                SetStatus("<color=#ff5555>주입 실패</color> — LifetimeScope 등록을 확인하세요.");
                return;
            }

            m_Score.ScoreChanged += OnScoreChanged;
            m_Save.Saved += OnSaved;

            OnScoreChanged(m_Score.CurrentScore);
            SetStatus($"최고 점수: {m_Save.LoadScore()}점");

            m_Director.TimerUpdated += OnTimerUpdated;
        }

        private void OnDestroy()
        {
            if (m_Score != null)
                m_Score.ScoreChanged -= OnScoreChanged;
            if (m_Save != null)
                m_Save.Saved -= OnSaved;

            if (m_Director != null)
                m_Director.TimerUpdated -= OnTimerUpdated;
        }

        private void OnScoreChanged(int score)
        {
            if (m_ScoreText != null)
                m_ScoreText.text = $"점수: {score}";
        }

        private void OnSaved(int score)
        {
            SetStatus($"저장됨 — {score}점 (시각 {Time.time:F1}s)");
        }

        private void OnResetClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void SetStatus(string message)
        {
            if (m_StatusText != null)
                m_StatusText.text = message;
        }

        private void OnTimerUpdated(float remaining)
        {
            if (m_TimerText != null)
                m_TimerText.text = $"남은 시간: {Mathf.CeilToInt(remaining)}초";
        }
    }
}
