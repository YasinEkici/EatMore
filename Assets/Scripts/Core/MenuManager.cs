using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject settingsScreen;
    private int currentPosition;

    
    private void Start()
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

    public void ChangePosition(int change)
    {
        currentPosition += change;

        if (change != 0)
            SoundManager.instance.PlaySound(changeSound); 

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition(currentPosition);
    }
    private void AssignPosition(int position)
    {
            arrow.position = new Vector3(buttons[position].position.x -180, buttons[position].position.y + 30, 0);
    }
    private void Interact()
    {
        

        SoundManager.instance.PlaySound(interactSound);  

        if (currentPosition == 0 )
        {
            LoadGame();
        }
        else
        {
            Quit();
        }
       
            
    }

    public void Settings()
    {
        optionsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }
   public void LoadGame()
    {
        //Start game
        SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //executed only in editor
#endif

    }

}
