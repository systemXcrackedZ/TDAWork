using UnityEngine;
using System.Collections;

namespace TDAWork.Contoller
{
    public class BotAI : MonoBehaviour
    {
        public static BotAI singleton;  // Публичный указатель на класс

        [SerializeField] private float MovementSpeed; // Скорость перемещения
        [SerializeField] private float RotationSpeed; // Скорость вращения
        [SerializeField] private GameObject BulletPrefab; // Префаб пули

        [HideInInspector] public bool isReverseMoving; // Обратное движение?
        [HideInInspector] public int currentWaypoint; // Текущий чекпоинт

        private GameObject[] waypoints; // Чекпоинты
        private float lastWaypointSwitchTime; // Последнее время перехода на другую точку

        private readonly Vector3[] directions = new Vector3[2] { Vector3.up, Vector3.down }; // Направления вращения

        private void Start() // Инициализация скрипта
        {
            singleton = this; // Присваивание значения указателя на класс
            waypoints = new GameObject[10]; InitWaypoints(); lastWaypointSwitchTime = Time.time; // Инициализация чекпоинтов
            StartCoroutine(FindShotTarget()); // Запуск поиска цели
        }
        private void InitWaypoints() // Инициализация чекпоинтов
        {
            for (int i = 0; i < waypoints.Length; i++) waypoints[i] = GameObject.Find($"WayPoint ({i})"); // Поиск чекпоинтов
        }

        private void Update() // Обработка на каждом кадре
        {
            Vector3 startPosition = waypoints[currentWaypoint].transform.position; // Текущая точка
            Vector3 endPosition = !isReverseMoving ? waypoints[currentWaypoint + 1].transform.position : waypoints[currentWaypoint - 1].transform.position; // Конечная точка

            float pathLength = Vector3.Distance(startPosition, endPosition); // Дистанция от точки до точки
            float totalTimeForPath = pathLength / MovementSpeed; // Время для прохождения отрезка
            float currentTimeOnPath = Time.time - lastWaypointSwitchTime; // Время на прохождение от точки до точки

            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath); // Перемещение

            if (Vector3.Distance(transform.position, endPosition) < 0.1f) // Персонаж дошёл до точки?
            {
                if (isReverseMoving) { currentWaypoint--; if (currentWaypoint == 0) isReverseMoving = false; } // Выбор следующей точки
                else { currentWaypoint++; if (currentWaypoint == 9) isReverseMoving = true; } // Выбор следующей точки

                StartCoroutine(Rotate()); lastWaypointSwitchTime = Time.time; // Вращение
            }
        }

        private IEnumerator Rotate() // Вращение
        {
            Vector3 direction = directions[Random.Range(0, directions.Length)]; // Выбор направления

            int rotationCount = (int)(90 / RotationSpeed); // Количество вращений

            for (int i = 0; i < rotationCount; i++) // Цикл вращения
            {
                transform.rotation *= Quaternion.AngleAxis(RotationSpeed, direction); // Вращение
                yield return new WaitForSeconds(0.025f); // Задержка плавности вращения
            }
        }

        private IEnumerator FindShotTarget() // Поиск объекта для выстрела
        {
            while (true) // Бесконечный цикл
            {
                RaycastHit hit; // Переменная найденного объекта
                Vector3 forward = transform.TransformDirection(Vector3.forward); // Получение направления вперёд относительно персонажа

                if (Physics.Raycast(transform.Find("mesh").position, forward, out hit)) if (hit.collider.gameObject.name == "PlayerPrefab(Clone)") Shot(hit.collider.gameObject); // Выстрел

                yield return new WaitForSeconds(0.5f); // Задержка после выстрела
            }
        }

        private void Shot(GameObject @object) // Выстрел
        {
            Bullet bullet = Instantiate(BulletPrefab, transform.position, transform.rotation).AddComponent<Bullet>(); // Создание пули
            bullet.OwnerIsBot = true; // Указание бота для пули
            bullet.EnemyObject = @object; // Указание врага для пули
        }
    }
}
