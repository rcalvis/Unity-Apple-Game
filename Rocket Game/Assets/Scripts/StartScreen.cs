using System.Collections.Generic;
using System.Threading;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public UIDocument uIDocument;
    private Button pinkManButton;
    private Button ninjaFrogButton;
    private Button virtualGuyButton;
    private Button maskDudeButton;
    private Image pinkManImage;
    private Image ninjaFrogImage;
    private Image virtualGuyImage;
    private Image maskDudeImage;
    private Button startButton;

    [SerializeField]
    private List<Sprite> pinkManIdle = new List<Sprite>();
    [SerializeField]
    private List<Sprite> ninjaFrogIdle = new List<Sprite>();
    [SerializeField]
    private List<Sprite> virtualGuyIdle = new List<Sprite>();
    [SerializeField]
    private List<Sprite> maskDudeIdle = new List<Sprite>();
    [SerializeField, Range(0.05f, 0.5f)] private float frameTime = 0.12f;

    private float timer;
    private int frameIndex;
    private Button selectedCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pinkManButton = uIDocument.rootVisualElement.Q<Button>("PinkMan");
        ninjaFrogButton = uIDocument.rootVisualElement.Q<Button>("NinjaFrog");
        virtualGuyButton = uIDocument.rootVisualElement.Q<Button>("VirtualGuy");
        maskDudeButton = uIDocument.rootVisualElement.Q<Button>("MaskDude");
        startButton = uIDocument.rootVisualElement.Q<Button>("StartButton");

        pinkManImage = pinkManButton.Q<Image>("PinkManImage");
        ninjaFrogImage = ninjaFrogButton.Q<Image>("NinjaFrogImage");
        virtualGuyImage = virtualGuyButton.Q<Image>("VirtualGuyImage");
        maskDudeImage = maskDudeButton.Q<Image>("MaskDudeImage");

        if (pinkManImage == null || ninjaFrogImage == null || virtualGuyImage == null || maskDudeImage == null)
        {
            Debug.Log("Idiot 1");
            enabled = false;
            return;
        }

        if (pinkManIdle == null || pinkManIdle.Count == 0 || ninjaFrogIdle == null || ninjaFrogIdle.Count == 0 || virtualGuyIdle == null || virtualGuyIdle.Count == 0 || maskDudeIdle == null || maskDudeIdle.Count == 0)
        {
            Debug.Log("Idiot 2");
            enabled = false;
            return;
        }

        frameIndex = 0;
        pinkManImage.sprite = pinkManIdle[frameIndex];
        ninjaFrogImage.sprite = ninjaFrogIdle[frameIndex];
        virtualGuyImage.sprite = virtualGuyIdle[frameIndex];
        maskDudeImage.sprite = maskDudeIdle[frameIndex];

        pinkManImage.pickingMode = PickingMode.Ignore;
        ninjaFrogImage.pickingMode = PickingMode.Ignore;
        virtualGuyImage.pickingMode = PickingMode.Ignore;
        maskDudeImage.pickingMode = PickingMode.Ignore;

        pinkManButton.clicked += () => SelectCharacter(pinkManButton);
        ninjaFrogButton.clicked += () => SelectCharacter(ninjaFrogButton);
        virtualGuyButton.clicked += () => SelectCharacter(virtualGuyButton);
        maskDudeButton.clicked += () => SelectCharacter(maskDudeButton);

        startButton.clicked += startGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (pinkManIdle == null || pinkManIdle.Count == 0 || ninjaFrogIdle == null || ninjaFrogIdle.Count == 0 || virtualGuyIdle == null || virtualGuyIdle.Count == 0 || maskDudeIdle == null || maskDudeIdle.Count == 0) 
        {
            Debug.Log("Idiot 3"); 
            return;
        }

        timer += Time.unscaledDeltaTime;
        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % pinkManIdle.Count;
            pinkManImage.sprite = pinkManIdle[frameIndex];
            ninjaFrogImage.sprite = ninjaFrogIdle[frameIndex];
            virtualGuyImage.sprite = virtualGuyIdle[frameIndex];
            maskDudeImage.sprite = maskDudeIdle[frameIndex];
        }
    }

    void SelectCharacter(Button selected)
    {
        Debug.Log("Character selected");
        if (selectedCharacter == null)
        {
            selectedCharacter = selected;
            Debug.Log(selectedCharacter);
            SetSelected(selected, true);
        } else if (selectedCharacter != selected)
        {
            SetSelected(selectedCharacter, false);
            selectedCharacter = selected;
            SetSelected(selected, true);
        } else
        {
            SetSelected(selected, false);
            selectedCharacter = null;
        }
    }

    void SetSelected(Button b, bool selected)
    {
        b.style.borderBottomColor = Color.gray;
        b.style.borderTopColor = Color.gray;
        b.style.borderRightColor = Color.gray;
        b.style.borderLeftColor = Color.gray;
        b.style.borderTopWidth = selected ? 3 : 0;
        b.style.borderBottomWidth = selected ? 3 : 0;
        b.style.borderLeftWidth = selected ? 3 : 0;
        b.style.borderRightWidth = selected ? 3: 0;
        b.style.borderTopLeftRadius = 2;
        b.style.borderTopRightRadius = 2;
        b.style.borderBottomLeftRadius = 2;
        b.style.borderBottomRightRadius = 2;
    }

    void startGame()
    {
        if (selectedCharacter != null)
        {
            PlayerPrefs.SetString("Character", selectedCharacter.name);
        } else
        {
            PlayerPrefs.SetString("Character", "PinkManButton");
        }
        SceneManager.LoadScene("Game");
    }

}
