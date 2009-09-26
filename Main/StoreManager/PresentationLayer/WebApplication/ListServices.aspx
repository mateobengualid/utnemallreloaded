<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListServices.aspx.cs" Inherits="WebApplication.ListServices"
    MasterPageFile="~/MasterPage.master" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="listServices" runat="server">
    <script language="javascript" type="text/javascript">
        function confirm() 
        { 
            if (confirm == true)
            else
            {
                return false;
            }
        }
    </script>    
    <asp:ScriptManager ID="ScriptManagerMaster" runat="server">
    </asp:ScriptManager>
    <div id="main" class="mainservices">
        <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        <div class="location" id="locationservices">
            <asp:Label ID="LabelTitle" runat="server" Text="SERVICES" meta:resourcekey="LabelTitleResource1"></asp:Label>
        </div>
        <p id="title">
            &nbsp;<asp:UpdatePanel ID="UpdatePanelMessage" runat="server">            
            <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 142px">
                        <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" ImageAlign="Right"
                            meta:resourcekey="InformationImageResource1" />
                    </td>
                    <td>
                        <center>
                            <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                                Text="Service has been updates successfuly" meta:resourcekey="LabelMessageResource1"></asp:Label>
                        </center>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            </asp:UpdatePanel>
        </p>
        <table cellpadding="5">
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanelGridViewServices" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="ServicesGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                CellPadding="3" EmptyDataText="Couldn't find any services!" ForeColor="Black"
                                GridLines="Vertical" Width="100%" meta:resourcekey="ServicesGridResource1">
                                <FooterStyle BackColor="#CCCCCC" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/img/arrows/select.jpg"
                                        meta:resourcekey="CommandFieldResource1">
                                        <ItemStyle Width="10px" />
                                    </asp:CommandField>
                                    <asp:BoundField HeaderText="Service Name" DataField="ServiceName" meta:resourcekey="BoundFieldResource1">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Start Date" DataField="ServiceStartDate" meta:resourcekey="BoundFieldResource4" />
                                    <asp:BoundField HeaderText="Stop Date" DataField="ServiceStopDate" meta:resourcekey="BoundFieldResource5" />
                                    <asp:BoundField HeaderText="Is Deployed" DataField="ServiceDeployed" meta:resourceKey="BoundFieldResource2">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Service Description" DataField="ServiceDescription" meta:resourcekey="BoundFieldResource3" />
                                </Columns>
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#80A3B6" Font-Bold="True" ForeColor="Black" />
                                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#CCCCCC" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:UpdateProgress ID="UpdateProgressBuildService" AssociatedUpdatePanelID="UpdatePanelBuildService" runat="server">
                        <ProgressTemplate>
                            <img src="img/loader.gif" alt="Building"/>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanelBuildService" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ButtonBuildService" runat="server" OnClick="ButtonBuildService_Click" 
                                Text="Build"  meta:resourcekey="ButtonBuildServiceResource1" />                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                    <td align="right" >
                    <asp:Button ID="ButtonDesign" runat="server" CausesValidation="False" Text="Design Service"
                        meta:resourcekey="ButtonDesignServiceResource1" OnClick="ButtonDesign_Click"
                        />
                    <asp:Button ID="ButtonNewService" runat="server" OnClick="ButtonNewService_Click"
                        Text="New Service" meta:resourcekey="ButtonNewServiceResource1"  />
                    <asp:Button ID="ButtonEditService" runat="server" CausesValidation="False" OnClick="ButtonEditService_Click"
                        Text="Edit Service" meta:resourcekey="ButtonEditServiceResource1" />
                    <asp:Button ID="ButtonDeleteService" runat="server" Text="Delete Service" 
                            onclick="ButtonDeleteService_Click" meta:resourcekey="ButtonDeleteServiceResource1" />    
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
