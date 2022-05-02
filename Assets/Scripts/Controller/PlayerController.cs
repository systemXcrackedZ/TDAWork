using UnityEngine;
using System.Collections;

using TDAWork.UI;

namespace TDAWork.Contoller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject BulletPrefab; // ������ ����

        [SerializeField] private float MovementSpeed; // ���������� �������� �����������
        [SerializeField] private float RotationSpeed; // ���������� �������� ��������
        [SerializeField] private float ShotDelay; // ���������� �� ��������

        private bool isShot = false; // ���������� ���������� �� ��������
        private RicochetDrawer Trajectory; // ����� ����������

        private void Awake() => Trajectory = GameObject.Find("RicochetDrawer").GetComponent<RicochetDrawer>(); // ������������ �������� ����� ����������

        private void FixedUpdate() // ����������� � ���������� �� ������������� ����������� ������
        {
            if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * (Time.deltaTime * MovementSpeed)); // ����������� �����
            else if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * (Time.deltaTime * MovementSpeed)); // ����������� �����

            if (Input.GetKey(KeyCode.A)) transform.rotation *= Quaternion.AngleAxis(RotationSpeed, Vector3.down); // �������� � ����� �������
            if (Input.GetKey(KeyCode.D)) transform.rotation *= Quaternion.AngleAxis(RotationSpeed, Vector3.up); // �������� � ������ �������
        }

        private void Update() // ��������� �� ������ �����
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward); // ��������� ����������� ����� ������������ ���������

            RaycastHit[] hits = Physics.RaycastAll(transform.position, forward); // ����� ���� �������� �� ������� ������� ��������
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject != gameObject) // ���� ������ �� ������� �����
                    Trajectory.DrawTrajectory(new Vector3[2] { transform.Find("mesh").position, forward *= hit.distance }); //������ ���������� ������������ �������
                else // �����
                    Trajectory.DrawTrajectory(new Vector3[2] { transform.Find("mesh").position, forward *= 8 }); //������ ������ ����������
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isShot) // ����� ������ � ������� �� � ��
            {
                isShot = true; // ������� ���������� � ��
                StartCoroutine(Shot()); // �������
            }
        }

        private IEnumerator Shot() // ������� ��������
        {
            Bullet bullet = Instantiate(BulletPrefab, transform.Find("mesh").position, transform.rotation).AddComponent<Bullet>(); // �������� ����
            bullet.EnemyObject = GameObject.Find("EnemyPrefab(Clone)"); // ���� ��� ������ ������� ��������(� ������ ������ ���������)

            yield return new WaitForSeconds(ShotDelay); // ����������� �� ��������

            isShot = false; // �� �������� �������
        }
    }
}