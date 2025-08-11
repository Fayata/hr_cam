<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="user_admin.aspx.cs" Inherits="hr_cam.user_admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .success-message {
    background-color: #DFF0D8; /* Warna background untuk pesan sukses */
    /* Tambahan gaya sesuai kebutuhan */
}

    .error-message {
    background-color: #F2DEDE; /* Warna background untuk pesan gagal */
    /* Tambahan gaya sesuai kebutuhan */
    }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
      <div class="row">
        <div class="col-12">
          <div class="card my-4">
            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
              <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
                <h5 class="text-white text-capitalize ps-3">Users Admin</h5>
              </div>
            </div>
            <div class="row">
                <div class="col-lg-6">
                    <div id="successMessageDiv" runat="server" class="alert alert-success alert-dismissible fade show" role="alert" style="margin: 15px; color: white; display: none;">
                        <asp:Literal ID="successMessage" runat="server"></asp:Literal>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                    <div id="failMessageDiv" runat="server" class="alert alert-danger alert-dismissible fade show" role="alert" style="margin: 15px; color: white; display: none;">
                        <asp:Literal ID="failMessage" runat="server"></asp:Literal>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                    <div id="updateMessageDiv" runat="server" class="alert alert-warning alert-dismissible fade show" role="alert" style="margin: 15px; color: white; display: none;">
                        <asp:Literal ID="updateMessage" runat="server"></asp:Literal>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                </div>
            </div>

            <div class="card-body px-0 pb-2">
                <a href="add_user_admin.aspx" class="btn" style="background-color: #0d6efd;color:white;margin:15px;">Add User Admin</a>
                <%--<span onclick="generate_config()" class="btn btn-success" style="margin-top:15px;">Generate Collector Config</span>--%>
              <div class="table-responsive p-0" style="margin:5px;">
                  <%--<asp:Table class="table table-striped table-bordered" ID="Table1" runat="server">
                      <asp:TableRow>
                        <asp:TableCell>
                            Row 0, Col 0
                        </asp:TableCell>
                        <asp:TableCell>
                            Row 0, Col 1
                        </asp:TableCell>
                    </asp:TableRow>
                  </asp:Table>--%>
                  <%--<table id="DataTable2" class="table table-striped table-bordered">
                    <thead>
                      <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Created At</th>
                        <th colspan="2">Action</th>
                      </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>2</td>
                            <td>3</td>
                            <td>4</td>
                            <td>5</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>2</td>
                            <td>3</td>
                            <td>4</td>
                            <td>5</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>2</td>
                            <td>3</td>
                            <td>4</td>
                            <td>5</td>
                        </tr>
                    </tbody>
                  </table>--%>
                  <table id="DataTable" class="table table-striped table-bordered" aria-describedby="user_list">
                      <thead>
                        <tr>
                          <th>Name</th>
                          <th>Email</th>
                          <th>Created At</th>
                          <th style="width: 10%;">Action</th>
                          <%--<th>Action</th>--%>
                        </tr>
                      </thead>
                      <tbody runat="server" id="TableBody">
                      </tbody>
                    </table>

                <%--<table class="table align-items-center mb-0" id="table_users">
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Email</th>
                      <th>Created At</th>
                      <th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                  </tbody>
                </table>--%>
                <%--<asp:GridView ID="table_users" runat="server" CssClass="table align-items-center mb-0">
                    <Columns>
                        <asp:HyperLinkField DataTextField="Action" DataNavigateUrlFields="id" DataNavigateUrlFormatString="edit_user.aspx?id={0}" Text="Edit" />
                    </Columns>
                </asp:GridView>--%>                  
                  <asp:GridView class="table table-striped table-bordered" ID="table_users" runat="server">
                      <%--<Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Created At" HeaderText="Created At" />
                        <asp:BoundField DataField="Action" HeaderText="Action" HtmlEncode="false" />
                    </Columns>--%>
                      <%--<Columns></Columns>--%>
                        <%--<Columns>
                            <asp:BoundField datafield="name" headertext="name" />
                            <asp:boundfield datafield="email" headertext="email" />
                            <asp:boundfield datafield="created at" headertext="created at" />
                            <asp:templatefield headertext="action">
                                <itemtemplate>
                                    <a href='<%# "edit_user.aspx?id=" + eval("id") %>'>edit</a>
                                </itemtemplate>
                            </asp:templatefield>
                        </Columns>--%>
                    </asp:GridView>
                  <%--<asp:GridView ID="table_users" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Created At" HeaderText="Created At" />
                        <asp:BoundField DataField="Action" HeaderText="Action" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>--%>

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
        //$('#table_users').DataTable();
        //$('#Table1').DataTable();
        $('#DataTable').DataTable();
    });

    
</script>
    <script>
        function generate_config() {
            alert("masuk api");
        }
    </script>
</asp:Content>

