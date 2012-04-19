<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="ASPNetWebApp.WebForm" %>
<%@ Register Src="~/Controls/TwitterFeedControl.ascx" TagName="TwitterFeedControl" TagPrefix="tf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Web Form</title>
    <link href="/Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="page">
        <header>
            <div id="title">
                <h1>Twitter Feed Control Demo Application - WebForms</h1>
            </div>
        </header>
        <section id="main">

        <p>
            Here's the Twitter control<br /><br />
            <tf:TwitterFeedControl ID="TwitterFeedControl" runat="server" />
        </p>

        <p>
            <a href="/">Try the MVC version</a>
        </p>


        </section>
        <footer>
        </footer>
    </div>

    </form>
</body>
</html>
