<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.ascx.cs" 
Inherits="WebApplication.controls.MessageBox" %>

<asp:Panel ID="MsgBoxPanel" runat="server" Width="100%">
    <table border="0" width="100%" style="border: medium solid #336699 ;">
    <tr>
        <th style="text-align:right; vertical-align: middle; width:40%">
        <asp:Image ID="MsgBoxImage" runat="server" Width="25px" Height="25px" ImageUrl="~/img/msgs/info.png" /></th>
        <th style="text-align:left; font-weight:bold; font-size:14px; vertical-align: middle;">
        <asp:Label ID="MsgBoxTitle" runat="server" Text="Information"></asp:Label></th>
    </tr>
    <tr>
        <td colspan="2" 
            style="text-align:center; font-size:12px; font-weight: bold; vertical-align: top;">
            <asp:Label ID="MsgBoxMessage" runat="server" Text="This is an information message&lt;br&gt;With two lines"></asp:Label></td>
    </tr>
    </table>
</asp:Panel>