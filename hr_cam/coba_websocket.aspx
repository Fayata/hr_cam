<!DOCTYPE html>
<html>
<head>
    <title>WebSocket Example</title>
</head>
<body>
    <div id="messages"></div>

    <script>
        //var socket = new WebSocket("wss://" + window.location.host + "/WebSocketHandler.ashx");
        var socket = new WebSocket("wss://localhost:44388/WebSocketHandler.ashx");

        socket.onopen = function (event) {
            console.log("WebSocket connection opened.");
        };

        socket.onmessage = function (event) {
            console.log("Message received: " + event.data);
            var messagesDiv = document.getElementById("messages");
            messagesDiv.innerHTML += "<p>" + event.data + "</p>";
        };

        socket.onclose = function (event) {
            console.log("WebSocket connection closed.");
        };

        socket.onerror = function (error) {
            console.log("WebSocket error: " + error);
        };
</script>

</body>
</html>
