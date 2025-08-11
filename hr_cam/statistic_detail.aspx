<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="statistic_detail.aspx.cs" Inherits="hr_cam.statistic_detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
  <div class="row">
    <div class="col-12">
      <div class="card my-4">
        <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
          <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
            <h5 class="text-white text-capitalize ps-3"><asp:Label ID="Judul" runat="server" CssClass="text-white text-capitalize ps-3"></asp:Label></h5>
          </div>
        </div>

        <div class="card-body px-0 pb-2">
          <div class="table-responsive p-0" style="margin:5px;">
              <table id="DataTable" class="table table-striped table-bordered" aria-describedby="vms_list">
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
    <script type="text/javascript">
    $(document).ready(function () {
        //$('#table_users').DataTable();
        //$('#Table1').DataTable();
        $('#DataTable').DataTable();
    });
    </script>
</asp:Content>
