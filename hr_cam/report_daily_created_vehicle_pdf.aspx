<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report_daily_created_vehicle_pdf.aspx.cs" Inherits="hr_cam.report_daily_created_vehicle_pdf" %>

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
                        <b style="font-size:20px">Report Daily Created Vehicle</b>
                    </td>
                </tr>
            </table>
            <br/>
            <div class="table-responsive">
                <table class="table" style="width: 100%;" border="1" id="isitabel" aria-describedby="vehicle_list">
                    <thead>
                        <tr>
                            <th>No</th>
                            <th>License Plate Number</th>
                            <th>Type</th>
                            <th>Created At</th>
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

