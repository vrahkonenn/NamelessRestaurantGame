using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance;

    public Customer customerPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;

    public List<Table> tables = new List<Table>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(InitialSpawn());
    }

    IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(5f);
        SpawnCustomer();

        yield return new WaitForSeconds(20f);
        SpawnCustomer();

        yield return new WaitForSeconds(15f);
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        Table freeTable = GetFreeTable();
        if (freeTable == null)
            return;

        Customer newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        newCustomer.AssignTable(freeTable, exitPoint);

        freeTable.AssignCustomer(newCustomer);
    }

    Table GetFreeTable()
    {
        foreach (var table in tables)
        {
            if (!table.IsOccupied())
                return table;
        }

        return null;
    }

    public void CustomerLeft(Table table)
    {
        table.ClearTable();
        StartCoroutine(SpawnWithDelay());
    }

    IEnumerator SpawnWithDelay()
    {
    yield return new WaitForSeconds(10f);
    SpawnCustomer();
    }
}
