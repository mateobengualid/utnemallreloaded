<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Service.aspx.cs" Inherits="WebApplication.Service"
    MasterPageFile="~/MasterPage.master" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="service" runat="server">
    <div id="main" class="mainservice">
        <div id="welcome">
            <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        </div>
        <div class="location" id="locationservice">
            <asp:Label ID="LabelTitle" runat="server" Text="SERVICE" 
                meta:resourcekey="LabelTitleResource1"></asp:Label>
        </div>
        <p id="title">
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
        </p>
        <table id="loginform" style="border-collapse:collapse; width:auto;" cellpadding="5">
            <tr>
                <td>
                    <asp:Label ID="LabelServiceName" runat="server" Text="Service Name" meta:resourcekey="LabelServiceNameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxServiceName" runat="server" meta:resourcekey="TextBoxServiceNameResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="LabelDescription" runat="server" Text="Description" meta:resourcekey="LabelDescriptionResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxServiceDescription" runat="server" meta:resourcekey="TextBoxServiceDescriptionResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    <asp:Label ID="LabelStartDate" runat="server" Text="Start Date" meta:resourcekey="LabelStartDateResource1"></asp:Label>
                    <asp:Label ID="LabelFormatDateStart" runat="server" Text="(mm/dd/yyyy)" 
                        meta:resourcekey="LabelFormatDateStartResource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxServiceStartDate" runat="server" MaxLength="10" 
                        meta:resourcekey="TextBoxServiceStartYearResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    <asp:Label ID="LabelStopDate" runat="server" Text="Stop Date" meta:resourcekey="LabelStopDateResource1"></asp:Label>
                    <asp:Label ID="LabelFormatDateStop" runat="server" Text="(mm/dd/yyyy)" 
                        meta:resourcekey="LabelFormatDateStopResource1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxServiceStopDate" runat="server"
                        MaxLength="10" meta:resourcekey="TextBoxServiceStopMonthResource1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp</td>
            </tr>
            <tr>
                <td style="vertical-align: top; border-left: 1px solid #fff; border-top: 1px solid #fff; border-bottom: 1px solid #fff;">
                    <asp:Label ID="LabelCategories" runat="server" Text="Categories" meta:resourcekey="LabelCategoriesResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td style="border-right: 1px solid #fff; border-top: 1px solid #fff; border-bottom: 1px solid #fff;">
                    <div style="overflow: scroll; height: 200px;">
                        <asp:TreeView ID="TreeViewCategories" runat="server" ForeColor="White" meta:resourcekey="TreeViewCategoriesResource1"
                            ShowCheckBoxes="All">
                        </asp:TreeView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div align="right" >
                        <asp:Button ID="ButtonAccept" runat="server" Text="Accept" OnClick="ButtonAccept_Click"
                            meta:resourcekey="ButtonAcceptResource1" />
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click"
                            meta:resourcekey="ButtonCancelResource1" />
                    </div>
                </td>
            </tr>
              
    </table> 
    </div>
    </form>
</asp:Content>
