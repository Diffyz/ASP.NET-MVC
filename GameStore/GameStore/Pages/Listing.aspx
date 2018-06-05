<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Listing.aspx.cs" Inherits="GameStore.Pages.Listing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>GameStore</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h5>start</h5>
            <%
               foreach (GameStore.Models.Game game in GetGames())
                {
                    Response.Write(String.Format(@"
                        <div class='item'>
                            <h3>{0}</h3>
                            {1}
                            <h4>{2:c}</h4>
                        </div>", 
                        game.Name, game.Description, game.Price));
                }
                %>
        </div>
        <h5>finish</h5>
    </form>
</body>
</html>
