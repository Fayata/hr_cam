<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="hr_cam.report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container-fluid py-4">
                <div class="row mb-5">
                    <div class="col-md-6">
                                <table style="width: 100%;margin:10px;" aria-describedby="filter">
                                    <tr>
                                        <th style="color:white;">From</th>
                                        <th></th>
                                        <th style="color:white;">To</th>
                                        <th></th>
                                    </tr>
                                    <tr>
                                        
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                                        </td>
                                        <td>-</td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Filter" OnClick="Button1_Clicked" />
                                        </td>
                                    </tr>
                                </table>
                    </div>

                </div>
                <div class="row mb-5">
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-user-group"></i>
                                    <%--<i class="material-icons opacity-10">today</i>--%>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Face Detected Today</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card1')">
                                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">today</i>--%>
                                    <i class="fa-solid fa-user-slash"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Blacklist Detection</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card2')">
                                            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                    <i class="fa-solid fa-user-group"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Face Detected This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card3')">
                                            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                    <i class="fa-solid fa-user-slash"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Blacklist Detection This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card4')">
                                            <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end baris 1 -->
                <!-- baris 2 -->
                <div class="row mb-5">
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">today</i>--%>
                                    <i class="fa-solid fa-user-check"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <%--<p class="text-sm mb-0 text-capitalize">Today's Face Recognized</p>--%>
                                    <p class="text-sm mb-0 text-capitalize">Today's Unique <br />Fit To Work</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card5')">
                                            <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">today</i>--%>
                                    <%--<i class="fa-solid fa-user"></i>--%>
                                    <i class="fa-solid fa-user-xmark"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Not Fit <br />To Work</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card6')">
                                            <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                    <i class="fa-solid fa-user-check"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Unique Fit To Work <br /> This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card7')">
                                            <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                    <%--<i class="fa-solid fa-user"></i>--%>
                                    <i class="fa-solid fa-user-xmark"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Not Fit To Work <br /> This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card8')">
                                            <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end baris 2 -->
                <!-- baris 3 -->
                <div class="row mb-5">
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Unique <br /> License Plate Detected</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card9')">
                                            <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-traffic-light"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Traffic Detected<br />&nbsp;</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card10')">
                                            <asp:Label ID="Label19" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Unique License Plate <br /> Detected This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card11')">
                                            <asp:Label ID="Label11" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-traffic-light"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Traffic Detected This Month<br />&nbsp;</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card12')">
                                            <asp:Label ID="Label20" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-5">
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">today</i>--%>
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Unique <br /> License Plate Recognized</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card13')">
                                            <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Today's Unique <br /> Expire License Plate</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card14')">
                                            <asp:Label ID="Label14" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
            
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Unique License Plate <br /> Recognized This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card15')">
                                            <asp:Label ID="Label16" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                        <div class="card">
                            <div class="card-header p-3 pt-2">
                                <div
                                    class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                                    <i class="fa-solid fa-car-rear"></i>
                                </div>
                                <div class="text-end pt-1">
                                    <p class="text-sm mb-0 text-capitalize">Unique Expire <br /> License Plate This Month</p>
                                    <h4 class="mb-0">
                                        <a href="#" onclick="Summary('Card16')">
                                            <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-footer" style="padding: 1em;">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-5">
        <%--<div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
            <div class="card">
                <div class="card-header p-3 pt-2">
                    <div
                        class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                        <i class="fa-solid fa-car-rear"></i>
                    </div>
                    <div class="text-end pt-1">
                        <p class="text-sm mb-0 text-capitalize">Today's Unique <br /> Expiring License Plate</p>
                        <h4 class="mb-0">
                            <a href="#" onclick="Summary('Card17')">
                                <asp:Label ID="Label15" runat="server" Text="Label"></asp:Label>
                            </a>
                        </h4>
                    </div>
                </div>
                <div class="card-footer" style="padding: 1em;">
                </div>
            </div>
        </div>--%>
        <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
            <div class="card">
                <div class="card-header p-3 pt-2">
                    <div
                        class="icon icon-lg icon-shape bg-gradient-dark shadow-dark text-center border-radius-xl mt-n4 position-absolute">
                        <%--<i class="material-icons opacity-10">today</i>--%>
                        <i class="fa-solid fa-car-on"></i>
                    </div>
                    <div class="text-end pt-1">
                        <p class="text-sm mb-0 text-capitalize">Today's Unique <br> Unrecognized License Plate</p>
                        <h4 class="mb-0">
                            <a href="#" onclick="Summary('Card18')">
                                <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                            </a>
                        </h4>
                    </div>
                </div>
                <div class="card-footer" style="padding: 1em;">
                </div>
            </div>
        </div>
        <%--<div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
            <div class="card">
                <div class="card-header p-3 pt-2">
                    <div
                        class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                        <i class="fa-solid fa-car-rear"></i>
                    </div>
                    <div class="text-end pt-1">
                        <p class="text-sm mb-0 text-capitalize">Unique Expiring<br /> License Plate This Month</p>
                        <h4 class="mb-0">
                            <a href="#" onclick="Summary('Card19')">
                                <asp:Label ID="Label18" runat="server" Text="Label"></asp:Label>
                            </a>
                        </h4>
                    </div>
                </div>
                <div class="card-footer" style="padding: 1em;">
                </div>
            </div>
        </div>--%>
        <div class="col-xl-3 col-sm-6">
            <div class="card">
                <div class="card-header p-3 pt-2">
                    <div
                        class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                        <%--<i class="material-icons opacity-10">calendar_month</i>--%>
                        <i class="fa-solid fa-car-on"></i>
                    </div>
                    <div class="text-end pt-1">
                        <p class="text-sm mb-0 text-capitalize">Unique Unrecognized <br /> License Plate This Month</p>
                        <h4 class="mb-0">
                            <a href="#" onclick="Summary('Card20')">
                                <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
                            </a>
                        </h4>
                    </div>
                </div>
                <div class="card-footer" style="padding: 1em;">
                </div>
            </div>
        </div>
    </div>
        <%--</div>
      </div>--%>
    </div>
    <script>
        // Function to refresh the page every 30 seconds (30000 milliseconds)
        function autoRefresh() {
            setTimeout(function () {
                location.reload();
            }, 30000); // 30000 milliseconds = 30 seconds
        }

        // Call autoRefresh when the page is fully loaded
        //window.onload = autoRefresh;
    </script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#DataTable').DataTable();
    });
    function Summary(card) {
        //console.log()
    }
</script>
</asp:Content>
