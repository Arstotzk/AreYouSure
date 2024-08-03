using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject focusPoint;
    public TMP_Text textDialog;
    public TMP_Text personDialog;
    public string personName;
    public Animator animatorDialog;
    public List<DialogLine> welcomeDialogs;
    public DialogLine areYouSure;
    public DialogLine findRepeat;
    public DialogLine youAreSmart;
    public DialogLine youAreWrong;
    public List<DialogLine> endDialogs;
    public List<Item> items;
    public Item needItem;
    public int needItemIndex;
    public GameObject buttons;
    public Animator endAnimator;
    public float textDelaySpeed = 0.05f;
    public GameObject dropHint;
    public AudioSource showDialogSound;
    public AudioSource addendaSound;
    public DialogEnd dialogEnd;

    Mesh mesh;
    Vector3[] vertices;

    protected bool dialogStarted;
    protected bool dialogNeedToEnd;
    protected bool firstShow;
    protected DialogPhase phase;
    private int welcomeDialogsNum;
    private int endDialogsNum;
    private int startColorIndex;
    private int endColorIndex;
    private TurnOfLight lightOf;
    DialogLine _currentDialogLine;
    private AudioSource audioSource;
    private int dialogCalls = 0;

    private DialogLine currentDialogLine 
    {
        get 
        {
            return _currentDialogLine;
        }
        set 
        {
            _currentDialogLine = value;
            //textDialog.text = _currentDialogLine.text;
            StartCoroutine(PrintText(_currentDialogLine.text));

            if (_currentDialogLine.audioClip != null)
            {
                audioSource.clip = _currentDialogLine.audioClip;
                audioSource.Play();
            }

            if (_currentDialogLine.addendaClip != null)
            {
                addendaSound.clip = _currentDialogLine.addendaClip;
                addendaSound.Play();
            }
        } 
    }

    void Start()
    {
        phase = DialogPhase.NotStarted;
        dialogStarted = false;
        dialogNeedToEnd = false;
        firstShow = true;
        welcomeDialogsNum = 0;
        endDialogsNum = 0;
        needItemIndex = 0;
        audioSource = GetComponent<AudioSource>();
        lightOf = GetComponent<TurnOfLight>();
    }

    // Update is called once per frame
    void Update()
    {
        SetEffects(textDialog);
        SetEffects(personDialog);
        
    }

    void SetEffects(TMP_Text tMP_Text)
    {
        tMP_Text.ForceMeshUpdate();

        mesh = tMP_Text.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = Woubble(Time.time + i);
            vertices[i] = vertices[i] + offset;

        }
        mesh.vertices = vertices;
        tMP_Text.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Woubble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.0f), Mathf.Cos(time * 3.0f));
    }

    private IEnumerator PrintText(string text) 
    {
        dialogCalls++;
        var currentCall = dialogCalls;
        //clear text
        if (string.IsNullOrEmpty(text))
        {
            textDialog.text = "           ";
            yield break;
        }
        yield return new WaitForSeconds(0.4f);
        textDialog.text = string.Empty;
        var isMarking = false;
        for (var i = 0; i < text.Length; i++) 
        {
            if (currentCall < dialogCalls)
                yield break;
            textDialog.text += text[i];
            if (text[i].ToString() == "<")
                isMarking = true;
            if (text[i].ToString() == ">")
                isMarking = false;
            if (!isMarking)
                yield return new WaitForSeconds(textDelaySpeed);
        }
        audioSource.Stop();
    }
    public void StartDialog(GameObject player) 
    {
        showDialogSound.Play();
        dialogStarted = true;
        player.GetComponent<PlayerMovement>().OnDialog(focusPoint, this);
        personDialog.text = personName;

        if (phase == DialogPhase.NotStarted)
            phase = DialogPhase.Welcome;
        else if (phase == DialogPhase.Welcome && needItem != null)
            phase = DialogPhase.FindRepeat;

        animatorDialog.SetBool("IsShow", true);
    }
    public void ExitDialog(GameObject player) 
    {
        player.GetComponent<PlayerMovement>().ExitDialog();
        animatorDialog.SetBool("IsShow", false);
        firstShow = true;
        StartCoroutine(PrintText(string.Empty));
    }

    public void WelcomeDialog()
    {
        if (welcomeDialogsNum < welcomeDialogs.Count)
        {
            currentDialogLine = welcomeDialogs[welcomeDialogsNum];
            welcomeDialogsNum++;
        }
        else
        {
            dialogNeedToEnd = true;
            if(items.Count > 0)
                needItem = items[0];
        }
    }

    public void FindRepeat()
    {
        if (firstShow == true)
        {
            var findRepeatItem = new DialogLine();
            findRepeatItem.audioClip = findRepeat.audioClip;
            findRepeatItem.text = string.Format(findRepeat.text, needItem.itemName);
            currentDialogLine = findRepeatItem;
            firstShow = false;
        }
        else
        {
            dialogNeedToEnd = true;
        }
    }
    public void AreYouSure()
    {
        if (firstShow == true)
        {
            currentDialogLine = areYouSure;
            firstShow = false;
            //TODO отобрадение кнопок анимация
            buttons.SetActive(true);
            phase = DialogPhase.FindedItem;
        }
    }
    public void InteractSure(GameObject player, bool isSure) 
    {
        if (phase != DialogPhase.FindedItem)
            return;

        buttons.SetActive(false);
        var isMimic = player.GetComponent<InteractEnvironment>().itemInHands.isMimic;
        var itemName = player.GetComponent<InteractEnvironment>().itemInHands.itemName;
        if (isMimic && isSure)
        {
            dropHint.SetActive(false);
            player.GetComponent<PlayerHealth>().Damage();
            YouWrong();
            if (player.GetComponent<PlayerHealth>().health <= 0)
                phase = DialogPhase.EndDead;
            else
                phase = DialogPhase.FindNextItem;
        }
        if (!isMimic && isSure)
        {
            dropHint.SetActive(false);
            YouSmart();
            phase = DialogPhase.FindNextItem;
        }
        if (!isSure)
        {
            phase = DialogPhase.FindRepeat;
            ExitDialog(player);
            dialogStarted = false;
            dialogNeedToEnd = false;
        }
        if (itemName == "lightbulb")
        {
            lightOf.TurnOn();
        }
    }
    public void YouSmart()
    {
        currentDialogLine = youAreSmart;
    }
    public void YouWrong()
    {
        currentDialogLine = youAreWrong;
    }
    public void FindNextItem(GameObject player) 
    {
        needItemIndex++;
        if (needItemIndex < items.Count)
        {
            needItem = items[needItemIndex];
            Debug.Log("needItem: " + needItem.itemName);
            if (needItem.itemName == "lightbulb")
                lightOf.TurnOff();
            firstShow = true;
            Destroy(player.GetComponent<InteractEnvironment>().itemInHands.gameObject);
            FindRepeat();
            phase = DialogPhase.FindRepeat;
        }
        else
        {
            //todo end
            //ExitDialog(player);
            phase = DialogPhase.EndDead;
        }
    }
    public void EndDead(GameObject player)
    {
        if(player.GetComponent<InteractEnvironment>().itemInHands != null)
            Destroy(player.GetComponent<InteractEnvironment>().itemInHands.gameObject);

        endAnimator.SetBool("End", true);
        if (endDialogsNum < endDialogs.Count)
        {
            currentDialogLine = endDialogs[endDialogsNum];
            endDialogsNum++;
        }
        else
        {
            dialogEnd.PutPlayer(player);
            ExitDialog(player);
            player.GetComponent<PlayerMovement>().DisableCharacterController();
            endAnimator.SetBool("End", false);
        }
    }
    public virtual void Interact(GameObject player)  
    {
        if (dialogStarted == false)
        {
            StartDialog(player);
        }

        if (phase == DialogPhase.Welcome)
        {
            WelcomeDialog();
        }
        var itemInHands = player.GetComponent<InteractEnvironment>().itemInHands;
        if (phase == DialogPhase.FindRepeat && itemInHands != null)
        {
            if (itemInHands.itemName == needItem.itemName)
                AreYouSure();
            else
                FindRepeat(); //TODO Сказать что нужно Х, а не У
        }
        else if (phase == DialogPhase.FindNextItem)
        {
            FindNextItem(player);
        }
        else if (phase == DialogPhase.FindedItem)
        {
            //Нужно выбрать
        }
        else if (phase == DialogPhase.FindRepeat)
        {
            FindRepeat();
        }
        else if (phase == DialogPhase.EndDead)
        {
            EndDead(player);
        }

        if (dialogStarted == true && dialogNeedToEnd == true)
        {
            ExitDialog(player);
            dialogStarted = false;
            dialogNeedToEnd = false;
        }
    }
    protected enum DialogPhase
    {
        NotStarted,
        Welcome,
        FindRepeat,
        FindNextItem,
        FindedItem,
        EndDead
    }
}
