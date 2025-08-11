<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="cobe_people.aspx.cs" Inherits="hr_cam.cobe_people" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
           <h2>Person List</h2>
    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
        <asp:ListItem Text="10" Value="10" />
        <asp:ListItem Text="25" Value="25" />
        <asp:ListItem Text="50" Value="50" />
        <asp:ListItem Text="100" Value="100" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlImageFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlImageFilter_SelectedIndexChanged">
        <asp:ListItem Value="All" Text="All" />
        <asp:ListItem Value="Exists" Text="Image Exists" />
        <asp:ListItem Value="NotExists" Text="Image Not Exists" />
    </asp:DropDownList>
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search..."></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    <asp:Table ID="TableBody" runat="server" CssClass="table table-striped table-bordered">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Identification Number</asp:TableHeaderCell>
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Gender</asp:TableHeaderCell>
            <asp:TableHeaderCell>Type</asp:TableHeaderCell>
            <asp:TableHeaderCell>Image</asp:TableHeaderCell>
            <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

    <div class="pagination-controls">
        <asp:LinkButton ID="lnkPrevious" runat="server" CssClass="btn btn-primary" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
        <asp:Label ID="lblPageNumber" runat="server" CssClass="ml-2 mr-2"></asp:Label>
        <asp:LinkButton ID="lnkNext" runat="server" CssClass="btn btn-primary" OnClick="lnkNext_Click">Next</asp:LinkButton>
    </div>
        </div>
</asp:Content>
