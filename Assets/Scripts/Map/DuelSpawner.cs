using UnityEngine;

namespace TDAWork.Map
{
    public class DuelSpawner : MonoBehaviour
    {
        public static DuelSpawner singleton; // Публичный указатель на класс

        [SerializeField] private GameObject playerPrefab; // Префаб игрока
        [SerializeField] private GameObject enemyPrefab; // Префаб противник

        [SerializeField] private Transform playerSpawnTransform; // Трансформ точки спавна игрока
        [SerializeField] private Transform enemySpawnTransform; // Трансформ точки спавна противника

        private GameObject SpawnedPlayer; // Переменная заспавненного игрока
        private GameObject SpawnedEnemy; // Переменная заспавненного противника

        private void Start() // Инициализация скрипта
        {
            singleton = this; // Указание значения переменной

            SpawnedPlayer = Instantiate(playerPrefab, playerSpawnTransform.position, playerPrefab.transform.rotation); // Указание значения переменной
            SpawnedEnemy = Instantiate(enemyPrefab, enemySpawnTransform.position, enemyPrefab.transform.rotation); // Указание значения переменной
        }

        public void Respawn() // Метод перезапуска карты
        {
            Destroy(SpawnedPlayer); SpawnedPlayer = Instantiate(playerPrefab, playerSpawnTransform.position, playerPrefab.transform.rotation); // Перезапуск игрока 
            Destroy(SpawnedEnemy); SpawnedEnemy = Instantiate(enemyPrefab, enemySpawnTransform.position, enemyPrefab.transform.rotation); // Перезапуск противника 

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet"); // Поиск патронов
            for (int i = 0; i < bullets.Length; i++) Destroy(bullets[i]); // Удаление патронов
        }
    }
}
