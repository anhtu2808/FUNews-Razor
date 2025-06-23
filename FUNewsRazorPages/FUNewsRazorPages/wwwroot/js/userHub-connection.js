// Global UserHub Connection Manager
class UserHubManager {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.userEmail = null;
    }

    async connect(email) {
        if (this.isConnected || !email) {
            console.log("UserHub: Skipping connection - already connected or no email");
            return;
        }

        console.log("UserHub: Attempting to connect with email:", email);
        this.userEmail = email;
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`/userHub?email=${encodeURIComponent(email)}`)
            .configureLogging(signalR.LogLevel.Information)
            .build();

        try {
            await this.connection.start();
            this.isConnected = true;
            console.log("✅ UserHub: Successfully connected for email:", email);
            
            // Handle disconnection
            this.connection.onclose(() => {
                this.isConnected = false;
                console.log("❌ UserHub: Connection closed for email:", email);
            });

        } catch (err) {
            console.error("❌ UserHub: Failed to connect:", err.toString());
            console.error("Email:", email);
            console.error("URL:", `/userHub?email=${encodeURIComponent(email)}`);
        }
    }

    async disconnect() {
        if (this.connection && this.isConnected) {
            try {
                await this.connection.stop();
                this.isConnected = false;
                console.log("Disconnected from UserHub");
            } catch (err) {
                console.error("Error disconnecting from UserHub:", err.toString());
            }
        }
    }

    onUserStatusChanged(callback) {
        if (this.connection) {
            this.connection.on("UserStatusChanged", callback);
        }
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