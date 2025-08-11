<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="report_dpo.aspx.cs" Inherits="hr_cam.Report_dpo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
      <div class="row">
        <div class="col-12">
          <div class="card my-4">
            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
              <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
                <h5 class="text-white text-capitalize ps-3">Report Face Recognition DPO</h5>
              </div>
            </div>

            <div class="card-body px-0 pb-2">
                <div class="row mb-5">
                    <div class="col-md-6">
                                <table style="width: 100%;margin:10px;" aria-describedby="filter">
                                    <tr>
                                        <th>Camera</th>
                                        <th>From</th>
                                        <th></th>
                                        <th>To</th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                    </tr>
                                    <tr>
                                        <%--<td><asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList></td>
                                        <td><asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"></asp:DropDownList></td>--%>
                                        <td><select id="SelectMultiple" runat="server" class="js-example-basic-multiple" name="camera[]" multiple="true"></select></td>
              <%--                          <td><select class="js-example-basic-multiple" name="states[]" multiple="multiple">
  <option value="AL">Alabama</option>
  <option value="WY">Wyoming</option>
</select></td>--%>
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>

                                        </td>
                                        <td>-</td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Filter" OnClick="Button1_Click" />
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="Button2" CssClass="btn btn-danger" runat="server" Text="Export PDF" OnClick="Button2_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExportExcel" CssClass="btn btn-warning" runat="server" Text="Export CSV" OnClick="btnExportExcel_Click" />
                                        </td>
                                    </tr>
                                </table>
                    </div>

                </div>
              <div class="table-responsive p-0" style="margin:5px;">
                  <table id="DataTable1" class="table table-striped table-bordered" aria-describedby="dpo_list">
                      <thead>
                        <tr>
                            <th>Image</th>
                            <th></th>
                            <th>Occurred At</th>
                            <th>Location</th>
                            <th>Site</th>
                            <th>Person</th>
                            <%--<th>Entry Code</th>--%>
                            <th>Status
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
        $('#DataTable1').DataTable();
        $('.js-example-basic-multiple').select2();
        //$('#SelectMultiple').select2();
    });
</script>    
</asp:Content>
