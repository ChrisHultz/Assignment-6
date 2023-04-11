using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    private int passLives = 3;
    private int score = 0;

    public Text RankTxt;
    public Text ScoreTxt;
    public Text NameTxt;
    public Text inputText;
    public int num_scores = 10;
    public InputField input;
    public static string pass = "Player";

    public void Awake() {
        string path = "Assets/TxtFile/ScoresFile.txt";
        string line;
        string[] fields;
        string[] playerNames = new string[num_scores];
        int[] playerScores = new int[num_scores];
        int scores_read = 0;

        ScoreTxt.text = "";
        NameTxt.text = "";
        RankTxt.text = "";
        StreamReader reader = new StreamReader(path);
        int rank = 1;
        if (reader.Peek() == -1) {
            // Scores.txt file is empty
            RankTxt.text = "None";
            ScoreTxt.text = "None";
            NameTxt.text = "None";
        }
        else {
            while (!reader.EndOfStream && scores_read < num_scores) {
                line = reader.ReadLine();
                fields = line.Split(',');
                NameTxt.text += fields[0] + "\n";
                ScoreTxt.text += fields[1] + "\n";
                RankTxt.text += "#" + rank.ToString() + "\n";
                scores_read += 1;
                rank += 1;

            }
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene("MainGame");
        PlayerController.lives = passLives;
        PlayerController.carryScore = score;
    }

    public void inputName() {
        pass = input.text.ToString();
        inputText.text = pass.ToUpper();
    }
}