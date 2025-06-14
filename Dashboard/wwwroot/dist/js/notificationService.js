class NotificationService {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7179/notificationHub", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.startConnection();
        this.registerHandlers();
    }

    startConnection() {
        this.connection.start()
            .then(() => {
                console.log("SignalR Connected");
                this.connection.invoke("JoinAdminGroup");
            })
            .catch(err => {
                console.error("SignalR Connection Error: ", err);
                setTimeout(() => this.startConnection(), 5000);
            });
    }

    registerHandlers() {
        this.connection.on("ReceiveNotification", (notification) => {
            this.showToast(notification);
            this.updateNotificationBadge();
        });
    }

    showToast(notification) {
        // Implement toast notification using your preferred library
        console.log("New notification:", notification);
        // Example with Toastr:
        console.log(notification.message);
       toastr.info(notification.message, notification.title);
       // toastr.info("Test", "Test");
    }

    updateNotificationBadge() {
        const badge = document.getElementById('notificationBadge');
        if (badge) {
            const current = parseInt(badge.textContent) || 0;
            badge.textContent = current + 1;
            badge.classList.remove('d-none');
        }
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.notificationService = new NotificationService();
});