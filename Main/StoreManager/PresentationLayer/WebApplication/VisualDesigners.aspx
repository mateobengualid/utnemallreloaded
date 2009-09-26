<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualDesigners.aspx.cs" Inherits="WebApplication.VisualDesigners" culture="auto" uiculture="auto" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Silverlight Visual Designer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
</head>
<body style="margin:0px 0px 0px 0px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div  style="height:100%;">
            <asp:Silverlight ID="Silverlight" runat="server" 
                Source="~/ClientBin/SilverlightVisualDesigners.xap" 
                MinimumVersion="2.0.30923.0" 
                Width="100%" 
                Height="100%" 
                />
        </div>
    </form>
</body>
</html>
