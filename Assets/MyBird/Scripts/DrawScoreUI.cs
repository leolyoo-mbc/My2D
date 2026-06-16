using TMPro;
using UnityEngine;

namespace MyBird
{
    public class DrawScoreUI : MonoBehaviour
    {
        public TMP_Text score;

        private void Update()
        {
            score.text = GameManager.Instance.Score.ToString();
        }
    }
}