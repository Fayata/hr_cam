<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="statistics.aspx.cs" Inherits="hr_cam.statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-control {
            border: 1px solid #d2d6da;

        }
        .clickable {
            cursor: pointer;
            /*text-decoration: underline;*/
        }

    </style>
    <%--<script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2.0.0"></script>--%>
<script src="bootstrap2/js/chartjs-plugin-datalabels.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container-fluid py-4">
        <div class="row justify-content-right mb-5">
            <div class="col-md-12">
                <div class="card z-index-2 ">
                        <table style="width: 100%;margin:10px;" aria-describedby="filter">
                            <tr>
                                <th colspan="3">Auto Refresh</th>
                                <th>Camera Sites</th>
                                <th>From</th>
                                <th></th>
                                <th>To</th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr>
                                <td><input type="checkbox" name="auto_refresh2" id="auto_refresh2" checked> Auto Refresh</td> 
                                <td>
                                    <label for="refresh_interval2">Interval Refresh perdetik:</label>
                                </td>
                                <td>
                                    <input type="number" id="refresh_interval2" value="30" min="1" class="form-control"> <!-- default 30 detik -->
                                </td>
                                <td><asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server"></asp:DropDownList></td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                                </td>
                                <td>-</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td valign="middle">
                                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Filter" OnClick="Button1_Click" />
                                </td>
                            </tr>
                        </table>
                </div>
            </div>
            <%--<div class="col-md-6">
                <div class="card z-index-2 ">
                        <table style="width: 100%;margin:10px;">
                            <tr>
                                <th>Camera Sites</th>
                                <th>Camera Location</th>
                                <th>From</th>
                                <th></th>
                                <th>To</th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"></asp:DropDownList></td>
                                <td><asp:DropDownList ID="DropDownList3" CssClass="form-control" runat="server"></asp:DropDownList></td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server" TextMode="Date" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                                </td>
                                <td>-</td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server" TextMode="Date" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td valign="middle">
                                    <asp:Button ID="Button2" CssClass="btn btn-danger" runat="server" Text="Report" OnClick="Button2_Click" />
                                </td>
                            </tr>
                        </table>
                </div>
            </div>--%>
        </div>
        
        <div class="row">
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">groups</i>--%>
                            <i class="fa-solid fa-user-group"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">People Detection</p>
                            <a href="#" onclick="Summary('Card2.1')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">groups_2</i>--%>
                            <i class="fa-solid fa-user-slash"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">DPO Detection</p>
                            <a href="#" onclick="Summary('Card2.2')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
            
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">today</i>--%>
                            <i class="fa-solid fa-car-rear"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">License Plate Recognized</p>
                            <a href="#" onclick="Summary('Card2.3')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-5">
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">today</i>--%>
                            <i class="fa-solid fa-user-check"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">Person Valid Entry</p>
                            <a href="#" onclick="Summary('Card2.4')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">today</i>--%>
                            <%--<i class="fa-solid fa-user"></i>--%>
                            <i class="fa-solid fa-user-xmark"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">Invalid or Exception</p>
                            <a href="#" onclick="Summary('Card2.5')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-sm-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">today</i>--%>
                            <i class="fa-solid fa-car-on"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">License Plate Unrecognized</p>
                            <a href="#" onclick="Summary('Card2.6')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-5">
            <div class="col-xl-4 col-sm-4 mb-xl-0 mb-4">
                <div class="card">
                    <div class="card-header p-3 pt-2">
                        <div
                            class="icon icon-lg icon-shape bg-gradient-primary shadow-primary text-center border-radius-xl mt-n4 position-absolute">
                            <%--<i class="material-icons opacity-10">today</i>--%>
                            <i class="fa-solid fa-traffic-light"></i>
                        </div>
                        <div class="text-end pt-1">
                            <p class="text-sm mb-0 text-capitalize">Traffic Detected</p>
                            <a href="#" onclick="Summary('Card2.7')">
                                <h4 class="mb-0">
                                    <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                                </h4>
                            </a>
                        </div>
                    </div>
                    <div class="card-footer" style="padding: 1em;">
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-lg-6 col-md-10 mt-4 mb-4">
                <div class="card z-index-2">
                    <div class="card-header">
                    </div>
                    <div class="card-body" style="height:650px; overflow:auto;">
                        <table id="DataTable3" class="table table-striped table-bordered" aria-describedby="list_camera">
                            <thead>
                                <tr>
                                <th></th>
                                <th style="vertical-align:middle">Unique Face Recognized</th>
                                <th style="vertical-align:middle">Face Unrecognized</th>
                                <th style="vertical-align:middle">Unique License Plate Recognized</th>
                                <th style="vertical-align:middle">Unique License Plate Unrecognized</th>
                                </tr>
                            </thead>
                            <tbody runat="server" id="TbodyPie">
                            </tbody>
                            </table>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-10 mt-4 mb-4">
                <div class="card z-index-2 ">
                    <div class="card-header">
                        <%--<div class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
                            style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%)">
                            
                        </div>--%>
                    </div>
                    <div class="card-body">
                        <%--<h6 class="mb-0 ">Jumlah Deteksi Kamera</h6>--%>
                        <div class="chart">
                            <canvas id="chart-pie" class="chart-canvas" height="170"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<div class="row mt-4">
            <div class="row">
                <div class="col-lg-12 col-md-10 mt-4 mb-4">
                    <div class="card z-index-2  ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
                                style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%);">
                                <div class="chart">
                                    <canvas id="chart-line" class="chart-canvas" height="170"></canvas>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <h6 class="mb-0 "> Jumlah Tangkapan Orang Berdasarkan Zona </h6>
                        </div>
                    </div>
                </div>
            </div>
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
                            <h6 class="mb-0 ">Jumlah Tangkapan Kendaraan Berdasarkan Zona </h6>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>--%>
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
        <div class="row mt-4" id="fokusEvent">
            <%--<div class="col-lg-8">
                
            </div>--%>
            <div class="col">

                <div class="col mt-4 mb-3">
                    <div class="card z-index-2 " >
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="bg-gradient-dark shadow-dark border-radius-lg py-3 pe-1">
                                <h5 class="text-center" style="color: white;">Event History</h5>
                            </div>
                        </div>
                        <div class="card-body overflow-auto">
                            <h6 class="mb-0 " id="dataDisplay"></h6>
                            <table class="table table-borderless">
                                <tr>
                                    <th>Location</th>
                                    <th>Person</th>
                                    <th>Status</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td><asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"></asp:DropDownList></td>
                                    <td><asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                                    <td><asp:DropDownList ID="DropDownList3" CssClass="form-control" runat="server"></asp:DropDownList></td>
                                    <td><asp:Button ID="Button2" CssClass="btn btn-success" runat="server" Text="Search" OnClick="Button2_Click" /></td>
                                </tr>
                            </table>
                            <div class="table-responsive">
                                <%--<table class="table" style="width: 100%;" id="isitabel">
                                </table>--%>
                                <table id="DataTable" class="table table-striped table-bordered" aria-describedby="event_list">
                                    <thead>
                                      <tr>
                                        <th>Image</th>
                                        <th></th>
                                        <th>Occurred At</th>
                                        <th>Location</th>
                                        <th>Site</th>
                                        <th>Person</th>
                                        <th>Plate Number</th>
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
        $(document).ready(function () {
            //$('#table_users').DataTable();
            //$('#Table1').DataTable();
            $('#DataTable').DataTable({
                searching: false // Disable search box
            });
        });
        // Function to refresh the page every 30 seconds (30000 milliseconds)
        //function autoRefresh() {
        //    setTimeout(function () {
        //        location.reload();
        //    }, 30000); // 30000 milliseconds = 30 seconds
        //}
        function autoRefresh() {
            var interval = document.getElementById('refresh_interval2').value * 1000; // convert detik ke millisecond
            setTimeout(function () {
                location.reload();
            }, interval);
        }
        // Call autoRefresh when the page is fully loaded
        //window.onload = autoRefresh;
        //window.onload = function () {
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
            var isChecked = document.getElementById('auto_refresh2').checked;
            var interval = document.getElementById('refresh_interval2').value;

            localStorage.setItem('auto_refresh_enabled2', isChecked);
            localStorage.setItem('refresh_interval2', interval);
        }

        // Fungsi untuk memuat status dari localStorage ketika halaman dibuka
        function loadSettings() {
            var isChecked = localStorage.getItem('auto_refresh_enabled2') === 'true';
            var interval = localStorage.getItem('refresh_interval2') || 30; // default 30 detik jika tidak ada di localStorage

            document.getElementById('auto_refresh2').checked = isChecked;
            document.getElementById('refresh_interval2').value = interval;

            if (isChecked) {
                autoRefresh();
            }
        }
        window.onload = function () {
            loadSettings(); // Muat status dan interval ketika halaman dibuka
            
            var checkbox = document.getElementById('auto_refresh2');
            var intervalInput = document.getElementById('refresh_interval2');

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
    </script>
    <script>
        function setColumnSession(camera, filter) {
            // Lakukan postback dan kirim data kolom ke server
            __doPostBack('SetColumnSession', camera + ',' + filter);
        }
        function scrollToElement() {
            if ('<%= Session["lokasi"] %>' !== '') {
                var lokasi = '<%= Session["lokasi"] %>';
                if (lokasi == "none") { } else {
                    //alert(lokasi);
                    setTimeout(function () {
                        var element = document.getElementById("fokusEvent");
                        if (element) {
                            element.scrollIntoView({
                                behavior: 'smooth',
                                block: 'start' // Posisi elemen di viewport
                            });
                        }
                    }, 500); // Tambahkan jeda waktu untuk memastikan halaman sudah selesai render
                }
            }
        }


    </script>
    <script>
        function getRandomColor() {
            const letters = '0123456789ABCDEF';
            let color = '#';
            for (let i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
            return color;
        }
        //function fillChart(labels, plateRegister, plateUnregister, jumlah) {
        //    var ctx = document.getElementById("chart-bars").getContext("2d");

        //    var myChart = new Chart(ctx, {
        //        type: 'bar',
        //        data: {
        //            labels: labels,
        //            datasets: [{
        //                label: 'Plate Number Register',
        //                data: plateRegister,
        //                //backgroundColor: 'rgba(255, 23, 72, 0.2)',
        //                backgroundColor: 'rgba(255,74,112,0.5)',
        //                borderColor: 'rgba(255,74,112,1)',
        //                //borderWidth: 1,
        //                //order: 2
        //                borderWidth: 2,
        //                borderRadius: 5
        //            }, {
        //                label: 'Plate Number Unregister',
        //                data: plateUnregister,
        //                //backgroundColor: 'rgba(54, 162, 235, 0.2)',
        //                backgroundColor: 'rgba(54, 162, 235, 0.5)',
        //                borderColor: 'rgba(54, 162, 235, 1)',
        //                //borderWidth: 1,
        //                //order: 2
        //                borderWidth: 2,
        //                borderRadius: 5
        //                }
        //            //{
        //            //    label: 'Jumlah',
        //            //    data: jumlah,
        //            //    type: 'line',
        //            //    borderColor: 'rgba(255, 206, 86, 1)',
        //            //    borderWidth: 2,
        //            //    order: 1
        //            //    }
        //            ]
        //        },
        //        options: {
        //            scales: {
        //                yAxes: {
        //                    //stacked: true,
        //                    ticks: {
        //                        callback: function (value) {
        //                            return Number.isInteger(value) ? value : '';
        //                        }
        //                    }
        //                }
        //            },
        //            onClick: function (event, elements) {
        //                if (elements.length > 0) {
        //                    var label = this.data.labels[elements[0].index];
        //                    var from = document.getElementById("TextBox1").value;
        //                    var to = document.getElementById("TextBox2").value;
        //                    console.log(`Label: ${label}, From: ${from}, To: ${to}`);
        //                    // Tambahkan logika lain yang Anda butuhkan
        //                }
        //            }
        //        }
        //    });
        //}

        //function fillChart2(labels, unregister, invalid, blacklist, jumlah) {
        //    var ctx2 = document.getElementById("chart-line").getContext("2d");

        //    var myChart = new Chart(ctx2, {
        //        type: 'bar',
        //        data: {
        //            labels: labels,
        //            datasets: [{
        //                label: 'Unregister',
        //                data: unregister,
        //                //backgroundColor: 'rgba(255, 23, 72, 0.2)',
        //                backgroundColor: 'rgba(255, 74, 112, 0.5)',
        //                borderColor: 'rgba(255,74,112,1)',
        //                //borderWidth: 1,
        //                //order: 2
        //                borderWidth: 2,
        //                borderRadius: 5
        //                //borderSkipped: false,
        //            }, {
        //                    label: 'Valid',
        //                    data: jumlah,
        //                backgroundColor: 'rgba(255, 206, 86,0.5)',
        //                borderColor: 'rgba(255, 206, 86, 1)',
        //                    //borderWidth: 2,
        //                //order: 2
        //                borderWidth: 2,
        //                borderRadius: 5
        //                //borderSkipped: false,
        //            }, {
        //                label: 'Invalid',
        //                data: invalid,
        //                //backgroundColor: 'rgba(54, 162, 235, 0.2)',
        //                backgroundColor: 'rgba(54, 162, 235, 0.5)',
        //                borderColor: 'rgba(54, 162, 235, 1)',
        //                //borderWidth: 1,
        //                //order: 2
        //                borderWidth: 2,
        //                borderRadius: 5
        //                //borderSkipped: false,
        //            }, {
        //                label: 'DPO',
        //                data: blacklist,
        //                //backgroundColor: 'rgb(36,0,179, 0.2)',
        //                backgroundColor: 'rgb(47,0,230, 0.5)',
        //                borderColor: 'rgb(47,0,230, 1)',
        //                //borderWidth: 1,
        //                //order: 2
        //                borderWidth: 2,
        //                 borderRadius: 5
        //                //borderSkipped: false,
        //            }]
        //        },
        //        options: {
        //            scales: {
        //                yAxes: [{
        //                    stacked: false,
        //                    ticks: {
        //                        callback: function (value, index, values) {
        //                            return Math.floor(value); // Membulatkan nilai ke bawah
        //                        }
        //                    }
        //                }]
        //            },
        //            onClick: function (event, clickedItem) {
        //                if (clickedItem.length > 0) {
        //                    var label = this.data.labels[clickedItem[0]['index']];
        //                    //alert(label);
        //                    var from = document.getElementById("TextBox1").value;
        //                    var to = document.getElementById("TextBox2").value;
        //                    //alert(from);
        //                    //alert(to);

        //                }
        //            }
        //        }
        //    });
        //}

        //function fillChart2(labels, unregister, invalid, blacklist, jumlah) {
        //    var ctx2 = document.getElementById("chart-line").getContext("2d");

        //    var myChart = new Chart(ctx2, {
        //        type: 'bar',
        //        data: {
        //            labels: labels,
        //            datasets: [{
        //                label: 'Unregister',
        //                data: unregister,
        //                backgroundColor: 'rgba(255, 74, 112, 0.5)',
        //                borderColor: 'rgba(255,74,112,1)',
        //                borderWidth: 2,
        //                borderRadius: 5
        //            }, {
        //                label: 'Valid',
        //                data: jumlah,
        //                backgroundColor: 'rgba(255, 206, 86,0.5)',
        //                borderColor: 'rgba(255, 206, 86, 1)',
        //                borderWidth: 2,
        //                borderRadius: 5
        //            }, {
        //                label: 'Invalid',
        //                data: invalid,
        //                backgroundColor: 'rgba(54, 162, 235, 0.5)',
        //                borderColor: 'rgba(54, 162, 235, 1)',
        //                borderWidth: 2,
        //                borderRadius: 5
        //            }, {
        //                label: 'DPO',
        //                data: blacklist,
        //                backgroundColor: 'rgb(47,0,230, 0.5)',
        //                borderColor: 'rgb(47,0,230, 1)',
        //                borderWidth: 2,
        //                borderRadius: 5
        //            }]
        //        },
        //        options: {
        //            scales: {
        //                yAxes: {
        //                    //stacked: true,
        //                    ticks: {
        //                        callback: function (value) {
        //                            return Number.isInteger(value) ? value : '';
        //                        }
        //                    }
        //                }
        //            },
        //            onClick: function (event, elements) {
        //                if (elements.length > 0) {
        //                    var label = this.data.labels[elements[0].index];
        //                    var from = document.getElementById("TextBox1").value;
        //                    var to = document.getElementById("TextBox2").value;
        //                    console.log(`Label: ${label}, From: ${from}, To: ${to}`);
        //                    // Tambahkan logika lain yang Anda butuhkan
        //                }
        //            }
        //        }
        //    });
        //}

        //function fillChart3(labels, jumlah) {
        //    var ctx3 = document.getElementById("chart-pie").getContext("2d");
        //    var backgroundColors = labels.map(() => getRandomColor());
        //    var myChart = new Chart(ctx3, {
        //        type: 'pie',
        //        data: {
        //            labels: labels,
        //            datasets: [{
        //                label: 'Summary Detection',
        //                data: jumlah,
        //                backgroundColor: backgroundColors,  // Set warna background
        //                hoverOffset: 4
        //            }]
        //        }
        //    });
        //}
        function fillChart3(labels, jumlah) {
            var ctx3 = document.getElementById("chart-pie").getContext("2d");
            var backgroundColors = labels.map(() => getRandomColor());

            var myChart = new Chart(ctx3, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Summary Detection',
                        data: jumlah,
                        backgroundColor: backgroundColors,
                        hoverOffset: 4
                    }]
                },
                options: {
                    plugins: {
                        datalabels: {
                            color: '#fff', // Warna teks
                            formatter: function (value, context) {
                                return value === 0 ? null : value; // Jika nilai 0, tidak menampilkan label
                            },
                            font: {
                                weight: 'bold',
                                size: 14
                            }
                        }
                    }
                },
                plugins: [ChartDataLabels] // Aktifkan plugin
            });
        }
        function Summary(card) {
            window.open('card_detail.aspx?card=' + card, '_blank');
        }

    </script>
    

</asp:Content>

