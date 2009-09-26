<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="WebApplication.ChangePassword" MasterPageFile="~/MasterPage.master"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="changePassword" runat="server">
    <div id="main" class="mainpass">
        <div id="welcome">
            <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        </div>
        <div class="location" id="locationpass">
            <asp:Label ID="LabelTitle" runat="server" Text="CHANGE PASSWORD" meta:resourcekey="LabelTitleResource1"></asp:Label></div>
        <p id="title"/>
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" ImageAlign="Right"
                        meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                            Text="The user password has been changed" meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table id="loginform" cellpadding="5" width="auto">
            <tr>
                <td style="width:200px">
                    <asp:Label ID="LabelPassword" runat="server" Text="Password" meta:resourcekey="LabelPasswordResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPassword" runat="server" meta:resourcekey="TextBoxPasswordResource1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="TextBoxPassword"
                        ErrorMessage="*" meta:resourcekey="PasswordValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelNewPassword" runat="server" Text="New Password" meta:resourcekey="LabelNewPasswordResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewPassword" runat="server" meta:resourcekey="TextBoxNewPasswordResource1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPasswordValidator" runat="server" ErrorMessage="*"
                        ControlToValidate="TextBoxNewPassword" meta:resourcekey="NewPasswordValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelConfirmation" runat="server" Text="Confirmation" meta:resourcekey="LabelConfirmationResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewPasswordConfirm" runat="server" meta:resourcekey="TextBoxNewPasswordConfirmResource1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmValidator" runat="server" ErrorMessage="*"
                        ControlToValidate="TextBoxNewPasswordConfirm" meta:resourcekey="ConfirmValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div align="right" style="margin-right: 10px">
                        <asp:Button ID="ButtonAccept" runat="server" Text="Accept" OnClick="ButtonAccept_Click"
                            meta:resourcekey="ButtonAcceptResource1" />
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click"
                            meta:resourcekey="ButtonCancelResource1" CausesValidation="False" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
