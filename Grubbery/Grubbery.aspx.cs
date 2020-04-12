using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Grubbery : System.Web.UI.Page
{
    const string ORDER_ID_LABEL = "Order Number: ";
    static List<Object> menuItems = new List<Object>();
    static List<Object> transactionDetail = new List<Object>();
    static Transaction t = new Transaction();
    static int sequence;
    private string connString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=grubbery;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    private void clearTransaction()
    {
        transactionDetail.Clear();
        t.clearTotals();
        lstTransaction.Items.Clear();
        lblSubTotal.Text = "";
        lblTax.Text = "";
        lblTotal.Text = "";
    }

    private string createDottedLine(int length)
    {
        string dottedLine = "";
        for (int i = length; i < 24; i++)
        {
            if (i % 2 == 0)
                dottedLine = dottedLine + ".";
            else
                dottedLine = dottedLine + " ";
        }
        return dottedLine;
    }

    private void getData()
    {
        string query = "SELECT TOP 1 NAME, TAXRATE FROM STOREINFO ORDER BY ID DESC;"             +
                       "SELECT TRANSACTIONID FROM TRANSACTIONMASTER ORDER BY TRANSACTIONID DESC;" +
                       "SELECT * FROM DBO.vMENUITEMS ORDER BY DESCRIPTION;";
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand comm = new SqlCommand(query, conn);

        using (conn)
        {
            conn.Open();
            SqlDataReader r = comm.ExecuteReader();

            try
            {
                r.Read();
                lblHeader.Text = r[0].ToString();
                t.setTaxRate(Convert.ToDouble(r[1]));
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            r.NextResult();

            try
            {
                r.Read();
                t.setTransactionId((int)r[0]);
            }
            catch (Exception)
            {
                t.setTransactionId(0);
            }
            r.NextResult();

            try
            {                
                while (r.Read())
                {
                    menuItems.Add(new MenuItem((int)r[0], r[1].ToString(), r[2].ToString(), (double)r[3]));
                }
            }
            catch (Exception ex)
            {
                lstMenuItems.Text = ex.Message;
            }           

            r.Close();
            comm.Dispose();
            conn.Close();
        }
        lblTransactionId.Text = $"{ ORDER_ID_LABEL}{ t.getTransactonId().ToString()}";
    }

    private void writeToTransactionDetail()
    {
        SqlConnection conn = new SqlConnection(connString);
        string query = "SELECT * FROM DBO.TRANSACTIONDETAIL;";
        SqlCommand comm;

        using (conn)
        {
            try
            {
                conn.Open();
                foreach (TransactionDetail td in transactionDetail)
                {
                    comm = new SqlCommand(query, conn);
                    comm.CommandText = "INSERT INTO TRANSACTIONDETAIL (TRANSACTIONDETAILID, SEQUENCE, MENUITEMID, ITEMDESCRIPTION, ITEMPRICE) " +
                                       "VALUES (@transactionID, @sequence, @menuItemId, @itemDescription, @itemPrice)";
                    comm.Parameters.AddWithValue("@transactionId", td.getTransactionId());
                    comm.Parameters.AddWithValue("@sequence", td.getSequence());
                    comm.Parameters.AddWithValue("@menuItemId", td.getMenuItemId());
                    comm.Parameters.AddWithValue("@itemDescription", td.getItemDesc());
                    comm.Parameters.AddWithValue("@itemPrice", td.getPrice());
                    comm.ExecuteNonQuery();
                    comm.Dispose();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.ToString();
            }

            conn.Close();
        }
    }
    private void writeToTransactionMaster()
    {
        SqlConnection conn = new SqlConnection(connString);
        string query = "SELECT * FROM DBO.TRANSACTIONMASTER;";
        SqlCommand comm = new SqlCommand(query, conn);

        using (conn)
        {
            try
            {
                conn.Open();                
                comm.CommandText = "INSERT INTO TRANSACTIONMASTER (TRANSACTIONID, SUBTOTAL, TAX, AMOUNTPAID, ASSOCIATEID) " +
                    "               VALUES (@transactionID, @subTotal,  @tax, @amountPaid, @id)";
                comm.Parameters.AddWithValue("@transactionId", t.getTransactonId());
                comm.Parameters.AddWithValue("@subTotal", t.getSubTotal());
                comm.Parameters.AddWithValue("@tax", t.getTax());
                comm.Parameters.AddWithValue("@amountPaid", 7.00);
                comm.Parameters.AddWithValue("@id", 123);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.ToString();
            }
           
            comm.Dispose();
            conn.Close();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (lstMenuItems.Items.Count == 0)
        {
            menuItems.Clear();
            getData();            

            foreach (MenuItem mi in menuItems)
            {
                lstMenuItems.Items.Add(mi.getDescription());
            }       
        }
        Page.Title = lblHeader.Text; 
    }

    protected void lstMenuItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        foreach (MenuItem mi in menuItems)
        {
            if (lstMenuItems.SelectedItem == lstMenuItems.Items.FindByText(mi.getDescription()))
            {
                transactionDetail.Add(new TransactionDetail(t.getTransactonId(), ++sequence, mi.getId(), mi.getDescription(), mi.getPrice() ) );
                int descLength = mi.getDescriptionLength(mi.getDescription());
                lstTransaction.Items.Add(lstMenuItems.SelectedItem + createDottedLine(descLength) + mi.getPrice().ToString("C"));
                lblSubTotal.Text = t.addToSubTotal(mi.getPrice());
                lblTax.Text = t.calculateTax();
                lblTotal.Text = t.addTotal();
                break;
            }
        }
        lstMenuItems.SelectedIndex = -1;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearTransaction();
    }

    protected void btnRemoveItem_Click(object sender, EventArgs e)
    {
        string item = lstTransaction.SelectedValue;
        double subtractedPrice = 0.00;

        if (item != "")
        {
            transactionDetail.RemoveAt(lstTransaction.SelectedIndex);
            sequence--;
            double.TryParse(item.Substring(25), out subtractedPrice);
            lblSubTotal.Text = t.subtractFromSubTotal(subtractedPrice);
            lstTransaction.Items.Remove(lstTransaction.SelectedItem);
            lblTax.Text = t.calculateTax();
            lblTotal.Text = t.addTotal();
        }
    }

    protected void btnExecute_Click(object sender, EventArgs e)
    {
        if (lstTransaction.Items.Count > 0)
        {
            writeToTransactionDetail();
            writeToTransactionMaster();
            clearTransaction();
            if (lblMessage.Text == "")
            {
                lblMessage.Text = $"Order Number {t.getTransactonId()} Processed Successfully...";
                t.incrementTransactionId();
                lblTransactionId.Text = $"{ ORDER_ID_LABEL}{ t.getTransactonId().ToString()}";
            }
        }
    }
}