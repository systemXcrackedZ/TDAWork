using UnityEngine;

namespace TDAWork.Map
{
    public class DuelSpawner : MonoBehaviour
    {
        public static DuelSpawner singleton; // ��������� ��������� �� �����

        [SerializeField] private GameObject playerPrefab; // ������ ������
        [SerializeField] private GameObject enemyPrefab; // ������ ���������

        [SerializeField] private Transform playerSpawnTransform; // ��������� ����� ������ ������
        [SerializeField] private Transform enemySpawnTransform; // ��������� ����� ������ ����������

        private GameObject SpawnedPlayer; // ���������� ������������� ������
        private GameObject SpawnedEnemy; // ���������� ������������� ����������

        private void Start() // ������������� �������
        {
            singleton = this; // �������� �������� ����������

            SpawnedPlayer = Instantiate(playerPrefab, playerSpawnTransform.position, playerPrefab.transform.rotation); // �������� �������� ����������
            SpawnedEnemy = Instantiate(enemyPrefab, enemySpawnTransform.position, enemyPrefab.transform.rotation); // �������� �������� ����������
        }

        public void Respawn() // ����� ����������� �����
        {
            Destroy(SpawnedPlayer); SpawnedPlayer = Instantiate(playerPrefab, playerSpawnTransform.position, playerPrefab.transform.rotation); // ���������� ������ 
            Destroy(SpawnedEnemy); SpawnedEnemy = Instantiate(enemyPrefab, enemySpawnTransform.position, enemyPrefab.transform.rotation); // ���������� ���������� 

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet"); // ����� ��������
            for (int i = 0; i < bullets.Length; i++) Destroy(bullets[i]); // �������� ��������
        }
    }
}