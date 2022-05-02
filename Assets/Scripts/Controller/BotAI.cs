using UnityEngine;
using System.Collections;

namespace TDAWork.Contoller
{
    public class BotAI : MonoBehaviour
    {
        public static BotAI singleton;  // ��������� ��������� �� �����

        [SerializeField] private float MovementSpeed; // �������� �����������
        [SerializeField] private float RotationSpeed; // �������� ��������
        [SerializeField] private GameObject BulletPrefab; // ������ ����

        [HideInInspector] public bool isReverseMoving; // �������� ��������?
        [HideInInspector] public int currentWaypoint; // ������� ��������

        private GameObject[] waypoints; // ���������
        private float lastWaypointSwitchTime; // ��������� ����� �������� �� ������ �����

        private readonly Vector3[] directions = new Vector3[2] { Vector3.up, Vector3.down }; // ����������� ��������

        private void Start() // ������������� �������
        {
            singleton = this; // ������������ �������� ��������� �� �����
            waypoints = new GameObject[10]; InitWaypoints(); lastWaypointSwitchTime = Time.time; // ������������� ����������
            StartCoroutine(FindShotTarget()); // ������ ������ ����
        }
        private void InitWaypoints() // ������������� ����������
        {
            for (int i = 0; i < waypoints.Length; i++) waypoints[i] = GameObject.Find($"WayPoint ({i})"); // ����� ����������
        }

        private void Update() // ��������� �� ������ �����
        {
            Vector3 startPosition = waypoints[currentWaypoint].transform.position; // ������� �����
            Vector3 endPosition = !isReverseMoving ? waypoints[currentWaypoint + 1].transform.position : waypoints[currentWaypoint - 1].transform.position; // �������� �����

            float pathLength = Vector3.Distance(startPosition, endPosition); // ��������� �� ����� �� �����
            float totalTimeForPath = pathLength / MovementSpeed; // ����� ��� ����������� �������
            float currentTimeOnPath = Time.time - lastWaypointSwitchTime; // ����� �� ����������� �� ����� �� �����

            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath); // �����������

            if (Vector3.Distance(transform.position, endPosition) < 0.1f) // �������� ����� �� �����?
            {
                if (isReverseMoving) { currentWaypoint--; if (currentWaypoint == 0) isReverseMoving = false; } // ����� ��������� �����
                else { currentWaypoint++; if (currentWaypoint == 9) isReverseMoving = true; } // ����� ��������� �����

                StartCoroutine(Rotate()); lastWaypointSwitchTime = Time.time; // ��������
            }
        }

        private IEnumerator Rotate() // ��������
        {
            Vector3 direction = directions[Random.Range(0, directions.Length)]; // ����� �����������

            int rotationCount = (int)(90 / RotationSpeed); // ���������� ��������

            for (int i = 0; i < rotationCount; i++) // ���� ��������
            {
                transform.rotation *= Quaternion.AngleAxis(RotationSpeed, direction); // ��������
                yield return new WaitForSeconds(0.025f); // �������� ��������� ��������
            }
        }

        private IEnumerator FindShotTarget() // ����� ������� ��� ��������
        {
            while (true) // ����������� ����
            {
                RaycastHit hit; // ���������� ���������� �������
                Vector3 forward = transform.TransformDirection(Vector3.forward); // ��������� ����������� ����� ������������ ���������

                if (Physics.Raycast(transform.Find("mesh").position, forward, out hit)) if (hit.collider.gameObject.name == "PlayerPrefab(Clone)") Shot(hit.collider.gameObject); // �������

                yield return new WaitForSeconds(0.5f); // �������� ����� ��������
            }
        }

        private void Shot(GameObject @object) // �������
        {
            Bullet bullet = Instantiate(BulletPrefab, transform.position, transform.rotation).AddComponent<Bullet>(); // �������� ����
            bullet.OwnerIsBot = true; // �������� ���� ��� ����
            bullet.EnemyObject = @object; // �������� ����� ��� ����
        }
    }
}