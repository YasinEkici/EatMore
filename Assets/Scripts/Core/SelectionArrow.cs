using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private RectTransform[] arrowPositions;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform rect;
    private int currentPosition = 0;

    private void Start()
    {
        rect.position = new Vector3(arrowPositions[currentPosition].position.x, arrowPositions[currentPosition].position.y, 0);
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //change positions
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        //interact options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
        {
            SoundManager.instance.PlaySound(changeSound);
        }

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        rect.position = new Vector3(arrowPositions[currentPosition].position.x , arrowPositions[currentPosition].position.y  , 0);
    }

}
