<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsViewer.aspx.cs"
    Inherits="WebApplication.StatisticsViewer" MasterPageFile="~/MasterPage.master"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="statisticsViewer" runat="server">
    <div id="main" class="mainstatistics">
        <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        <div class="location" id="locationstatistics">
            <asp:Label ID="LabelTitle" runat="server" Text="STATISTICS" 
                meta:resourcekey="LabelTitleResource1"></asp:Label></div>
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" 
                        ImageAlign="Right" meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                            Text="Statistics Messages" meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table id="loginform">
            <tr>
                <td colspan="3" style="border: solid 2px #ddd;">
                    <center>
                        <asp:Label ID="LabelServiceStatistics" runat="server" Style="vertical-align: middle;
                            display: table-cell;" 
                            Text="Select <b>Service statistics</b> if you want to see statistics of a specific service and its components." 
                            meta:resourcekey="LabelServiceStatisticsResource1"></asp:Label>
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="ButtonServiceStatistics" runat="server" Text="Service Statistics"
                        CssClass="floatright" OnClick="ButtonServiceStatistics_Click" 
                        meta:resourcekey="ButtonServiceStatisticsResource2" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 40px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border: solid 2px #ddd;">
                    <center>
                        <asp:Label ID="LabelGlobalStatistics" runat="server" Style="vertical-align: middle;
                            display: table-cell;" 
                            Text="Select <b>Global statistics</b> if you want to see summarized information of the usage of all the services." 
                            meta:resourcekey="LabelGlobalStatisticsResource1"></asp:Label>
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="ButtonGlobalStatistics" runat="server" Text="Global Statistics" CssClass="floatright"
                        OnClick="ButtonGlobalStatistics_Click" Width="155px" 
                        meta:resourcekey="ButtonGlobalStatisticsResource1" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
