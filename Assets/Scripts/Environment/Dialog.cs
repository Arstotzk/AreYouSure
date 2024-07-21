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

    Mesh mesh;
    Vector3[] vertices;

    private bool dialogStarted;
    private bool dialogNeedToEnd;
    private bool firstShow;
    private DialogPhase phase;
    private int welcomeDialogsNum;
    private int startColorIndex;
    private int endColorIndex;
    DialogLine _currentDialogLine;
    private DialogLine currentDialogLine 
    {
        get 
        {
            return _currentDialogLine;
        }
        set 
        {
            _currentDialogLine = value;
            textDialog.text = _currentDialogLine.text;
            startColorIndex = _currentDialogLine.text.IndexOf(_currentDialogLine.colorWord);
            if (startColorIndex == -1)
                endColorIndex = startColorIndex;
            else
                endColorIndex = startColorIndex + _currentDialogLine.colorWord.Length;
        } 
    }

    void Start()
    {
        phase = DialogPhase.NotStarted;
        dialogStarted = false;
        dialogNeedToEnd = false;
        firstShow = true;
        welcomeDialogsNum = 0;
        startColorIndex = -1;
        endColorIndex = startColorIndex;
        needItemIndex = 0;
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
    public void StartDialog(GameObject player) 
    {
        dialogStarted = true;
        player.GetComponent<PlayerMovement>().OnDialog(focusPoint, this);

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
            needItem = items[0];
        }
    }

    public void FindRepeat()
    {
        if (firstShow == true)
        {
            var findRepeatItem = findRepeat;
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
        if (isMimic && isSure)
        {
            //TODO damage player
            player.GetComponent<PlayerHealth>().Damage();
            YouWrong();
            phase = DialogPhase.FindNextItem;
        }
        if (!isMimic && isSure)
        {
            //TODO next step
            YouSmart();
            //FindNextItem(player);
            phase = DialogPhase.FindNextItem;
        }
        if (!isSure)
        {
            phase = DialogPhase.FindRepeat;
            ExitDialog(player);
            dialogStarted = false;
            dialogNeedToEnd = false;
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
            firstShow = true;
            Destroy(player.GetComponent<InteractEnvironment>().itemInHands.gameObject);
            FindRepeat();
            phase = DialogPhase.FindRepeat;
        }
        else
        {
            //todo end
            ExitDialog(player);
        }
    }
    public void Interact(GameObject player)  
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

        if (dialogStarted == true && dialogNeedToEnd == true)
            ExitDialog(player);

        dialogStarted = false;
        dialogNeedToEnd = false;

    }
    private enum DialogPhase
    {
        NotStarted,
        Welcome,
        FindRepeat,
        FindNextItem,
        FindedItem,
        EndDead
    }
}
