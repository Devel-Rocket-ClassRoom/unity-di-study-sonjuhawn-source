using DIStudy.CoinClicker.Student;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DIStudy.DudageClicker.Student
{
    public class DudageClicker : MonoBehaviour
    {
        [SerializeField]
        private LayerMask m_ClickMask = ~0;

        private void Update()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Camera.main == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_ClickMask))
                return;

            Dudage dudage = hit.collider.GetComponentInParent<Dudage>();
            if (dudage != null)
                dudage.Collect();
        }
    }
}
