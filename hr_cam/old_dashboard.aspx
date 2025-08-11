<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="old_dashboard.aspx.cs" Inherits="hr_cam.old_dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        
        <!-- card 4 diatas -->
        <!-- baris 1 -->
        <div class="row mb-5">
            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-user-group"></i> Face Detected</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card1')">
                                                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card3')">
                                                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-user-slash"></i> Blacklist</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card2')">
                                                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card4')">
                                                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-user-check"></i> Unique Fit To Work</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card5')">
                                                    <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">
                                                Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card7')">
                                                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            
        </div>
        <!-- end baris 1 -->


        <!-- baris 2 -->
        <!-- <div class="row mb-5">
            
        </div> -->

        <!-- end baris 2 -->
        <!-- baris 3 -->
        <div class="row mb-5">
            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-user-xmark"></i> Invalid or Exception</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <%--<i class="fa-solid fa-user"></i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">
                                                Daily
                                            </p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card6')">
                                                    <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <%--<i class="fa-solid fa-user"></i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card8')">
                                                    <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-car-rear"></i> Unique License Plate</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">
                                                Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card9')">
                                                    <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">
                                                Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card11')">
                                                    <asp:Label ID="Label11" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-traffic-light"></i> Traffic Detected</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card10')">
                                                    <asp:Label ID="Label19" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card12')">
                                                    <asp:Label ID="Label20" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            
        </div>
        <!-- end baris 3 -->

        <!-- baris 4 -->
        <!-- <div class="row mb-5">
            
        </div> -->
        <!-- end baris 4 -->

        <!-- baris 5 -->
        <div class="row mb-5">
            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-car-rear"></i> Unique License Plate Recognized</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card13')">
                                                    <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card15')">
                                                    <asp:Label ID="Label16" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-car-rear"></i> Unique Expire License Plate</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card14')">
                                                    <asp:Label ID="Label14" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card16')">
                                                    <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-sm-12 mb-xl-0 mb-4">
                <div class="card" style="background-color: #E5E1DA;">
                    <h5 style="color: white;" class="card-header bg-gradient-primary"><i class="fa-solid fa-car-on"></i> Unique Unrecognized License Plate</h5>
                    <div class="card-body mt-3">
                        <div class="row">
                            <div class="col-xl-6 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-success shadow-success text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">today</i>--%>
                                            <i class="fa-solid fa-calendar-day"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Daily</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card18')">
                                                    <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-sm-6">
                                <div class="card">
                                    <div class="card-header pt-2">
                                        <div
                                            class="icon icon-lg icon-shape bg-gradient-warning shadow-warning text-center border-radius-xl mt-n4 position-absolute">
                                            <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                            <i class="fa-solid fa-calendar-days"></i>
                                        </div>
                                        <div class="text-end pt-1">
                                            <p class="text-sm mb-0 text-capitalize">Monthly</p>
                                        </div>
                                        <div class="text-center pt-2 mt-3">
                                            <!-- <br> -->
                                            <h3 class="mb-0">
                                                <a href="#" onclick="Summary('Card20')">
                                                    <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
                                                </a>
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- end baris 5 -->

                <div class="row justify-content-right mb-5">
            <div class="col-md-12">
                <div class="card z-index-2 ">
                    <%--<button onclick="connectWebSocket()">Connect to WebSocket</button>--%>
                        <table style="width: 100%;margin:10px;" aria-describedby="filter">
                            <tr>
                                <th colspan="3">Auto Refresh</th>
                                <th>From</th>
                                <th></th>
                                <th>To</th>
                                <th>Limit</th>
                                <th></th>
                            </tr>
                            <tr>
                                <td><input type="checkbox" name="auto_refresh" id="auto_refresh" checked></td> 
                                <td>
                                    <label for="refresh_interval">Interval Refresh perdetik:</label>
                                </td>
                                <td>
                                    <input type="number" id="refresh_interval" value="30" min="1" class="form-control"> <!-- default 30 detik -->
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" TextMode="Date" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
<%--                                    <input type="date" class="form-control" name="from" id="from"
                                        style="border: 2px solid #0001;" runat="server" value="">--%>

                                </td>
                                <td>-</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server" TextMode="Date" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
