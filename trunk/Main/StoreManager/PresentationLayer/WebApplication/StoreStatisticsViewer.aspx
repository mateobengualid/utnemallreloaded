<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreStatisticsViewer.aspx.cs"
    Inherits="WebApplication.StoreStatisticsViewer" MasterPageFile="~/MasterPage.master"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="statisticsViewer" runat="server">
    <div id="main" class="mainstatistics">
        <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        <div class="location" id="locationstorestatistics">
            <asp:Label ID="LabelTitle" runat="server" Text="GLOBAL STATISTICS" meta:resourcekey="LabelTitleResource1"></asp:Label></div>
        <p id="title" />
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" ImageAlign="Right"
                        Height="56px" meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                            Text="Global Statistics Messages" meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table cellpadding="5">
            <tr>
                <td>
                    <asp:Label ID="LabelDataModality" runat="server" Text="Data Modality" meta:resourcekey="LabelDataModalityResource1"
                        CssClass="floatleft" />
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListDataModality" runat="server" CssClass="floatleft"
                        Width="100px" meta:resourcekey="DropDownListDataModalityResource1">
                        <asp:ListItem meta:resourcekey="ListItemResource1" />
                        <asp:ListItem meta:resourcekey="ListItemResource2" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="ButtonObtainStatistics" runat="server" CssClass="floatleft" Text="Obtain"
                        meta:resourcekey="ButtonObtainStatisticsResource1" Width="74px" OnClick="ButtonObtainStatistics_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="GridViewStoreStatistics" runat="server" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" CssClass="floatright"
                        EmptyDataText="No hay estadisticas asociadas al servicio." meta:resourcekey="GridViewStoreStatisticsResource1">
                        <FooterStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField HeaderText="Service" ItemStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="Field" meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField HeaderText="Value" ItemStyle-Width="100px" DataField="Value" meta:resourcekey="BoundFieldResource2" />
                        </Columns>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
