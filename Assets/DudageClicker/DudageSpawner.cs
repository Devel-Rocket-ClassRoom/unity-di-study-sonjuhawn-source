using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DIStudy.DudageClicker.Student
{
    public class DudageSpawner
        : MonoBehaviour
    {
        [SerializeField]
        private Dudage m_DudagePrefab;

        [SerializeField]
        private GameObject[] m_Holes;

        private List<int> m_AvailableHoles;

        private IObjectResolver m_Resolver;
        private MyGameConfig m_Config;

        [Inject]
        public void Construct(IObjectResolver resolver, MyGameConfig config)
        {
            m_Resolver = resolver;
            m_Config = config;
        }

        private void Start()
        {
            if (m_DudagePrefab == null)
            {
                Debug.LogWarning("[CoinSpawner] Coin 프리팹이 연결되지 않았습니다.");
                return;
            }

            if (m_Resolver == null)
            {
                Debug.LogWarning("[CoinSpawner] 주입되지 않았습니다 — LifetimeScope 등록을 확인하세요.");
                return;
            }

            m_AvailableHoles = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            for (int i = 0; i < 3; i++)
                Spawn();
        }

        private void Spawn()
        {
            if (m_AvailableHoles.Count == 0) return;

            int randIndex = Random.Range(0, m_AvailableHoles.Count);
            int holeIndex = m_AvailableHoles[randIndex];
            m_AvailableHoles.RemoveAt(randIndex); // 점유 표시

            Dudage dudage = m_Resolver.Instantiate(m_DudagePrefab, m_Holes[holeIndex].transform.position, m_DudagePrefab.transform.rotation);
            dudage.Init(m_Holes[holeIndex].transform);
            dudage.Collected += (d) => {
                m_AvailableHoles.Add(holeIndex);
                StartCoroutine(RespawnAfterDelay());
            };
        }

        private void OnCoinCollected(Dudage coin)
        {
            coin.Collected -= OnCoinCollected;
            StartCoroutine(RespawnAfterDelay());
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(m_Config.RespawnDelay);
            Spawn();
        }
    }
}
