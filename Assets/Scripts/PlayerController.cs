using System;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Camera cam;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public static int carryScore = 0;
    public static int lives = 3;
    public int num_scores = 10;
    public GameObject objectToDisable;
	public GameObject objectToDisable2;
    public GameObject objectToDisable3;
    private bool pickedUp = false;
    private bool touched = false;
    public Text ScoreTxt;
    public Text livesTxt;
    
    public List<GameObject> CoinsList;
    private GameObject activePickUp;

    void Awake() {
        agent.updateRotation = false;
        GameObject parentCoins = GameObject.Find("Coins");

        foreach (Transform child in parentCoins.transform) {
            CoinsList.Add(child.gameObject);
        }

        foreach (GameObject pickUp in CoinsList) {
            pickUp.SetActive(false); 
        }

        livesTxt.text = "Lives: " + lives.ToString();
        ScoreTxt.text = "Score: " + carryScore.ToString();
        RandomCoinSpawn();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit)) {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.remainingDistance > agent.stoppingDistance) {
            character.Move(agent.desiredVelocity, false, false);
        }
        else {
            character.Move(Vector3.zero, false, false);
        }
    }

    void OnTriggerEnter(Collider other) {
        string path = "Assets/TxtFile/ScoresFile.txt";
        string line;
        string[] fields;
        int scores_written = 0;
        string newName = Menu.pass;
        string newScore = carryScore.ToString();
        string[] writeNames = new string[10];
        string[] writeScores = new string[10];
        bool newScoreWritten = false;
		 if (other.gameObject.CompareTag ("PickUp")) {
            if (pickedUp == false) {
                pickedUp = true;
                carryScore += 10;
                Invoke("SetPickUp", 0.5f);
            }
			other.gameObject.SetActive (false);
            
            ScoreTxt.text = "Score: " + carryScore.ToString();
            RandomCoinSpawn();
		}
        else if (other.gameObject.CompareTag("Enemy")) {
            if (lives > 1) {
                if (touched == false) {
                    touched = true;
                    lives--;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
			else {
                objectToDisable.SetActive(false);
			    objectToDisable2.SetActive(false);
                objectToDisable3.SetActive(false);
				StreamReader reader = new StreamReader(path);
				while (!reader.EndOfStream ) {
            		line = reader.ReadLine();
            		fields = line.Split(',');
					if (!newScoreWritten && scores_written < num_scores) {
						if(Convert.ToInt32(newScore) > Convert.ToInt32(fields[1])) {
							writeNames[scores_written] = newName;
                    		writeScores[scores_written] = newScore;
                    		newScoreWritten = true;
                    		scores_written += 1;
						}
					}
					if(scores_written < num_scores) { // we have not written enough lines yet
                		writeNames[scores_written] = fields[0];
                		writeScores[scores_written] = fields[1];
                		scores_written += 1;
					}
				}
				reader.Close();
				StreamWriter writer = new StreamWriter(path);
				for(int x = 0; x < scores_written; x++) {
					writer.WriteLine(writeNames[x] + ',' + writeScores[x]);
				}
				writer.Close();
        		AssetDatabase.ImportAsset(path);
        		TextAsset asset = (TextAsset)Resources.Load("scores");
				Invoke("ToExit", 0.5f);
			}
        }
	}

    void RandomCoinSpawn() {
        int randomIndex = UnityEngine.Random.Range(0, CoinsList.Count);
        activePickUp = CoinsList[randomIndex];
        activePickUp.SetActive(true);
    }

    void ToExit() {
        SceneManager.LoadScene("Exit");
    }
    void SetPickUp() {
        pickedUp = false;
    }
}
