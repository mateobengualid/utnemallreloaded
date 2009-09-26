<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="WebApplication.ErrorPage" MasterPageFile="~/MasterPage.master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="ErrorForm" runat="server">
    <div id="main">
        <div id="welcome">
            <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" 
                meta:resourcekey="LabelWelcomeResource1" ></asp:Label>
        </div>
        <div class="location" id="locationerror">
            <asp:Label ID="LabelTitle" runat="server" Text="ERROR" 
                meta:resourcekey="LabelTitleResource1" ></asp:Label></div>
        <p id="title"/>
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/error.png" 
                        ImageAlign="Right" meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Text="An error has occured in the application."
                            Style="vertical-align: middle; display: table-cell;" 
                            meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table id="loginform" cellpadding="5" width="100%">
            <tr>
                <td align="center">
                    <asp:TextBox ID="textBoxErrorDetails" runat="server" Width="400px" 
                        ReadOnly="True" TextMode="MultiLine" Visible="False" Height="200px"></asp:TextBox>               
                </td>
            </tr>
            <tr>    
                <td>
                    <asp:Button ID="ButtonShowErrorDetails" runat="server" Text="Show Details" 
                        onclick="ButtonShowErrorDetails_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
