using UnityEngine;
using UnityEngine.UI;

namespace TDAWork.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager singleton; // ��������� ��������� �� �����

        [SerializeField] private Text countText; // ������� �����

        private int playerPoints; // ���������� ����� ������
        private int enemyPoints; // ���������� ����� ����������

        private void Awake() => singleton = this; // ������������ �������� ��������� �� �����

        public int IncreasePoints(bool isBot) // ��������� �����
        {
            int newCount = isBot ? enemyPoints++ : playerPoints++; // �������� ������ ���������� �����
            countText.text = $"{playerPoints}:{enemyPoints}"; // ������� ������ �������� �����
            return newCount; // ����������� ������ ���������� �����
        }
    }
}