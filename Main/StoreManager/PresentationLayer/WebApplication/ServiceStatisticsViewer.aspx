<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceStatisticsViewer.aspx.cs"
    Inherits="WebApplication.ServiceStatisticsViewer" MasterPageFile="~/MasterPage.master"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="statisticsViewer" runat="server">
    <div id="main" class="mainstatistics">
        <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        <div class="location" id="locationstorestatistics">
            <asp:Label ID="LabelTitle" runat="server" Text="GLOBAL STATISTICS" meta:resourcekey="LabelTitleResource1"></asp:Label></div>
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" ImageAlign="Right"
                        meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                            Text="Service Statistics Messages" meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table cellpadding="5">
            <tr>
                <td style="width: 40px">
                    <asp:Label ID="LabelService" runat="server" Text="Service" CssClass="floatright"
                        meta:resourcekey="LabelServiceResource1"></asp:Label>
                </td>
                <td style="width: 240px">
                    <asp:DropDownList ID="ServiceDropDownList" runat="server" Width="240px" meta:resourcekey="ServiceDropDownListResource1">
                    </asp:DropDownList>
                </td>
                <td style="width: 70px">
                    <asp:Button ID="ButtonView" runat="server" Text="View" Width="70px" CssClass="floatright"
                        OnClick="ButtonView_Click" meta:resourcekey="ButtonViewResource1" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div id="divScroll" style="height: 350px; overflow: auto;"
                        onscroll="$get('hdnScrollTop').value = this.scrollTop;">
                        <asp:GridView ID="GridViewServiceStatistics" runat="server" AutoGenerateColumns="False"
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                            CellPadding="3" ForeColor="Black" GridLines="Vertical" CssClass="floatleft" meta:resourcekey="GridViewServiceStatisticsResource1">
                            <FooterStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:TemplateField HeaderText="Image" meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Image ID="formTemplateImage" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, GetLocalResourceObject("ImageDataFieldName").ToString()).ToString() %>'
                                            meta:resourcekey="formTemplateImageResource1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Title" meta:resourcekey="BoundFieldResource1" />
                                <asp:TemplateField HeaderText="Summary" meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, GetLocalResourceObject("SummaryDataFieldName").ToString()).ToString().Replace(Environment.NewLine,"<br/>") %>'
                                            meta:resourcekey="lblValueResource1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
