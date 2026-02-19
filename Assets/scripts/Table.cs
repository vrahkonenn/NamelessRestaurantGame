using UnityEngine;

public class Table : MonoBehaviour
{
    public int tableNumber;
    public Transform customerSpot;

    private Customer currentCustomer;

    public bool IsOccupied()
    {
        return currentCustomer != null;
    }

    public void AssignCustomer(Customer customer)
    {
        currentCustomer = customer;
    }

    public void ClearTable()
    {
        currentCustomer = null;
    }

    public Transform GetSpot()
    {
        return customerSpot;
    }
}
