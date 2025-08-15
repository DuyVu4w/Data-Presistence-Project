using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // Start is called before the first frame update

    private SaveData data = new SaveData();
    private void Awake()
    {
        // check main scene is exist player name obj
        GameObject playerNameController = GameObject.Find("Player Name");
        if (playerNameController == null)
        {
            // return menu sence if not exist player name
            SceneManager.LoadScene(0);
        }
    }

    void Start()
    {
        LoadBestScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void LateUpdate()
    {
        if (m_Started && m_Points > data.points)
        {
            SaveBestScore();
            LoadBestScore();
        }    
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.playerName = PlayerNameController.instance.playerName;
        data.points = m_Points;

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/savefile2.json", json);
        Debug.Log(Application.persistentDataPath);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile2.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (json != "")
            {
                data = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                data.playerName = "";
                data.points = 0;
            }
            // Update Best Score Text
            BestScoreText.text = "Best score: " + data.points + " Name: " + data.playerName;
            return;
        }

        data.playerName = "";
        data.points = 0;
        BestScoreText.text = "Best score: " + data.points + " Name: " + data.playerName;
    }
        
    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int points;
    }
}
