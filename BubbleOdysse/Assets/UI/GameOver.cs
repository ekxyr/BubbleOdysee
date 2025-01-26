using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public VisualElement ui;

    public Button playButton;
    public Button audioButton;
    public Button quitButton;
    [SerializeField] GameObject _HideMouseParent;
    [SerializeField] GameObject _HideMouse;
    public Boolean hide = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        playButton = ui.Q<Button>("Start");
        print("funktioniert");
        playButton.clicked += OnPlayButtonClicked;


        audioButton = ui.Q<Button>("Audio");
        audioButton.clicked += OnAudioButtonClicked;

        quitButton = ui.Q<Button>("Quit");
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void OnAudioButtonClicked()
    {
        Debug.Log("Audio!");
    }

    private void OnPlayButtonClicked()
    {
        hide = true;
        if (hide == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        gameObject.SetActive(false);

    }
}