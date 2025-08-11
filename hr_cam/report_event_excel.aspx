<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_event_excel.aspx.cs" Inherits="hr_cam.Report_event_excel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <title></title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.0/xlsx.full.min.js"></script>
    <script src="bootstrap2/js/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%" aria-describedby="report">
                <tr>
                    <th align="left" style="width: 50%;"><img src="bootstrap/logo.png" id="logonya" runat="server" class="img-fluid" alt=""/></th>
                    <th align="right" style="width: 50%;"><img src="bootstrap/logo.png" id="logo2" runat="server" class="img-fluid" alt=""/></th>
                </tr>
                <tr>
                    <%--<td stye="width:155px;"><img id="logonya" runat="server" class="img-fluid" alt=""></td>--%>
                    <td align="center" colspan="2">
                        <b style="font-size:20px">Report Event History</b>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <b style="font-size: 11px;">Periode <asp:Label ID="Label1" runat="server" Text=""></asp:Label> to <asp:Label ID="Label2" runat="server" Text=""></asp:Label></b>
                    </td>
                </tr>
            </table>
            <br/>
            <asp:Button ID="btnExportExcel" CssClass="btn btn-warning" runat="server" Text="Export Excel 2" OnClick="btnExportExcel_Click" />
                <table id="myTable" class="table" style="width: 100%;" border="1" id="isitabel" aria-describedby="event_list">
                    <thead>
                        <tr>
                            <th>No</th>
                            <th colspan="2">Image</th>
                            <th>Occurred At</th>
                            <th>Location</th>
                            <th>Site</th>
                            <th>Person</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody runat="server" id="TableBody">
                        <tr>
                            <td>satu</td>
                            <td>dua</td>
                            <td>tiga</td>
                            <td>empat</td>
                            <td>lima</td>
                            <td>enam</td>
                            <td>tujuh</td>
                            <td>delapan</td>
                        </tr>
                    </tbody>
                </table>
        </div>
    </form>
</body>
</html>
<script>
    $(document).ready(function () {
        //alert("ada ko");
        //const table = document.getElementById('myTable');
        //const workbook = XLSX.utils.table_to_book(table, { sheet: "Sheet1" });
        //XLSX.writeFile(workbook, 'data.xlsx');
    });
    //$(document).ready(function () {
    //    console.log("ready!");
    //});
</script>


