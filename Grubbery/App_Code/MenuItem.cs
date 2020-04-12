using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MenuItem
/// </summary>
public class MenuItem
{
    private int _id;
    private string _desc;
    private string _cat;
    private double _price;
    public MenuItem(int id, string desc, string cat, double price)
    {
        _id = id;
        _desc = desc;
        _cat = cat;
        _price = price;
    }

    public int getId()
    {
        return _id;
    }
    public string getCategory()
    {
        return _cat;
    }

    public int getDescriptionLength(string desc)
    {
        return desc.Length;
    }


    public string getDescription()
    {
        return _desc;
    }

    public double getPrice()
    {
        return _price;
    }
}