<%--                                    <input type="date" class="form-control" runat="server" name="to" id="to"
                                        style="border: 2px solid #0001;" value="">--%>

                                </td>
                                <td><asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                                <td>
                                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Filter" OnClick="Button1_Click" />
                                    <%--<asp:Button ID="Button2" CssClass="btn btn-danger" runat="server" Text="Report" />--%>
                                    <%--<input type="submit" class="btn btn-success" name="submit" value="Filter"
                                        style="margin-bottom: 0px;margin-right: 5px;">
                                    <input type="submit" class="btn btn-danger" name="report" value="Report"
                                        style="margin-bottom: 0px;">--%>
                                </td>
                            </tr>
                        </table>
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <div class="col mt-4 mb-3">
                    <div class="card z-index-2 ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <!-- <div class="border-radius-lg py-3 pe-1" style="background: darkkred;"> -->
                            <div class="border-radius-lg py-3 pe-1" style="background: #e01739;">
                                <h5 class="text-center" style="color: white;">Event History Traffic</h5>
                            </div>
                        </div>
                        <div class="card-body overflow-auto">
                            <div class="table-responsive">
                                <%--<table class="table" style="width: 100%;" id="isitabel">
                                </table>--%>
                                <table id="DataTable3" class="table table-striped table-bordered" aria-describedby="traffic_list">
                                    <thead>
                                      <tr>
                                        <th>Image</th>
                                        <th>Occurred At</th>
                                        <th>Location</th>
                                        <th>Site</th>
                                      </tr>
                                    </thead>
                                    <tbody runat="server" id="Tbody2">
                                    </tbody>
                                  </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <div class="col mt-4 mb-3">
                    <div class="card z-index-2 ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="border-radius-lg py-3 pe-1" style="background: #e01739;">
                                <h5 class="text-center" style="color: white;">Event History DPO</h5>
                            </div>
                        </div>
                        <div class="card-body overflow-auto">
                            <h6 class="mb-0 " id="dataDisplay2"></h6>
                            <div class="table-responsive">
                                <%--<table class="table" style="width: 100%;" id="isitabel">
                                </table>--%>
                                <table id="DataTable2" class="table table-striped table-bordered" aria-describedby="dpo_list">
                                    <thead>
                                      <tr>
                                        <th colspan="2">Image</th>
                                        <th>Occurred At</th>
                                        <th>Location</th>
                                        <th>Site</th>
                                        <th>Person</th>
                                        <th>Status</th>
                                      </tr>
                                    </thead>
                                    <tbody runat="server" id="Tbody1">
                                    </tbody>
                                  </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <%--<div class="col-lg-8">
                <div class="row">
                    <div class="col-lg-12 col-md-10 mt-4 mb-4">
                        <div class="card z-index-2 ">
                            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                                <div class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
                                    style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%)">
                                    <div class="chart">
                                        <canvas id="chart-bars" class="chart-canvas" height="170"></canvas>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <h6 class="mb-0 ">Jumlah Tangkapan MCU Berdasarkan Zona
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-10 mt-4 mb-4">
                        <div class="card z-index-2  ">
                            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                                <div class="bg-gradient-success shadow-success border-radius-lg py-3 pe-1"
                                    style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%);">
                                    <div class="chart">
                                        <canvas id="chart-line" class="chart-canvas" height="170"></canvas>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <h6 class="mb-0 "> Jumlah Tangkapan Kamera Berdasarkan Zona </h6>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div class="col">

                <div class="col mt-4 mb-3">
                    <div class="card z-index-2 ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="bg-gradient-dark shadow-dark border-radius-lg py-3 pe-1">
                                <h5 class="text-center" style="color: white;">Event History Invalid or Exception</h5>
                            </div>
                        </div>
                        <div class="card-body overflow-auto">
                            <h6 class="mb-0 " id="dataDisplay"></h6>
                            <%--<table class="mb-3">
                                <tr>
                                    <td><label>Limit:</label></td>
                                    <td><asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                                    <td><asp:Button ID="Button2" CssClass="btn btn-success" runat="server" Text="Apply" OnClick="Button2_Click" /></td>
                                </tr>
                            </table>--%>
                            
                            <div class="table-responsive">
                                <%--<table class="table" style="width: 100%;" id="isitabel">
                                </table>--%>
                                <table id="DataTable" class="table table-striped table-bordered" aria-describedby="event_list">
                                    <thead>
                                      <tr>
                                        <th colspan="2">Image</th>
                                        <th>Occurred At</th>
                                        <th>Location</th>
                                        <th>Site</th>
                                        <th>Person</th>
                                        <th>Status</th>
                                      </tr>
                                    </thead>
                                    <tbody runat="server" id="TableBody">
                                    </tbody>
                                  </table>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>

    </div>
    <script>
        var socket;
        function connectWebSocket() {
            // Gantilah 'ws://your-server/WebSocketHandler.ashx' dengan alamat WebSocket Anda yang benar
            socket = new WebSocket("wss://localhost:44388/WebSocketHandler.ashx");

            // Ketika koneksi WebSocket berhasil dibuka
            socket.onopen = function (event) {
                console.log("WebSocket is open now.");
                // Kirim pesan awal atau lakukan inisialisasi lain jika perlu
                socket.send("Hello, server!");
                startHeartbeat(); // Memulai pengiriman ping secara berkala
            };

            // Ketika menerima pesan dari server
            socket.onmessage = function (event) {
                console.log("Message from server:", event.data);
                // Tambahkan logika untuk menangani pesan dari server di sini
            };

            // Ketika koneksi WebSocket ditutup
            socket.onclose = function (event) {
                console.log("WebSocket is closed now.", event.reason);
                // Coba hubungkan ulang setelah beberapa detik jika perlu
                setTimeout(function () {
                    connectWebSocket();
                }, 5000); // Coba hubungkan ulang setelah 5 detik
            };

            // Ketika ada kesalahan dalam koneksi WebSocket
            socket.onerror = function (error) {
                console.error("WebSocket Error:", error);
                // Tambahkan penanganan kesalahan sesuai kebutuhan
            };
        }

        // Fungsi untuk memulai pengiriman ping secara berkala untuk menjaga koneksi tetap aktif
        function startHeartbeat() {
            setInterval(function () {
                if (socket.readyState === WebSocket.OPEN) {
                    socket.send("ping"); // Kirim pesan "ping" ke server untuk menjaga koneksi tetap aktif
                }
            }, 5000); // Kirim ping setiap 5 detik
        }


    </script>
    <script>
        // Function to refresh the page every 30 seconds (30000 milliseconds)
        //function autoRefresh() {
        //    setTimeout(function () {
        //        location.reload();
        //    }, 30000); // 30000 milliseconds = 30 seconds
        //}
        function autoRefresh() {
            var interval = document.getElementById('refresh_interval').value * 1000; // convert detik ke millisecond
            setTimeout(function () {
                location.reload();
            }, interval);
        }

        // Call autoRefresh when the page is fully loaded
        //window.onload = autoRefresh;
        //window.onload = function () {
        //    //connectWebSocket();
        //    var checkbox = document.getElementById('auto_refresh');
        //    if (checkbox.checked) {
        //        autoRefresh();
        //    }

        //    checkbox.addEventListener('change', function () {
        //        if (checkbox.checked) {
        //            autoRefresh();
        //        }
        //    });
        //};
        function saveSettings() {
            var isChecked = document.getElementById('auto_refresh').checked;
            var interval = document.getElementById('refresh_interval').value;

            localStorage.setItem('auto_refresh_enabled', isChecked);
            localStorage.setItem('refresh_interval', interval);
        }

        // Fungsi untuk memuat status dari localStorage ketika halaman dibuka
        function loadSettings() {
            var isChecked = localStorage.getItem('auto_refresh_enabled') === 'true';
            var interval = localStorage.getItem('refresh_interval') || 30; // default 30 detik jika tidak ada di localStorage

            document.getElementById('auto_refresh').checked = isChecked;
            document.getElementById('refresh_interval').value = interval;

            if (isChecked) {
                autoRefresh();
            }
        }
        window.onload = function () {
            loadSettings(); // Muat status dan interval ketika halaman dibuka

            var checkbox = document.getElementById('auto_refresh');
            var intervalInput = document.getElementById('refresh_interval');

            // Jalankan autoRefresh ketika checkbox diaktifkan
            checkbox.addEventListener('change', function () {
                saveSettings(); // Simpan status checkbox ke localStorage
                if (checkbox.checked) {
                    autoRefresh();
                }
            });

            // Simpan interval baru saat diubah dan reset auto-refresh
            intervalInput.addEventListener('input', function () {
                saveSettings(); // Simpan interval baru ke localStorage
                if (checkbox.checked) {
                    autoRefresh(); // Reset auto-refresh dengan interval baru
                }
            });
        };

        function Summary(card) {
            window.open('card_detail.aspx?card='+card, '_blank');
        }
    </script>
    <script>
        function fillChart(labels, kadaluarsa, hampirkadaluarsa, jumlah) {
            var ctx = document.getElementById("chart-bars").getContext("2d");

            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Kadaluarsa',
                        data: kadaluarsa,
                        backgroundColor: 'rgba(255, 23, 72, 0.2)',
                        borderColor: 'rgba(255,74,112,1)',
                        borderWidth: 1,
                        order: 2
                    }, {
                        label: 'Hampir Kadaluarsa',
                        data: hampirkadaluarsa,
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1,
                        order: 2
                    }, {
                        label: 'Jumlah',
                        data: jumlah,
                        type: 'line',
                        borderColor: 'rgba(255, 206, 86, 1)',
                        borderWidth: 2,
                        order: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            stacked: true
                        }]
                    },
                    onClick: function (event, clickedItem) {
                        if (clickedItem.length > 0) {
                            var label = this.data.labels[clickedItem[0]['index']];
                            //alert(label);
                            var from = document.getElementById("TextBox1").value;
                            var to = document.getElementById("TextBox2").value;
                            //alert(from);
                            //alert(to);
                            $.ajax({
                                type: 'POST',
                                url: 'old_dashboard.aspx/GetData',
                                //data: JSON.stringify({ location: label, from: new Date(from).toISOString(), to: new Date(to).toISOString() }), // Konversi tanggal ke format ISO string
                                data: JSON.stringify({ location: label, from: from, to: to }), // Konversi tanggal ke format ISO string
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (response) {
                                    console.log(response); // Tampilkan response untuk memeriksa isinya

                                    // Cek apakah response.d ada dan bukan null
                                    if (response.d) {
                                        var cameraEvents = response.d; // Tidak perlu lagi parse JSON

                                        for (var i = 0; i < cameraEvents.length; i++) {
                                            var event = cameraEvents[i];
                                            console.log(event.Camera, event.OccurredAt, event.Location, event.Name, event.PersonType, event.ExpiredAt);
                                        }
                                    } else {
                                        console.error("Response tidak valid: ", response);
                                    }
                                },
                                error: function (xhr, status, error) {
                                    console.error(xhr.responseText);
                                }
                            });

                        }
                    }
                }
            });
        }

        function fillChart2(labels, pegawai, blacklist, tamu, jumlah) {
            var ctx2 = document.getElementById("chart-line").getContext("2d");

            var myChart = new Chart(ctx2, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Pegawai',
                        data: pegawai,
                        backgroundColor: 'rgba(255, 23, 72, 0.2)',
                        borderColor: 'rgba(255,74,112,1)',
                        borderWidth: 1,
                        order: 2
                    }, {
                        label: 'Blacklist',
                        data: blacklist,
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1,
                        order: 2
                    },{
                        label: 'Tamu',
                        data: tamu,
                        backgroundColor: 'rgb(36,0,179, 0.2)',
                        borderColor: 'rgb(47,0,230, 1)',
                        borderWidth: 1,
                        order: 2
                    }, {
                        label: 'Jumlah',
                        data: jumlah,
                        type: 'line',
                        borderColor: 'rgba(255, 206, 86, 1)',
                        borderWidth: 2,
                        order: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            stacked: true
                        }]
                    },
                    onClick: function (event, clickedItem) {
                        if (clickedItem.length > 0) {
                            var label = this.data.labels[clickedItem[0]['index']];
                            //alert(label);
                            var from = document.getElementById("TextBox1").value;
                            var to = document.getElementById("TextBox2").value;
                            //alert(from);
                            //alert(to);

                        }
                    }
                }
            });
        }
    </script>

</asp:Content>
