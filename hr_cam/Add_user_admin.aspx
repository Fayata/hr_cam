<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Add_user_admin.aspx.cs" Inherits="hr_cam.Add_user_admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <div class="row">
            <div class="col-12">
                <div class="card my-4">
                    <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                        <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
                            <h5 class="text-white text-capitalize ps-3">Add Users Admin</h5>
                        </div>
                    </div>
                    <div class="card-body px-0 pb-2">
                        <div style="margin: 15px;">
                                <div class="col-lg-6 mb-3">
                                    <div>
                                        <label class="form-label">Name</label>
                                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-3">
                                    <div>
                                        <label class="form-label">Role</label>
                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-3">
                                    <div>
                                        <label class="form-label">Email Address</label>
                                        <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-3">
                                        <label class="form-label">Password</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                        <span class="input-group-text">
                                            <%--<span class="material-icons" onclick="TogglePasswordVisibility('TextBox3', 'visibilityIcon1')" id="visibilityIcon1">visibility</span>--%>
                                            <i class="fa-solid fa-eye" onclick="TogglePasswordVisibility('TextBox3', 'visibilityIcon1')" id="visibilityIcon1"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-3">
                                        <label class="form-label">Confirm Password</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                        <span class="input-group-text">
                                            <%--<span class="material-icons" onclick="TogglePasswordVisibility('TextBox4', 'visibilityIcon2')" id="visibilityIcon2">visibility</span>--%>
                                            <i class="fa-solid fa-eye" onclick="TogglePasswordVisibility('TextBox4', 'visibilityIcon2')" id="visibilityIcon2"></i>
                                        </span>
                                    </div>
                                </div>
                                <%--<div class="col-lg-6 mb-3">
                                    <div>
                                        <label class="form-label">Site Access</label>
                                        <asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>--%>
                            <div class="col-lg-6">
                                <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Save" OnClick="Button1_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function TogglePasswordVisibility(inputId, iconId) {
            var passwordInput = document.getElementById(inputId);
            //alert(passwordInput);
            var visibilityIcon = document.getElementById(iconId);

            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                visibilityIcon.classList.remove("fa-eye");
                visibilityIcon.classList.add("fa-eye-slash");
            } else {
                passwordInput.type = "password";
                visibilityIcon.classList.remove("fa-eye-slash");
                visibilityIcon.classList.add("fa-eye");
            }
        }
    </script>
</asp:Content>
