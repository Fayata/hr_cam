<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CheckSession.aspx.cs" Inherits="hr_cam.CheckSession" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bg-light">
    <p>Session Email:
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></p>
    <p>Session Name:
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></p>
    <p>Session Role:
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label></p>
        </div>
</asp:Content>
