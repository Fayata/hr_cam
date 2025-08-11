<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="list_report.aspx.cs" Inherits="hr_cam.list_report" %>

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
                            <h5 class="text-white text-capitalize ps-3">Create Report</h5>
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
                        <a href="create_report.aspx" class="btn" style="background-color: #0d6efd; color: white; margin: 15px;">Create Report</a>
                        <%--<button onclick="startWebSocket()">Coba Websocket</button>--%>
                        <div class="table-responsive p-0" style="margin: 5px;">
                            <table id="DataTable" class="table table-striped table-bordered" aria-describedby="camera_list">
                                <thead>
                                    <tr>
                                        <th>Filename</th>
                                        <th>Created At</th>
                                        <th style="width:50%;">Queries</th>
                                        <th>Status</th>
                                        <th style="width: 15%;"></th>
                                        <%--<th>Action</th>--%>
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

        function startWebSocket() {
            alert("masuk");
            //var socket = new WebSocket("wss://" + window.location.host + "/WebSocketHandler.ashx");
            var socket = new WebSocket("wss://localhost:44388/WebSocketHandler.ashx");

            socket.onopen = function (event) {
                console.log("WebSocket connection opened.");
                socket.send("Hello Server!");
                sessionStorage.setItem("lastSentMessage", "Hello Server!");  // Simpan di sessionStorage
                //alert("WebSocket connection opened");
            };

            socket.onmessage = function (event) {
                console.log("Message from server: " + event.data);
            };




        }
        window.onload = function () {
            var lastSentMessage = sessionStorage.getItem("lastSentMessage");
            if (lastSentMessage) {
                console.log("Last sent message: " + lastSentMessage);
            }
        };
</script>
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
            var table=$('#DataTable').DataTable({
                ordering: false
            });

            var refreshInterval; // To store interval ID

            function checkCreatedStatus() {
                var hasCreated = false;

                // Loop through the table rows and check if "Created" exists in the Status column (index 1)
                table.rows().every(function () {
                    var data = this.data();
                    if (data[3] === "created") {
                        hasCreated = true;
                    }
                });

                // If "Created" is found, start auto-refresh
                if (hasCreated && !refreshInterval) {
                    refreshInterval = setInterval(function () {
                        location.reload(); // Refresh the page
                    }, 10000); // Refresh every 5 seconds
                }
                // If "Created" is not found, stop auto-refresh
                else if (!hasCreated && refreshInterval) {
                    clearInterval(refreshInterval);
                    refreshInterval = null;
                }
            }

            // Initial check when the page loads
            checkCreatedStatus();

            // Optionally, you can re-check periodically (e.g., every 10 seconds) if data changes
            setInterval(checkCreatedStatus, 10000); // Check every 10 seconds for changes
        });


    </script>
</asp:Content>

