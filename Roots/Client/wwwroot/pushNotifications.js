function requestNotificationPermission() {
    return Notification.requestPermission().then(permission => {
        return permission;
    });
}