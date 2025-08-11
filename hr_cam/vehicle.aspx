<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="vehicle.aspx.cs" Inherits="hr_cam.vehicle" %>
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
                <h5 class="text-white text-capitalize ps-3">License Plate Recognition</h5>
                <%--<h5 class="text-white text-capitalize ps-3">Vehicle</h5>--%>
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
                <div class="row justify-content-right mb-5">
                    <input type="hidden" id="url_api" value="<%: System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] %>"/>
                    <div class="hide">
                        <input type="file" id="csv-file" style="display:none;"/>
                    </div>
                    <div class="col-4" style="flex: 0 0 auto;">
                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" ClientIDMode="Static" style="display:none;"></asp:TextBox>
                        <table>
                            <tr>
                                <th style="width: 120px;"><input type="text" id="limit" class="form-control" placeholder="Limit Sync" /></th>
                                <th><a href="#" onclick="syncNVR()" class="btn btn-warning"><span id="badgeSpan" class="badge text-bg-danger" style="border-radius:5px;"></span> Sync to NVR</a></th>
                                <th><a href="#" onclick="selectCSV()" class="btn btn-success">Import CSV</a></th>
                            </tr>
                            
                        
                        </table>
                    </div>
                </div>
                <div class="row justify-content-right">
                    <div class="col-md-6 justify-content-right">
                        <table class="table table-borderless">
                          <tr>
                              <th>License Plate Number</th>
                              <th>Status</th>
                              <th></th>
                          </tr>
                          <tr>
                              <td><asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                              <td><asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"></asp:DropDownList></td>
                              <td><asp:Button ID="Button3" CssClass="btn btn-success" runat="server" Text="Search" OnClick="Button3_Click" /></td>
                          </tr>
                      </table>
                  </div>
                </div>
              <div class="table-responsive p-0" style="margin:5px;">
                  <table id="DataTable" class="table table-striped table-bordered" aria-describedby="vehicle_list">
                      <thead>
                        <tr>
                            <th>Type</th>
                            <th>Police No</th>
                            <th>Vehicle</th>
                            <th>Owner Name</th>
                            <th>Brand</th>
                            <th>Model</th>
                            <th>Entry Code</th>
                            <th>Last Updated</th>
                            <th style="width: 12%;">Action</th>
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
        function getBasicAuth() {
            return '<%= Session["BasicAuth"] %>';
        }
        function autoRefresh() {
            setTimeout(function () {
                location.reload();
            }, 30000); // 30000 milliseconds = 30 seconds
        }

        // Call autoRefresh when the page is fully loaded
        //window.onload = autoRefresh;
    </script>
<script type="text/javascript">
    var url_api;
    $(document).ready(function () {
        //alert(getBasicAuth());
        url_api = document.getElementById("url_api").value;
        //alert(getBasicAuth())
        //$('#table_users').DataTable();
        //$('#Table1').DataTable();
        var sync = document.getElementById("TextBox1").value;
        //alert(sync);
        document.getElementById("badgeSpan").innerHTML = sync;
        $('#DataTable').DataTable({
            searching: false // Disable search box
        });
    });
</script>
    <script>

        //function syncNVR() {
        //    var limit = document.getElementById("limit").value;
        //    const data = new FormData();
        //    const auth = getBasicAuth();
        //    data.append("limit", limit);
        //    fetch('http://localhost:3000/v1/vehicles/sync', {
        //        method: 'POST',
        //        headers: {
        //            'Authorization': auth,
        //            //'Content-Type': 'application/json' // Sesuaikan dengan tipe konten jika diperlukan
        //        },
        //        body: data
        //    })
        //        .then(response => {
        //            if (!response.ok) {
        //                throw new Error('Gagal mengunggah file');
        //            }
        //            console.log('File berhasil diunggah');
        //            alert("Successfully synced with NVR");
        //            document.getElementById("limit").value="";
        //            location.reload();
        //        })
        //        .catch(error => {
        //            console.error(error);
        //        });

        //}

        function syncNVR() {
            var limit = document.getElementById("limit").value;
            //alert(limit);
            const data = new FormData();
            const url = url_api + "v1/vehicles/sync";
            //const auth = getBasicAuth();
            data.append("limit", limit);
            //fetch('http://localhost:3000/v1/vehicles/sync', {
            fetch(url, {
                method: 'POST',
                //headers: {
                //    'Authorization': auth,
                //    //'Content-Type': 'application/json' // Sesuaikan dengan tipe konten jika diperlukan
                //},
                body: data
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Gagal mengunggah file');
                    }
                    console.log('File berhasil diunggah');
                    alert("Successfully synced with NVR-");
                    document.getElementById("limit").value = "";
                    location.reload();
                })
                .catch(error => {
                    console.error(error);
                });

        }

        function selectCSV() {
            const csvInput = document.getElementById("csv-file");
            //const auth = getBasicAuth();
            const url = url_api + "v1/vehicles/import";
            csvInput.onchange = async function () {
                const data = new FormData();
                data.append("file", this.files[0]);
                var response;
                //fetch('http://localhost:3000/v1/vehicles/import', {
                fetch(url, {
                    method: 'POST',
                    //headers: {
                    //    'Authorization': auth,
                    //    //'Content-Type': 'application/json' // Sesuaikan dengan tipe konten jika diperlukan
                    //},
                    body: data
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Gagal mengunggah file');
                        }
                        console.log('File berhasil diunggah');
                        alert("Import File Success");
                        location.reload();
                    })
                    .catch(error => {
                        console.error(error);
                    });
            };
            csvInput.click();
        }
        function syncSingle(vehicleId) {
            var response;
            //const auth = getBasicAuth();
            const url = url_api + "v1/vehicles/" + vehicleId + "/sync";
            //fetch('http://localhost:3000/v1/vehicles/' + vehicleId + '/sync', {
            fetch(url, {
                method: 'POST',
                //headers: {
                //    'Authorization': auth,
                //    //'Content-Type': 'application/json' // Sesuaikan dengan tipe konten jika diperlukan
                //},
                //body: data
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Gagal mengunggah file');
                    }
                    console.log('File berhasil diunggah');
                    alert("Successfully synced with NVR");
                    location.reload();
                })
                .catch(error => {
                    console.error(error);
                });
        }
        function confirmDelete(event, id) {
            event.preventDefault(); // Mencegah aksi default dari link
            //const auth = getBasicAuth();
            const url = url_api + "v1/vehicles/"+id;
            if (confirm('Are you sure you want to delete this item?')) {
                //fetch(`http://localhost:3000/v1/vehicles/${id}`, {
                fetch(url, {
                    method: 'DELETE',
                    headers: {
                        //'Authorization': auth,
                        'Content-Type': 'application/json'
                    }
                })
                    .then(response => {
                        if (response.ok) {
                            alert('Item deleted successfully.');
                            location.reload();
                            // Anda bisa menambahkan logika tambahan di sini, misalnya:
                            // Menghapus elemen dari DOM atau melakukan refresh halaman
                        } else {
                            alert('Failed to delete item.');
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        alert('An error occurred while deleting the item.');
                    });
            }
        }
    </script>
</asp:Content>

