using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private RectTransform[] arrowPositions;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject settingsScreen;
    private int currentPosition;

    private void Awake()
    {
        ChangePosition(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        else if (Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(1))
            Interact();

    }

    public void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition(currentPosition);
    }
    private void AssignPosition(int _position)
    {
        arrow.position = new Vector3(arrowPositions[_position].position.x, arrowPositions[_position].position.y, 0);
    }
    private void Interact()
    {


        SoundManager.instance.PlaySound(interactSound);

    
        if (currentPosition == 2)//exit
            Exit();
    }
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Exit()
    {
        settingsScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

}
