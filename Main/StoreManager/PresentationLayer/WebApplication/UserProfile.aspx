<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="WebApplication.UserProfile"
    MasterPageFile="~/MasterPage.master" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="userProfile" runat="server">
    <div id="main" class="mainuserprofile">
        <div id="welcome">
            <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        </div>
        <div class="location" id="locationuserprofile">
            <asp:Label ID="LabelTitle" runat="server" Text="USER PROFILE" 
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
                                Text="User Profile has been updated successfuly" meta:resourcekey="LabelMessageResource1"></asp:Label>
                        </center>
                    </td>
                </tr>
            </table>
        </p>
        <table id="loginform" cellpadding="5" width="auto">
            <tr>
                <td style="width:200px">
                    <asp:Label ID="LabelUserName" runat="server" Text="User Name" meta:resourcekey="LabelUserNameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxUserName" runat="server" meta:resourcekey="TextBoxUserNameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameValidator" runat="server" ControlToValidate="TextBoxUserName"
                        ErrorMessage="*" meta:resourcekey="UserNameValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelName" runat="server" Text="Name" meta:resourcekey="LabelNameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxName" runat="server" meta:resourcekey="TextBoxNameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NameValidator" runat="server" ControlToValidate="TextBoxName"
                        ErrorMessage="*" meta:resourcekey="NameValidatorResource2"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelSurname" runat="server" Text="Surname" meta:resourcekey="LabelSurnameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxSurname" runat="server" meta:resourcekey="TextBoxSurnameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="SurnameValidator" runat="server" ControlToValidate="TextBoxSurname"
                        ErrorMessage="*" meta:resourcekey="SurnameValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelPhoneNumber" runat="server" Text="Phone Number" meta:resourcekey="LabelPhoneNumberResource1"
                        CssClass="floatright" ></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPhoneNumber" runat="server" meta:resourcekey="TextBoxPhoneNumberResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PhoneValidator" runat="server" ControlToValidate="TextBoxPhoneNumber"
                        ErrorMessage="*" meta:resourcekey="PhoneValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelJobPosition" runat="server" Text="Job Position" meta:resourcekey="LabelJobPositionResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxCharge" runat="server" meta:resourcekey="TextBoxChargeResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="JobValidator" runat="server" ControlToValidate="TextBoxCharge"
                        ErrorMessage="*" meta:resourcekey="JobValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2"/>
            </tr>
            <tr>
                <td colspan="2">
                    <div align="right" style="margin-right: 10px">
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
