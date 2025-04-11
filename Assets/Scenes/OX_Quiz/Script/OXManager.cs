using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OXManager : MonoBehaviour
{
    public QuestionData questionData;
    public GameObject EEGManager1;
    public GameObject EEGManager2;
    public GameObject EEGManager3;
    public GameObject EEGManager4;
    public TMP_Text questionText;
    public TMP_Text scoreText;
    public TMP_Text topicText;
    public TMP_Text levelText;
    public Button oButton;
    public Button xButton;
    public bool currAns = true;

    private int score = 0;
    private List<QuestionData.Question> questions;
    private QuestionData.Question currentQuestion;

    void Start()
    {
        questions = new List<QuestionData.Question>(questionData.questions); // 복사
        scoreText.text = "점수: 0";

        oButton.onClick.AddListener(() => CheckAnswer(true));
        xButton.onClick.AddListener(() => CheckAnswer(false));

        NextQuestion();
    }

    void NextQuestion()
    {
        if (questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];
            questionText.text = currentQuestion.questionText;
            topicText.text = currentQuestion.topic;
            levelText.text = currentQuestion.level.ToString();
        }
        else
        {
            questionText.text = "게임 종료!";
            topicText.text = "";
            oButton.interactable = false;
            xButton.interactable = false;
        }
    }

    void CheckAnswer(bool playerChoice)
    {
        if (playerChoice == currentQuestion.isCorrect)
        {
            score += 10;
            scoreText.text = "점수: " + score;
            currAns = true;
        }
        else
        {
            currAns = false;
        }
        EEGManager1.GetComponent<EEGDataManager>().RecordEEG();
        EEGManager2.GetComponent<EEGDataManager>().RecordEEG();
        EEGManager3.GetComponent<EEGDataManager>().RecordEEG();
        EEGManager4.GetComponent<EEGDataManager>().RecordEEG();
        questions.Remove(currentQuestion);
        NextQuestion();
    }
}
