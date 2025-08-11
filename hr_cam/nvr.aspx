<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="nvr.aspx.cs" Inherits="hr_cam.Nvr" %>
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
                <h5 class="text-white text-capitalize ps-3">Network Video Records</h5>
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
                <a href="add_nvr.aspx" class="btn" style="background-color: #0d6efd;color:white;margin:15px;">Add Network Video Recorder</a>
                <%--<div class="row justify-content-left mb-5">--%>
                    <%--<div class="hide">
                        <input type="file" id="csv-file" style="display:none;"/>
                    </div>
                    <div class="col-3" style="flex: 0 0 auto;width: 12%;">
                        <a href="#" onclick="selectCSV()" class="btn btn-success">Import CSV</a>
                    </div>--%>
                    <%--<div class="col-3" style="flex: 0 0 auto;width: 12%;">
                    </div>--%>
                <%--</div>--%>
              <div class="table-responsive p-0" style="margin:5px;">
                  <table id="DataTable" class="table table-striped table-bordered" aria-describedby="nvr_list">
                      <thead>
                        <tr>
                            <th>URL / IP Address</th>
                            <th>Username</th>
                            <th style="width: 10%;">Action</th>
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
        //$('#table_users').DataTable();
        //$('#Table1').DataTable();
        $('#DataTable').DataTable();
    });
</script>
    <script>
        function selectCSV() {
            const csvInput = document.getElementById("csv-file");
            csvInput.onchange = async function () {
                const data = new FormData();
                data.append("file", this.files[0]);
                var response;
                //try {
                //    response = await fetch("/blacklists", {
                //        headers: {
                //            "X-CSRF-Token": csrfToken,
                //            Accept: "application/json",
                //        },
                //        method: "POST",
                //        body: data,
                //    });
                //} catch (error) {
                //    console.log(error);
                //}
                fetch('http://localhost:3000/v1/nvrs/import', {
                    method: 'POST',
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

                //if (response.ok) {
                //    location.reload();
                //} else {
                //    const result = await response.json();
                //    alert(JSON.stringify(result.message));
                //}
            };
            csvInput.click();
        }
        function confirmDelete(event, id) {
            event.preventDefault(); // Mencegah aksi default dari link

            if (confirm('Are you sure you want to delete this item?')) {
                fetch(`http://localhost:3000/v1/nvrs/${id}`, {
                    method: 'DELETE',
                    headers: {
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

