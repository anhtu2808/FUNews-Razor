// Global UserHub Connection Manager
class UserHubManager {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.userEmail = null;
        this.reconnectAttempts = 0;
        this.maxReconnectAttempts = 5;
    }

    async connect(email) {
        if (this.isConnected || !email) {
            console.log("UserHub: Skipping connection - already connected or no email");
            return;
        }

        console.log("UserHub: Attempting to connect with email:", email);
        console.log("UserHub: Current host:", window.location.host);
        console.log("UserHub: Current protocol:", window.location.protocol);
        
        this.userEmail = email;
        
        // Build connection URL - use current host for network compatibility
        const hubUrl = `/userHub?email=${encodeURIComponent(email)}`;
        console.log("UserHub: Connection URL:", hubUrl);
        
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl, {
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents | signalR.HttpTransportType.LongPolling
            })
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    if (retryContext.previousRetryCount < this.maxReconnectAttempts) {
                        return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
                    } else {
                        return null; // Stop retrying
                    }
                }
            })
            .build();

        try {
            await this.connection.start();
            this.isConnected = true;
            this.reconnectAttempts = 0;
            console.log("✅ UserHub: Successfully connected for email:", email);
            console.log("✅ UserHub: Connection state:", this.connection.state);
            console.log("✅ UserHub: Transport:", this.connection.transport?.name);
            
            // Handle disconnection
            this.connection.onclose((error) => {
                this.isConnected = false;
                console.log("❌ UserHub: Connection closed for email:", email);
                if (error) {
                    console.error("❌ UserHub: Connection closed with error:", error);
                }
            });

            // Handle reconnecting
            this.connection.onreconnecting((error) => {
                this.isConnected = false;
                this.reconnectAttempts++;
                console.log(`🔄 UserHub: Reconnecting attempt ${this.reconnectAttempts}...`);
                if (error) {
                    console.error("🔄 UserHub: Reconnecting due to error:", error);
                }
            });

            // Handle reconnected
            this.connection.onreconnected((connectionId) => {
                this.isConnected = true;
                console.log("🔄 UserHub: Reconnected successfully with ID:", connectionId);
            });

        } catch (err) {
            console.error("❌ UserHub: Failed to connect:", err.toString());
            console.error("❌ UserHub: Email:", email);
            console.error("❌ UserHub: URL:", hubUrl);
            console.error("❌ UserHub: Current host:", window.location.host);
            console.error("❌ UserHub: User agent:", navigator.userAgent);
            
            // Try to get more network info
            this.logNetworkInfo();
        }
    }

    async disconnect() {
        if (this.connection && this.isConnected) {
            try {
                await this.connection.stop();
                this.isConnected = false;
                console.log("UserHub: Disconnected successfully");
            } catch (err) {
                console.error("UserHub: Error disconnecting:", err.toString());
            }
        }
    }

    onUserStatusChanged(callback) {
        if (this.connection) {
            this.connection.on("UserStatusChanged", callback);
        }
    }

    // Debug method to log network information
    logNetworkInfo() {
        console.log("=== Network Debug Info ===");
        console.log("Host:", window.location.host);
        console.log("Hostname:", window.location.hostname);
        console.log("Port:", window.location.port);
        console.log("Protocol:", window.location.protocol);
        console.log("Origin:", window.location.origin);
        console.log("User Agent:", navigator.userAgent);
        console.log("Online:", navigator.onLine);
        
        // Try to detect if we're on localhost vs network IP
        if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
            console.log("⚠️  Running on localhost - may have issues with network access");
        } else {
            console.log("✅ Running on network IP - should work across devices");
        }
    }

    // Get connection status
    getStatus() {
        return {
            isConnected: this.isConnected,
            connectionState: this.connection?.state || 'Disconnected',
            userEmail: this.userEmail,
            reconnectAttempts: this.reconnectAttempts,
            transport: this.connection?.transport?.name || 'Unknown'
        };
    }
}

// Global instance
window.userHubManager = new UserHubManager();

// Auto-disconnect on page unload
window.addEventListener('beforeunload', function() {
    if (window.userHubManager) {
        window.userHubManager.disconnect();
    }
});

// Log network info on load for debugging
window.addEventListener('load', function() {
    if (window.userHubManager) {
        window.userHubManager.logNetworkInfo();
    }
});

// Monitor online/offline status
window.addEventListener('online', function() {
    console.log("🌐 Browser is back online");
});

window.addEventListener('offline', function() {
    console.log("📴 Browser is offline");
}); 