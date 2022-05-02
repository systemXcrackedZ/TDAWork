using UnityEngine;

namespace TDAWork.UI
{
    public class RicochetDrawer : MonoBehaviour
    {
        private LineRenderer lineRenderer; // Переменная траектории

        private void Awake() => lineRenderer = GetComponent<LineRenderer>(); // Указание значения переменной траектории

        public void DrawTrajectory(Vector3[] points) // Отрисовка траектории
        {
            lineRenderer.positionCount = points.Length; // Указание количества точек
            lineRenderer.SetPositions(points); // Указание позиции траектории
        }
    }
}