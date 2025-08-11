<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="hr_cam.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- <div class="page-header align-items-start min-vh-100" style="background-color:#lama-af0000;"> -->
    <div class="page-header align-items-start min-vh-100" style="background-color: #e01739;">
        <!-- <span class="mask bg-gradient-dark opacity-6"></span> -->
        <span class="mask bg-gradient-light opacity-6"></span>
        <div class="container my-auto">
            <div class="row">
                <div class="col-lg-4 col-md-8 col-12 mx-auto">
                    <div class="card z-index-0 fadeIn3 fadeInBottom">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                            <%--<div class="bg-gradient-primary border-radius-lg py-3 pe-1"
                                style="background-image: initial; box-shadow: 0 4px 20px 0 rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(166, 6, 6, 0.84) !important;">
                                    <img src="bootstrap/images2.jpeg" class="img-fluid" alt="">
                            </div>--%>
                            <div class="bg-gradient-primary border-radius-lg py-3 pe-1"
                                style="background-image: initial; box-shadow: 0 4px 20px 0 rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(166, 6, 6, 0.84) !important; display: flex; justify-content: center; align-items: center;">
                                <%--<img src="bootstrap/danawa.jpg" class="img-fluid" alt="" style="width: 180px;">--%>
                                <img src="bootstrap/images2.jpeg" class="img-fluid" alt="">
                            </div>

                        </div>
                        <div class="card-body">
                            <form action="#" method="post">
                                <%--<div class="input-group input-group-outline my-3">
                                    <label class="form-label">Email</label>
                                    <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="input-group input-group-outline mb-3">
                                    <label class="form-label">Password</label>
                                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                                </div>--%>
                                <div class="mb-3">
                                    <label for="exampleFormControlTextarea1" class="form-label">Email</label>
                                    <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label for="exampleFormControlTextarea1" class="form-label">Password</label>
                                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="text-center">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn" BackColor="#e01739" ForeColor="White" Text="Sign In" OnClick="Button1_Click1" />
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
