<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" 
Inherits="WebApplication.Home" MasterPageFile="~/MasterPage.master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div id="main" class="mainhome">
    <div id="welcome">
        <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" 
            CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
    </div>
        <div class="location" id="locationhome">
            <asp:Label ID="LabelTitle" runat="server" Text="HOME" 
                meta:resourcekey="LabelTitleResource1"></asp:Label>
        </div>
        <p id="title">
            <table width="100%">
                <tr>
                    <td style="width: 142px">
                        <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" 
                            ImageAlign="Right" meta:resourcekey="InformationImageResource1" />
                    </td>
                    <td>
                        <center>
                            <asp:Label ID="LabelMessage" runat="server" 
                                Style="vertical-align: middle; display: table-cell;" 
                                meta:resourcekey="LabelMessageResource1"></asp:Label>
                        </center>
                    </td>
                </tr>
            </table>
        </p>
   </div>
</asp:Content>
