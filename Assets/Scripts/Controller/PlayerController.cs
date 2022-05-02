using UnityEngine;
using System.Collections;

using TDAWork.UI;

namespace TDAWork.Contoller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject BulletPrefab; // Префаб пули

        [SerializeField] private float MovementSpeed; // Переменная скорости перемещения
        [SerializeField] private float RotationSpeed; // Переменная скорости поворота
        [SerializeField] private float ShotDelay; // Переменная КД выстрела

        private bool isShot = false; // Логическая переменная КД выстрела
        private RicochetDrawer Trajectory; // Линия траектории

        private void Awake() => Trajectory = GameObject.Find("RicochetDrawer").GetComponent<RicochetDrawer>(); // Присваивание значение линии траектории

        private void FixedUpdate() // Перемещение с обработкой на фиксированном количеством кадров
        {
            if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * (Time.deltaTime * MovementSpeed)); // Перемещение вперёд
            else if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * (Time.deltaTime * MovementSpeed)); // Перемещение назад

            if (Input.GetKey(KeyCode.A)) transform.rotation *= Quaternion.AngleAxis(RotationSpeed, Vector3.down); // Вращение в левую сторону
            if (Input.GetKey(KeyCode.D)) transform.rotation *= Quaternion.AngleAxis(RotationSpeed, Vector3.up); // Вращение в правую сторону
        }

        private void Update() // Обработка на каждом кадре
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward); // Получение направления вперёд относительно персонажа

            RaycastHit[] hits = Physics.RaycastAll(transform.position, forward); // Поиск всех объектов на которые смотрит персонаж
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject != gameObject) // если объект не текущий игрок
                    Trajectory.DrawTrajectory(new Vector3[2] { transform.Find("mesh").position, forward *= hit.distance }); //рисует траекторию относительно объекта
                else // иначе
                    Trajectory.DrawTrajectory(new Vector3[2] { transform.Find("mesh").position, forward *= 8 }); //рисует прямую траекторию
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isShot) // Нажат пробел и выстрел не в КД
            {
                isShot = true; // Выстрел становится в КД
                StartCoroutine(Shot()); // Выстрел
            }
        }

        private IEnumerator Shot() // Коротин выстрела
        {
            Bullet bullet = Instantiate(BulletPrefab, transform.Find("mesh").position, transform.rotation).AddComponent<Bullet>(); // Создаётся пуля
            bullet.EnemyObject = GameObject.Find("EnemyPrefab(Clone)"); // Пуле как объект задаётся аппонент(в данном случае компьютер)

            yield return new WaitForSeconds(ShotDelay); // Выполняется КД выстрела

            isShot = false; // КД выстрела спадает
        }
    }
}