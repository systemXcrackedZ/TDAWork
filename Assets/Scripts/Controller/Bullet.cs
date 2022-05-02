using UnityEngine;

using TDAWork.UI;
using TDAWork.Map;

namespace TDAWork.Contoller
{
    public class Bullet : MonoBehaviour
    {
        public GameObject EnemyObject; // Вражеский объект
        public bool OwnerIsBot; // Владелец пули - бот?

        private Vector3 direction; // Направление

        private void Awake() => direction = Vector3.forward; // Направление - прямо.

        private void OnTriggerEnter(Collider other) // Вхождение в коллайдер.
        {
            if (other.gameObject.CompareTag("player") && other.gameObject == EnemyObject) // Коллайдер - игрок и вражеский объект
            {
                UIManager.singleton.IncreasePoints(OwnerIsBot); // Увеличение поинтов

                BotAI.singleton.isReverseMoving = false; // отключение движения бота
                BotAI.singleton.currentWaypoint = 0; // нулевые движения бота

                DuelSpawner.singleton.Respawn(); // переигровка
                Destroy(gameObject); // удаление пули
            }
            if (other.gameObject.CompareTag("border")) Destroy(gameObject); // удаление пули
            if (other.gameObject.CompareTag("cubeee")) // рикошет пули
            {
                direction = -direction; // Изменение направления
                transform.rotation *= Quaternion.AngleAxis(90, Vector3.up); // Вращение пули
            }
        }

        private void Update() => transform.Translate(direction * (Time.deltaTime * 10)); // движение пули
    }
}