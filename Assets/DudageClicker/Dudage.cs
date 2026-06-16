using System;
using UnityEngine;
using VContainer;

namespace DIStudy.DudageClicker.Student 
{
    public class Dudage : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_CollectClip;

        private Vector3 m_HidePos; 
        private Vector3 m_ShowPos;
        public float m_Speed = 1f;
        private float m_T;

        private bool m_IsUp = false;
        private bool m_IsHiding = false;
        private float m_AliveTimer;
        [SerializeField] private float m_AliveTime = 2f;

        private IScoreService m_Score;
        private IAudioService m_Audio;
        private MyGameConfig m_Config;
        private bool m_Collected;

        public event Action<Dudage> Collected;


        [Inject]
        public void Construct(IScoreService score, IAudioService audio, MyGameConfig config)
        {
            m_Score = score;
            m_Audio = audio;
            m_Config = config;
        }

        public void Init(Transform hole)
        {
            m_ShowPos = hole.position;                        
            m_HidePos = hole.position + Vector3.down * 1.2f; 
            transform.position = m_HidePos;                  
        }

        private void Update()
        {
            if (!m_IsUp)
            {
                // 올라오는 중
                m_T += Time.deltaTime * m_Speed;
                transform.position = Vector3.Lerp(m_HidePos, m_ShowPos, m_T);
                if (m_T >= 1f)
                {
                    m_T = 1f;
                    m_IsUp = true;
                }
                return;
            }

            if (!m_IsHiding)
            {
                // 대기 중
                m_AliveTimer += Time.deltaTime;
                if (m_AliveTimer >= m_AliveTime)
                    m_IsHiding = true;
                return;
            }

            // 내려가는 중
            m_T -= Time.deltaTime * m_Speed;
            transform.position = Vector3.Lerp(m_HidePos, m_ShowPos, m_T);
            if (m_T <= 0f)
            {
                Collected?.Invoke(this);
                Destroy(gameObject);
            }
        }

        public void Collect()
        {
            if (m_Collected)
                return;

            if (m_Score == null)
            {
                Debug.LogWarning("[Coin] 주입되지 않았습니다 — IObjectResolver.Instantiate로 생성했는지 확인하세요.");
                return;
            }

            m_Collected = true;
            m_Score.Add(m_Config.CoinValue);
            m_Audio.PlaySoundEffect(m_CollectClip);
            Collected?.Invoke(this);
            Destroy(gameObject);
        }
    }

}

