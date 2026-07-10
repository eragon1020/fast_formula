using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ResultSprite
{
    public int id;
    public Sprite sprite;
}

public class OrderManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ReactionManager reactionManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("UI")]
    [SerializeField] private TMP_Text orderNameText;
    [SerializeField] private GameObject clientText;
    [SerializeField] private Image orderImage;
    public GameObject orderClock;
    private Image clockImage;

    [Header("Sprites")]
    [SerializeField] private List<ResultSprite> resultSprites;

    public ReactionData currentOrder;
    private List<ReactionData> validReactions;

    public float timeRemaining = 15.0f;
    public bool isDone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clockImage = orderClock.GetComponent<Image>();

        validReactions = reactionManager.GetAllReactions().FindAll(reaction => reaction.type == "Success");

        GenerateOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            OrderFailed();
        }

        if (timeRemaining < 5)
        {
            clockImage.color = Color.red;
        }

        SetActiveClock();
    }

    public void GenerateOrder()
    {
        currentOrder = validReactions[Random.Range(0, validReactions.Count)];

        timeRemaining = 15f;
        isDone = false;
        orderClock.SetActive(true);
        clockImage.color = Color.white;
        clientText.SetActive(false);

        UpdateOrderUI();
    }

    private void UpdateOrderUI()
    {
        orderNameText.text = currentOrder.resultName;

        ResultSprite sprite = resultSprites.Find(sprite => sprite.id == currentOrder.id);

        if (sprite != null) orderImage.sprite = sprite.sprite;
    }

    void OrderFailed()
    {
        isDone = true;

        scoreManager.OnOrderExpired();

        clientText.SetActive(true);

        Invoke(nameof(GenerateOrder), 2f);
    }

    void OrderSuccess()
    {
        isDone = true;

        scoreManager.OnCorrectMix(timeRemaining);

        Invoke(nameof(GenerateOrder), 2f);
    }

    void OrderIncorrect()
    {
        isDone = true;

        scoreManager.OnWrongMix(timeRemaining);

        Invoke(nameof(GenerateOrder), 2f);
    }

    public ReactionData GetCurrentOrder()
    {
        return currentOrder;
    }

    public void SetActiveClock()
    {
        if (isDone)
        {
            orderClock.SetActive(false);
            clockImage.color = Color.white;
        }
    }

    public void CheckOrder(ReactionData reaction)
    {
        if (isDone) return;

        switch (reaction.type)
        {
            case "Success":

                if (reaction.id == currentOrder.id)
                    OrderSuccess();
                else
                    OrderIncorrect();

                break;

            case "Fail":

                OrderIncorrect();
                break;
        }
    }
}
