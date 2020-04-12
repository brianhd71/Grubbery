<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grubbery.aspx.cs" Inherits="Grubbery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        
    </title>
    <link href="CSS/GrubberyStyles.css" rel="stylesheet" />
</head>
<body>
    <header id="header">
        <h1><asp:Label ID="lblHeader" runat="server" Text=""></asp:Label></h1>        
    </header>

    <form id="form1" runat="server">       
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
        <main id="main">
          <article id="main-window">
              <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="lstMenuItems" runat="server" CssClass="menu-items" OnSelectedIndexChanged="lstMenuItems_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>           
                    <asp:ListBox ID="lstTransaction" runat="server" CssClass="transaction-items" SelectionMode="Multiple"></asp:ListBox>      
                    <hr />

                    <asp:Label ID="Label1" runat="server" Text="SubTotal: "></asp:Label>
                    <asp:Label ID="lblSubTotal" runat="server" >0.00</asp:Label> 
                    <asp:Button ID="btnRemoveItem" runat="server" Text="Remove Item" OnClick="btnRemoveItem_Click" CssClass="button" />
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Tax: "></asp:Label>
                    <asp:Label ID="lblTax" runat="server">0.00</asp:Label>
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="Total: "></asp:Label>
                    <asp:Label ID="lblTotal" runat="server">0.00</asp:Label>
                    <asp:Label ID="lblTransactionId" runat="server" Text="" CssClass="order-number"></asp:Label>
                    <hr />

                    <asp:Label ID="lblMessage" runat="server" width="50%"></asp:Label>  
                    <asp:Button ID="btnExecute" runat="server" Text="Pay Now" OnClick="btnExecute_Click" CssClass="button" />  
                    <asp:Button ID="btnClear" runat="server" Text="Cancel Transaction" OnClick="btnCancel_Click" OnClientClick="return confirm('Are you sure you want to cancel the transation?')" CssClass="button" />
                </ContentTemplate>
              </asp:UpdatePanel>             
          </article>
          <aside id="side-nav">
            Navigation
          </aside>
        </main>

    <footer id="footer">         
    </footer>
    </form>

    </body>
</html>
