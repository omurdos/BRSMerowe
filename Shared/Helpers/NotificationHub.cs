namespace BRSSinnar.Dashboard.Helpers
{
    using Microsoft.AspNetCore.SignalR;

   public class NotificationHub : Hub
{
    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
    }
}
}