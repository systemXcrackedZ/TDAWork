using UnityEngine;
using UnityEngine.UI;

namespace TDAWork.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager singleton; // Публичный указатель на класс

        [SerializeField] private Text countText; // Счётчик очков

        private int playerPoints; // Переменная очков игрока
        private int enemyPoints; // Переменная очков противника

        private void Awake() => singleton = this; // Присваивание значения указателя на класс

        public int IncreasePoints(bool isBot) // Изменение очков
        {
            int newCount = isBot ? enemyPoints++ : playerPoints++; // Указание нового количества очков
            countText.text = $"{playerPoints}:{enemyPoints}"; // Задание текста счётчика очков
            return newCount; // Возвращения нового количества очков
        }
    }
}