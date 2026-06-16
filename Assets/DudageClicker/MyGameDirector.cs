using DIStudy.DudageClicker.Student;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VContainer.Unity;

public sealed class MyGameDirector : IStartable, ITickable, IDisposable
{
    private readonly IScoreService m_Score;
    private readonly ISaveService m_Save;
    private readonly MyGameConfig m_Config;

    private float m_Elapsed;
    private const float k_GameTime = 30f;
    private bool m_GameOver = false;

    public event Action<float> TimerUpdated;


    public MyGameDirector(IScoreService score, ISaveService save, MyGameConfig config)
    {
        m_Score = score;
        m_Save = save;
        m_Config = config;
    }

    public void Start()
    {
        int bestScore = m_Save.LoadScore();
        Debug.Log($"[GameDirector] 시작 — 최고 점수 {bestScore}점");
    }

    public void Tick()
    {
        if (m_GameOver) return;

        m_Elapsed += Time.deltaTime;
        TimerUpdated?.Invoke(k_GameTime - m_Elapsed); 

        if (m_Elapsed >= k_GameTime)
        {
            m_GameOver = true;
            if (m_Score.CurrentScore > m_Save.LoadScore())
            {
                m_Save.SaveScore(m_Score.CurrentScore);
                Debug.Log($"[GameDirector] 새 최고 점수 — {m_Score.CurrentScore}점 저장");
            }
            else
            {
                Debug.Log($"[GameDirector] 게임 종료 — 최고 점수 갱신 실패 ({m_Score.CurrentScore}점)");
            }
        }
    }

    public void Dispose()
    {
        // 게임 종료 전에 앱을 끄면 저장 안 함 (최고점수만 저장하는 정책)
    }
}