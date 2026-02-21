using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class Customer : MonoBehaviour

{
    [Header("Navigation")]
    private NavMeshAgent agent;
    private Table assignedTable;
    private Transform exitPoint;

    private bool hasArrived = false;
    private bool isLeaving = false;

    [Header("Order")]
    public OrderData currentOrder;
    private bool hasOrdered = false;

    [Header("Timers")]
    public float serviceTime = 60f;
    public float foodWaitTime = 120f;

    private float currentTimer;
    private float maxTimer;

    private bool waitingForService = false;
    private bool waitingForFood = false;

    [Header("UI")]
    public TextMeshPro timerText;
    public TextMeshPro CustomerOrderText;

    private enum TipTier { None, Small, Big }
    private TipTier currentTipTier = TipTier.None;

    private Animator animator;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void AssignTable(Table table, Transform exit)
    {
        assignedTable = table;
        exitPoint = exit;

        agent.SetDestination(table.GetSpot().position);
    }

    void Update()
    {
        HandleArrival();
        HandleTimer();
        HandleExitArrival();
        UpdateAnimation();
        if (hasArrived && !isLeaving)
        {
            LookAtTable();
        }
    }

    void UpdateAnimation()
    {
        if (animator == null || agent == null)
            return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void LookAtTable()
    {
        if (assignedTable == null)
            return;

        Vector3 lookDir = assignedTable.GetSpot().position - transform.position;
        lookDir.y = 0;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            // Smooth käännös pöytään
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }



    // ===============================
    // ARRIVAL TO TABLE
    // ===============================
    void HandleArrival()
    {
        if (hasArrived || isLeaving)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasArrived = true;
            agent.isStopped = true;
            agent.updateRotation = false;
            OnArrivedAtTable();
        }
    }

    void OnArrivedAtTable()
    {
        Debug.Log("Customer arrived at table: " + assignedTable.tableNumber);

        waitingForService = true;
        StartTimer(serviceTime);
    }

    // ===============================
    // TIMER
    // ===============================
    void StartTimer(float time)
    {
        currentTimer = time;
        maxTimer = time;

        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    void HandleTimer()
    {
        if (!waitingForService && !waitingForFood)
            return;

        currentTimer -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTimer).ToString();
            UpdateTimerVisual();
        }

        if (currentTimer <= 0f)
        {
            TimerExpired();
        }
    }

    void UpdateTimerVisual()
    {
        float ratio = currentTimer / maxTimer;

        if (ratio >= 0.5f)
        {
            timerText.color = Color.green;
            currentTipTier = TipTier.Big;
        }
        else if (ratio > 0.25f)
        {
            timerText.color = Color.yellow;
            currentTipTier = TipTier.Small;
        }
        else
        {
            timerText.color = Color.red;
            currentTipTier = TipTier.None;
        }
    }

    void TimerExpired()
    {
        Debug.Log("Customer at table " + assignedTable.tableNumber + " got angry! -50");

        ScoreManager.Instance.AddScore(-50);

        waitingForService = false;
        waitingForFood = false;

        LeaveRestaurant();
    }

    int CalculateTip()
    {
        if (currentTipTier == TipTier.Big)
            return 50;
        else if (currentTipTier == TipTier.Small)
            return 20;
        else
            return 0;
    }

    // ===============================
    // INTERACTION
    // ===============================
    public bool Interact(PickupItem heldItem)
    {
        if (!hasArrived || isLeaving)
            return false;

        // Generate order
        if (!hasOrdered)
        {
            currentOrder = OrderManager.Instance.GenerateOrder();
            hasOrdered = true;

            string corderText =  
                        currentOrder.mainCourse.ingredientName + "\n" + 
                        currentOrder.side.ingredientName + "\n" +
                        currentOrder.salad.ingredientName + "\n" +
                        currentOrder.drink.ingredientName;
            CustomerOrderText.text = corderText;

            waitingForService = false;
            waitingForFood = true;

            StartTimer(foodWaitTime);
            
            string orderText = $"Table {assignedTable.tableNumber} | " + 
                        currentOrder.mainCourse.ingredientName + " " + 
                        currentOrder.side.ingredientName + " " +
                        currentOrder.salad.ingredientName + " " +
                        currentOrder.drink.ingredientName;

            Debug.Log(orderText);

            return false;
        }

        if (heldItem == null)
            return false;

        Tray tray = heldItem.GetComponent<Tray>();
        if (tray == null)
            return false;

        if (currentOrder.Matches(tray))
        {
            int tip = CalculateTip();
            int total = 100 + tip;

            Debug.Log("Correct order for table " + assignedTable.tableNumber +
                      " | Tip: " + tip +
                      " | Total: " + total);

            ScoreManager.Instance.AddScore(total);

            tray.ClearTray();
            Destroy(heldItem.gameObject);

            waitingForFood = false;
            hasOrdered = false;

            StartCoroutine(EatAndLeave());

            return true;
        }
        else
        {
            Debug.Log("Wrong order for table " + assignedTable.tableNumber + " -50");

            ScoreManager.Instance.AddScore(-50);
            Destroy(heldItem.gameObject);

            return false;
        }
    }

    // ===============================
    // EAT + EXIT
    // ===============================
    IEnumerator EatAndLeave()
    {
        if (timerText != null)
            timerText.text = "Eating...";
            CustomerOrderText.text = "";


        yield return new WaitForSeconds(10f);

        LeaveRestaurant();
    }

    public void LeaveRestaurant()
    {
        if (isLeaving)
            return;

        isLeaving = true;

        waitingForService = false;
        waitingForFood = false;
        if (timerText != null)
            timerText.gameObject.SetActive(true);
            timerText.text = "FUCK YOU!";
        agent.updateRotation = true;
        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);
    }

    void HandleExitArrival()
    {
        if (!isLeaving)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            RestaurantManager.Instance.CustomerLeft(assignedTable);
            Destroy(gameObject);
        }
    }
}

