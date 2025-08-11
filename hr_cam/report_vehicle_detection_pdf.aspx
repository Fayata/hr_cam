<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_vehicle_detection_pdf.aspx.cs" Inherits="hr_cam.Report_vehicle_detection_pdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <title></title>
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
                        <b style="font-size:20px">Report License Plate Recognition</b>
                        <%--<b style="font-size:20px">Report Vehicle Detection</b>--%>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <b style="font-size: 11px;">Periode <asp:Label ID="Label1" runat="server" Text=""></asp:Label> to <asp:Label ID="Label2" runat="server" Text=""></asp:Label></b>
                    </td>
                </tr>
            </table>
            <br/>
            <div class="table-responsive">
                <table class="table" style="width: 100%;" border="1" id="isitabel" aria-describedby="vehicle_list">
                    <thead>
                        <tr>
                            <th>No</th>
                            <th>Image</th>
                            <th></th>
                            <th>License Plate Number</th>
                            <th>Occurred At</th>
                            <th>Location</th>
                            <th>Site</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody runat="server" id="TableBody">
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>

