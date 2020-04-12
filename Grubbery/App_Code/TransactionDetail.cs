using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TransactionDetail
/// </summary>
public class TransactionDetail
{
    private int _transactionDetailId;
    private int _sequence;
    private int _menuItemId;
    private string _itemDesc;
    private double _price;
    public TransactionDetail(int transactionId, int seq, int menuItemId, string itemDesc, double price)
    {
        _transactionDetailId = transactionId;
        _sequence = seq;
        _menuItemId = menuItemId;
        _itemDesc = itemDesc;
        _price = price;
    }

    public int getTransactionId()
    {
        return _transactionDetailId;
    }

    public int getSequence()
    {
        return _sequence;
    }

    public int getMenuItemId()
    {
        return _menuItemId;
    }

    public string getItemDesc()
    {
        return _itemDesc;
    }

    public double getPrice()
    {
        return _price;
    }
}