const char* CONFIG_HTML = R"html-code(
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Device Configuration</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 600px;
            margin: 40px auto;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .container {
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #333;
            margin-bottom: 30px;
            text-align: center;
        }
        .form-group {
            margin-bottom: 20px;
        }
        label {
            display: block;
            margin-bottom: 5px;
            color: #555;
            font-weight: bold;
        }
        input[type="text"],
        input[type="password"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        button {
            background-color: #007bff;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
            font-size: 16px;
            margin-bottom: 10px;
        }
        button:hover {
            background-color: #0056b3;
        }
        .reset-button {
            background-color: #dc3545;
            margin-top: 20px;
        }
        .reset-button:hover {
            background-color: #c82333;
        }
        .password-toggle {
            margin-top: 8px;
        }
        .password-toggle label {
            display: inline;
            font-weight: normal;
            margin-left: 5px;
        }
        .status {
            margin-top: 20px;
            padding: 10px;
            border-radius: 4px;
            display: none;
        }
        .success {
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        .error {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Device Configuration</h1>
        <form id="configForm" action="/api/config" method="post">
            <div class="form-group">
                <label for="device_name">Device Name:</label>
                <input type="text" id="device_name" name="device_name" required
                    placeholder="Enter device name">
            </div>
            
            <div class="form-group">
                <label for="wifi_ssid">WiFi SSID:</label>
                <input type="text" id="wifi_ssid" name="wifi_ssid" required
                    placeholder="Enter WiFi network name">
            </div>
            
            <div class="form-group">
                <label for="wifi_password">WiFi Password:</label>
                <input type="password" id="wifi_password" name="wifi_password" required
                    placeholder="Enter WiFi password">
                <div class="password-toggle">
                    <input type="checkbox" id="showPassword" onchange="togglePassword()">
                    <label for="showPassword">Show password</label>
                </div>
            </div>
            
            <button type="submit">Save Configuration</button>
        </form>
        <button onclick="resetDevice()" class="reset-button">Reset Device</button>
        <div id="status" class="status"></div>
    </div>

    <script>
        // Load current configuration when page loads
        window.addEventListener('load', loadConfig);

        function togglePassword() {
            const passwordInput = document.getElementById('wifi_password');
            const showPassword = document.getElementById('showPassword');
            passwordInput.type = showPassword.checked ? 'text' : 'password';
        }

        function showStatus(message, isError = false) {
            const statusDiv = document.getElementById('status');
            statusDiv.textContent = message;
            statusDiv.style.display = 'block';
            statusDiv.className = `status ${isError ? 'error' : 'success'}`;
            setTimeout(() => {
                statusDiv.style.display = 'none';
            }, 5000);
        }

        async function loadConfig() {
            try {
                const response = await fetch('/api/config');
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const config = await response.json();
                
                document.getElementById('device_name').value = config.device_name || '';
                document.getElementById('wifi_ssid').value = config.wifi_ssid || '';
                document.getElementById('wifi_password').value = config.wifi_password || '';
                
                showStatus('Configuration loaded successfully');
            } catch (error) {
                console.error('Error loading configuration:', error);
                showStatus('Failed to load configuration: ' + error.message, true);
            }
        }

        async function resetDevice() {
            if (confirm('Are you sure you want to reset the device? This will erase all setting and configurations.')) {
                try {
                    const response = await fetch('/api/reset', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    });
                    
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    
                    showStatus('Device reset successfully. Reboot to apply changes');
                    // Clear the form after successful reset
                    document.getElementById('configForm').reset();
                } catch (error) {
                    console.error('Error resetting device:', error);
                    showStatus('Failed to reset device: ' + error.message, true);
                }
            }
        }
    </script>
</body>
</html>
)html-code";

const char* APPLY_HTML = R"html-code(
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Configuration Saved</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 600px;
            margin: 40px auto;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .container {
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        .success-icon {
            color: #28a745;
            font-size: 48px;
            margin-bottom: 20px;
        }
        h1 {
            color: #333;
            margin-bottom: 20px;
        }
        .message {
            color: #666;
            margin-bottom: 30px;
            line-height: 1.5;
        }
        .button-group {
            display: flex;
            gap: 15px;
            justify-content: center;
        }
        button {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.2s;
        }
        .primary-button {
            background-color: #007bff;
            color: white;
        }
        .primary-button:hover {
            background-color: #0056b3;
        }
        .secondary-button {
            background-color: #6c757d;
            color: white;
        }
        .secondary-button:hover {
            background-color: #545b62;
        }
        .status {
            margin-top: 20px;
            padding: 10px;
            border-radius: 4px;
            display: none;
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="success-icon">✔</div>
        <h1>Configuration Saved</h1>
        <p class="message">
            Your device configuration has been saved successfully. 
            To apply the new settings, click the button below to reboot the device.
            The device will restart and connect using the new configuration.
        </p>
        <div class="button-group">
            <button class="secondary-button" onclick="window.location.href='/'">Back to Settings</button>
            <button class="primary-button" onclick="rebootDevice()">Apply and Reboot</button>
        </div>
        <div id="status" class="status"></div>
    </div>

    <script>
        function showStatus(message) {
            const statusDiv = document.getElementById('status');
            statusDiv.textContent = message;
            statusDiv.style.display = 'block';
        }

        async function rebootDevice() {
            try {
                const response = await fetch('/api/reboot', {
                    method: 'POST'
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                // Disable buttons after successful reboot request
                document.querySelectorAll('button').forEach(button => {
                    button.disabled = true;
                });

                // Update message to show reboot in progress
                document.querySelector('.message').textContent = 
                    'Device is rebooting... Please wait while the device restarts with the new configuration.';

            } catch (error) {
                console.error('Error initiating reboot:', error);
                showStatus('Failed to initiate reboot: ' + error.message);
            }
        }
    </script>
</body>
</html>

)html-code";