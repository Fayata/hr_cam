<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pie_chart.aspx.cs" Inherits="hr_cam.pie_chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <div class="row justify-content-right mb-5">
            <div class="col-md-6">
                <div class="card z-index-2 ">
                        <table style="width: 100%;margin:10px;" aria-describedby="filter">
                            <tr>
                                <th></th>
                                <th>Camera Sites</th>
                                <th>From</th>
                                <th></th>
                                <th>To</th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr>
                                <td><input type="checkbox" name="auto_refresh" id="auto_refresh" checked> Auto Refresh</td> 
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
            <div class="row mt-4">
                <div class="col-lg-6 col-md-10 mt-4 mb-4">
                    <div class="card z-index-2  ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
                                style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%);">
                                <table id="DataTable3" class="table table-striped table-bordered" aria-describedby="list_camera">
                                <thead>
                                    <tr>
                                    <th></th>
                                    <th>Face Recognized</th>
                                    <th>Face Unregistered</th>
                                    <th>License Plate Recognized</th>
                                    <th>License Plate Unregistered</th>
                                    </tr>
                                </thead>
                                <tbody runat="server" id="TbodyPie">
                                </tbody>
                                </table>
                                </div>
                        </div>
                        <div class="card-body">
                            
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-10 mt-4 mb-4">
                    <div class="card z-index-2 ">
                        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2 bg-transparent">
                            <div class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
                                style="background-image: linear-gradient(195deg, #ffffff 0%, #ffffff 100%)">
                                <div class="chart">
                                    <canvas id="chart-pie" class="chart-canvas" height="170"></canvas>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <h6 class="mb-0 ">Jumlah Deteksi Kamera</h6>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function getRandomColor() {
            const letters = '0123456789ABCDEF';
            let color = '#';
            for (let i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
            return color;
        }
        function fillChart3(labels, jumlah) {
            var ctx2 = document.getElementById("chart-pie").getContext("2d");
            var backgroundColors = labels.map(() => getRandomColor());
            var myChart = new Chart(ctx2, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Summary Detection',
                        data: jumlah,
                        backgroundColor: backgroundColors,  // Set warna background
                        hoverOffset: 4
                    }]
                },
                //options: {
                //    scales: {
                //        yAxes: {
                //            //stacked: true,
                //            ticks: {
                //                callback: function (value) {
                //                    return Number.isInteger(value) ? value : '';
                //                }
                //            }
                //        }
                //    },
                //    onClick: function (event, elements) {
                //        if (elements.length > 0) {
                //            var label = this.data.labels[elements[0].index];
                //            var from = document.getElementById("TextBox1").value;
                //            var to = document.getElementById("TextBox2").value;
                //            console.log(`Label: ${label}, From: ${from}, To: ${to}`);
                //            // Tambahkan logika lain yang Anda butuhkan
                //        }
                //    }
                //}
            });
        }
    </script>
</asp:Content>
