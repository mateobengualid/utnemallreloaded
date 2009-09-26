<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreProfile.aspx.cs" Inherits="WebApplication.StoreProfile"
    MasterPageFile="~/MasterPage.master" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <form id="storeProfile" runat="server">
    <asp:ScriptManager ID="ScriptManagerMaster" runat="server">
    </asp:ScriptManager>
    <div id="main" class="mainstoreprofile">
        <div id="welcome">
            <asp:Label ID="LabelWelcome" runat="server" Text="Welcome" CssClass="welcome" meta:resourcekey="LabelWelcomeResource1"></asp:Label>
        </div>
        <div class="location" id="locationstoreprofile">
            <asp:Label ID="LabelTitle" runat="server" Text="STORE PROFILE" 
                meta:resourcekey="LabelTitleResource1"></asp:Label>
        </div>
        <p id="title">
        </p>
        <table width="100%">
            <tr>
                <td style="width: 142px">
                    <asp:Image ID="InformationImage" runat="server" ImageUrl="~/img/msgs/info.png" ImageAlign="Right"
                         meta:resourcekey="InformationImageResource1" />
                </td>
                <td>
                    <center>
                        <asp:Label ID="LabelMessage" runat="server" Style="vertical-align: middle; display: table-cell;"
                            Text="Store Profile has been updates successfuly" meta:resourcekey="LabelMessageResource1"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
        <table id="loginform" style="border-collapse: collapse;" cellpadding="5">
            <tr>
                <td style="width: 200px">
                    <asp:Label ID="LabelStoreName" runat="server" Text="Store Name" meta:resourcekey="LabelStoreNameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxName" runat="server" meta:resourcekey="TextBoxNameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="StoreNameValidator" runat="server" ControlToValidate="TextBoxName"
                        ErrorMessage="*" meta:resourcekey="StoreNameValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelContactName" runat="server" Text="Contact Name" meta:resourcekey="LabelContactNameResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxContactName" runat="server" meta:resourcekey="TextBoxContactNameResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ContactValidator" runat="server" ControlToValidate="TextBoxContactName"
                        ErrorMessage="*" meta:resourcekey="ContactValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelPhoneNumber" runat="server" Text="Phone Number" meta:resourcekey="LabelPhoneNumberResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPhone" runat="server" meta:resourcekey="TextBoxPhoneResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PhoneValidator" runat="server" ControlToValidate="TextBoxPhone"
                        ErrorMessage="*" meta:resourcekey="PhoneValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelWebsite" runat="server" Text="Web Site" meta:resourcekey="LabelWebSiteResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxWebsite" runat="server" meta:resourcekey="TextBoxWebsiteResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="WebsiteValidator" runat="server" ControlToValidate="TextBoxWebsite"
                        ErrorMessage="*" meta:resourcekey="WebsiteValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelEmail" runat="server" Text="Email" meta:resourcekey="LabelEmailResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxEmail" runat="server" meta:resourcekey="TextBoxEmailResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailValidator" runat="server" ControlToValidate="TextBoxEmail"
                        Display="Dynamic" ErrorMessage="*" meta:resourcekey="EmailValidatorResource1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelDataModel" runat="server" Text="Data Model" 
                        CssClass="floatright" meta:resourcekey="LabelDataModelResource1"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelBuildDataModel" runat="server">
                        <ContentTemplate>
                            <asp:CheckBox ID="CheckBoxDeployed" runat="server" Enabled="False" Text=" " 
                                meta:resourcekey="CheckBoxDeployedResource1"/>
                            <asp:Label ID="LabelDeployed" runat="server" Text="Is Deployed" 
                                meta:resourcekey="LabelDeployedResource1"></asp:Label>
                            <asp:Button ID="ButtonBuildDataModel" runat="server" Text="Build" 
                                meta:resourcekey="ButtonBuildServiceResource1" onclick="ButtonBuildDataModel_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:UpdateProgress ID="UpdateProgressDataModel" AssociatedUpdatePanelID="UpdatePanelBuildDataModel"
                        runat="server">
                        <ProgressTemplate>
                            <img src="img/loader.gif" alt="Building" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; border-left: 1px solid #fff; border-top: 1px solid #fff;
                    border-bottom: 1px solid #fff;">
                    <asp:Label ID="LabelCategories" runat="server" Text="Categories" meta:resourcekey="LabelCategoriesResource1"
                        CssClass="floatright"></asp:Label>
                </td>
                <td style="border-right: 1px solid #fff; border-top: 1px solid #fff; border-bottom: 1px solid #fff;">
                    <div style="overflow: scroll; height: 200px;">
                    <asp:TreeView ID="treeViewCategories" runat="server" ForeColor="White" meta:resourcekey="TreeViewCategoriesResource1"
                        Width="150px" ShowCheckBoxes="All">
                    </asp:TreeView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <div align="right" style="margin-right: 10px">
                        <asp:Button ID="ButtonDesign" runat="server" Text="Design Data Model" 
                            OnClick="ButtonDesignDataModel_Click" meta:resourcekey="ButtonDesignResource1" />
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
