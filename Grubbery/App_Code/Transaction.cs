using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Object for the current transaction
/// </summary>
public class Transaction
{
    private int _TransactionIdCount;
    private double _SubTotal;
    private double _Tax;
    private double _Total;
    private double _SalesTaxRate;

    public Transaction()
    {
        clearTotals();
    }

    public string addToSubTotal(double itemPrice)
    {
        _SubTotal += itemPrice;
        return _SubTotal.ToString("C");
    }

    public string addTotal()
    {
        _Total = _SubTotal + _Tax;
        return _Total.ToString("C");
    }

    public string calculateTax()
    {
        _Tax = _SubTotal * _SalesTaxRate;
        return _Tax.ToString("C");
    }

    public void clearTotals()
    {
        _SubTotal = 0.00;
        _Tax = 0.00;
        _Total = 0.00;
    }

    public int getTransactonId()
    {
        return _TransactionIdCount;
    }

    public double getSubTotal()
    {
        return _SubTotal;
    }

    public double getTax()
    {
        return _Tax;
    }

    public void incrementTransactionId()
    {
        _TransactionIdCount++;
    }

    public void setTaxRate(double taxRate)
    {
        _SalesTaxRate = taxRate;
    }

    public void setTransactionId(int transactionId)
    {
        _TransactionIdCount = ++transactionId;
    }

    public string subtractFromSubTotal(double itemPrice)
    {
        _SubTotal -= itemPrice;
        return _SubTotal.ToString("C");
    }
}