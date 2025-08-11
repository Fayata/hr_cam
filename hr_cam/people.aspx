<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="people.aspx.cs" Inherits="hr_cam.people" %>
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
                <h5 class="text-white text-capitalize ps-3">People</h5>
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
                    <div class="hide">
                        <input type="file" id="image-file" style="display:none;"/>
                    </div>
                    <div class="hide">
                        <input type="file" id="image-files" multiple style="display:none;"/>
                    </div>
                    <div class="col-5" style="flex: 0 0 auto;">
                        
                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" ClientIDMode="Static" style="display:none;"></asp:TextBox>
                        <table>
                            <tr>
                                <th style="width: 120px;"><input type="text" id="limit" class="form-control" placeholder="Limit Sync" /></th>
                                <th><a href="#" onclick="syncVMS()" class="btn btn-warning"><span id="badgeSpan" class="badge text-bg-danger" style="border-radius:5px;"></span> Sync to VMS</a></th>
                                <th><a href="#" onclick="selectImages()" class="btn btn-primary">Upload Image</a></th>
                                <th><a href="#" onclick="selectCSV()" class="btn btn-success">Import CSV</a></th>
                                </tr>

                        </table>

                    </div>
                </div>
              <div class="table-responsive p-0" style="margin:5px;">
                  <table id="DataTable" class="table table-striped table-bordered"  aria-describedby="employee_list">
                      <thead>
                        <tr>
                            <th>Identification Number</th>
                            <th>Name</th>
                            <th>Gender</th>
                            <th>Type</th>
                            <th></th>
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
        function getBasicAuth() {
            return '<%= Session["BasicAuth"] %>';
        }
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
    var url_api;
    $(document).ready(function () {
        //alert(getBasicAuth());
        url_api = document.getElementById("url_api").value;
        
        var sync = document.getElementById("TextBox1").value;
        //alert(sync);
        document.getElementById("badgeSpan").innerHTML = sync;
        //$('#table_users').DataTable();
        //$('#Table1').DataTable();
        $('#DataTable').DataTable();
    });

    //document.getElementById('uploadLink').onclick = function () {
    //    var input = document.createElement('input');
    //    input.type = 'file';
    //    input.onchange = function (event) {
    //        var file = event.target.files[0];
    //        if (file) {
    //            uploadFile(file);
    //        }
    //    };
    //    input.click();
    //};

    //function uploadFile(file) {
    //    var xhr = new XMLHttpRequest();
    //    var formData = new FormData();
    //    formData.append('file', file);

    //    xhr.open('POST', 'http://localhost:3000/employee/import', true);
    //    xhr.onload = function () {
    //        if (xhr.status === 200) {
    //            // Berhasil
    //            console.log('File berhasil diunggah');
    //        } else {
    //            // Gagal
    //            console.error('Gagal mengunggah file');
    //        }
    //    };
    //    xhr.onerror = function () {
    //        console.error('Kesalahan koneksi');
    //    };
    //    xhr.send(formData);
    //}
    //function uploadFile(file) {
    //    var formData = new FormData();
    //    formData.append('file', file);

    //    fetch('http://localhost:3000/employee/import', {
    //        method: 'POST',
    //        body: formData
    //    })
    //        .then(response => {
    //            if (!response.ok) {
    //                throw new Error('Gagal mengunggah file');
    //            }
    //            console.log('File berhasil diunggah');
    //        })
    //        .catch(error => {
    //            console.error(error);
    //        });
    //}

    function kirimFileKeAPI() {
        var fileInput = document.getElementById('fileInput'); // Ambil input file dari HTML
        var file = fileInput.files[0]; // Ambil file dari input

        var formData = new FormData();
        formData.append('file', file); // Tambahkan file ke FormData

        // Buat permintaan HTTP POST
        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:3000/persons/import', true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                // Berhasil
                console.log('File berhasil dikirim ke API');
            } else {
                // Gagal
                console.error('Gagal mengirim file ke API');
            }
        };
        xhr.send(formData); // Kirim FormData ke API
    }
</script>    
<script type="text/javascript">
    function uploadFile() {
        document.getElementById('fileInput').click();
    }

    function handleFileUpload(event) {
        var file = event.target.files[0];
        if (file) {
            var formData = new FormData();
            formData.append('file', file);  // Ensure the key is 'file'

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'http://localhost:3000/persons/import', true);  // Ensure POST method
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        console.log('File uploaded successfully!');
                        console.log('Response:', xhr.responseText);
                    } else {
                        console.error('Error uploading file:', xhr.responseText);
                    }
                }
            };
            xhr.send(formData);
        }
    }
</script>
    <script>
        function selectCSV() {
            const csvInput = document.getElementById("csv-file");
            //const auth = getBasicAuth();
            const url = url_api + "v1/persons/import";
            csvInput.onchange = async function () {
                const data = new FormData();
                data.append("file", this.files[0]);
                var response;
                //try {
                //    response = await fetch("/employees", {
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
                //fetch('http://localhost:3000/v1/persons/import', {
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
                            console.log(error)
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

        function syncVMS() {
            var limit = document.getElementById("limit").value;
            //alert(limit);
            const data = new FormData();
            const url = url_api + "v1/persons/sync";
            //const auth = getBasicAuth();
            data.append("limit", limit);
                //fetch('http://localhost:3000/v1/persons/sync', {
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
                        alert("Successfully synced with VMS");
                        document.getElementById("limit").value = "";
                        location.reload();
                    })
                    .catch(error => {
                        console.error(error);
                    });

        }
        function selectImages() {
            const imagesInput = document.getElementById("image-files");
            const url = url_api + "v1/persons/import/images";
            //const auth = getBasicAuth();
            imagesInput.onchange = async function () {
                const data = new FormData();
                for (let i = 0; i < this.files.length; i++) {
                    data.append("images", this.files[i]);
                    //alert(this.files[i]);
                }
                var response;
                //fetch('http://localhost:3000/v1/persons/import/images', {
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
                //try {
                //    response = await fetch("/employees/images", {
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

                //if (response.ok) {
                //    location.reload();
                //} else {
                //    const result = await response.json();
                //    alert(JSON.stringify(result.message));
                //}
            };
            imagesInput.click();
        }

        function selectImage(personId) {
            const imageInput = document.getElementById("image-file");
            const url = url_api + "v1/persons/" + personId + "/import/image";
            //const auth = getBasicAuth();
            imageInput.onchange = async function () {
                const data = new FormData();
                data.append("image", this.files[0]);
                //alert(this.files[0]);
                var response;
                //fetch('http://localhost:3000/v1/persons/'+personId+'/import/image', {
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
                //try {
                //    response = await fetch(`/employees/${personId}/image`, {
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

                //if (response.ok) {
                //    location.reload();
                //} else {
                //    const result = await response.json();
                //    alert(JSON.stringify(result.message));
                //}
            };
            imageInput.click();
        }

        function syncSingle(personId) {
            var response;
            const url = url_api + "v1/persons/" + personId + "/sync";
            //const auth = getBasicAuth();
                //fetch('http://localhost:3000/v1/persons/' + personId + '/sync', {
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
                        alert("Successfully synced with VMS");
                        location.reload();
                    })
                    .catch(error => {
                        console.error(error);
                    });
        }

        
    </script>
    <script>
        function confirmDelete(event, id) {
            event.preventDefault(); // Mencegah aksi default dari link
            //const auth = getBasicAuth();
            const url = url_api + "v1/persons/${id}";
            if (confirm('Are you sure you want to delete this item?')) {
                //fetch(`http://localhost:3000/v1/persons/${id}`, {
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

