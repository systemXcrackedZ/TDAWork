using UnityEngine;

namespace TDAWork.UI
{
    public class RicochetDrawer : MonoBehaviour
    {
        private LineRenderer lineRenderer; // ���������� ����������

        private void Awake() => lineRenderer = GetComponent<LineRenderer>(); // �������� �������� ���������� ����������

        public void DrawTrajectory(Vector3[] points) // ��������� ����������
        {
            lineRenderer.positionCount = points.Length; // �������� ���������� �����
            lineRenderer.SetPositions(points); // �������� ������� ����������
        }
    }
}