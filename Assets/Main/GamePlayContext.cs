using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayContext : MonoBehaviour
{
    public List<PressMesh> shapes = new List<PressMesh>();

    public float diformationCofForNextShape = 0.001f;

    public Transform btnNextLvl;
    public Transform btnRestart;

    private int? _currentIndexShape;

    public void Start()
    {
        ShowNextShapes();
    }

    private void ShowNextShapes()
    {
        btnNextLvl.gameObject.SetActive(false);
        if (_currentIndexShape == null)
        {
            _currentIndexShape = 0;
            shapes[(int)_currentIndexShape].gameObject.SetActive(true);
            return;
        }     

        if (_currentIndexShape < shapes.Count - 1)
        {
            shapes[(int)_currentIndexShape].gameObject.SetActive(false);
            _currentIndexShape++;
            shapes[(int)_currentIndexShape].gameObject.SetActive(true);
            return;
        }
    }

    public void Update()
    {
        if (shapes[(int)_currentIndexShape].deformationÑoefficient <= diformationCofForNextShape)
        {
            if (_currentIndexShape != shapes.Count - 1)
                ShowBtnNextLvl();
            else
                ShowBtnRestart();
        }
        else
        {
            btnNextLvl.gameObject.SetActive(false);
            btnRestart.gameObject.SetActive(false);
        }
    }

    private void ShowBtnNextLvl()
    {
        btnNextLvl.gameObject.SetActive(true);
    }

    private void ShowBtnRestart()
    {
        btnRestart.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        ShowNextShapes();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
