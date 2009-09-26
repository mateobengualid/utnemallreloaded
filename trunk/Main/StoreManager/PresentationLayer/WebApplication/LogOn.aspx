<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogOn.aspx.cs" 
Inherits="WebApplication.LogOn" MasterPageFile="~/MasterPage.master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">    
    <form id="logOn" runat="server">    
    <div id="main" class="mainlogin">    
        <div class="location" id="locationlogin">        
            <asp:Label ID="LabelUtnEmallLogOn" runat="server" Text="UtnEmall LOGIN" 
                meta:resourcekey="LabelUtnEmallLoginResource1"></asp:Label></div>
            <p id="title"><asp:Label ID="LabelWelcome" runat="server" 
                    Text="Welcome to <strong>STORE MANAGER SYSTEM</strong>" 
                    meta:resourcekey="LabelWelcomeResource1"></asp:Label></p>
            <table width="100%"><tr><td style="width: 142px">
                <asp:Image ID="InformationImage" 
                    runat="server" ImageUrl="~/img/msgs/error.png" 
                    meta:resourcekey="ErrorImageResource1" ImageAlign="Right" /></td><td><center>
                    <asp:Label ID="LabelMessage" runat="server" style="vertical-align: middle;display: table-cell;" Text="The user and password is incorrect, please try again" 
                    meta:resourcekey="LabelMessageResource1"></asp:Label></center></td></tr></table>                
            <table id="loginform" cellpadding="5" style="width:auto">                
	            <tr><td>
                    <asp:Label ID="LabelUserName" runat="server" Text="Username" 
                        meta:resourcekey="LabelUsernameResource1" CssClass="floatleft"></asp:Label></td>
                    <td><asp:TextBox ID="TextBoxUserName" runat="server" 
                            meta:resourcekey="TextBoxUserNameResource1" Width="128px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserValidator" runat="server" 
                            ControlToValidate="TextBoxUserName" ErrorMessage="*" 
                            meta:resourcekey="UserValidatorResource1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
	            <tr><td><asp:Label ID="LabelPassword" runat="server" Text="Password" 
                        meta:resourcekey="LabelPasswordResource1" CssClass="floatright"></asp:Label></td>
	                <td><asp:TextBox ID="TextBoxPassword" runat="server" Width="128px"
                            meta:resourcekey="TextBoxPasswordResource1" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" 
                            ControlToValidate="TextBoxPassword" ErrorMessage="*" 
                            meta:resourcekey="PasswordValidatorResource1"></asp:RequiredFieldValidator>
                    </td>
	            </tr>
	            <tr>
	                <td colspan="2"></td>
	            </tr>
	            <tr>
	                <td colspan="2" >
	                    <div align="right" style="margin-right: 10px">	            	                	                
                            <asp:Button ID="ButtonLogOn" runat="server" Text="Login" OnClick="ButtonLogOn_Click" meta:resourcekey="ButtonLogOnResource1" />
                        </div>
                    </td>
                </tr>
            </table>
    </div>
    </form>
</asp:Content>
