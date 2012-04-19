<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TwitterFeedControl.ascx.cs" Inherits="ASPNetWebApp.Controls.TwitterFeedControl" %>
<div class="BlackHeading" style="text-align:left"><%=TwitterUserID %> on <a style="text-decoration:none" href="http://twitter.com/<%=TwitterUserID %>" target="_blank">Twitter</a></div>
<div class="twtr-widget twtr-widget-profile">
<div class="twtr-doc">
<div class="twtr-timeline" style="height:120px; overflow: auto; overflow-y:scroll; overflow-x:hidden;">
<div class="twtr-tweets">
<asp:Literal ID="litFeed" runat="server"></asp:Literal>
</div>
</div>
</div>
</div>

