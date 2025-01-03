namespace TimeSwap.Shared.Constants
{
    public enum Role
    {
        Admin,
        User
    }

    public enum SubscriptionPlan
    {
        Basic,
        Standard,
        Premium
    }

    public enum PermissionPolicy
    {
        FullControl,
        View,
        ViewAndEdit
    }

    public enum PostedDate
    {
        Today,
        Yesterday,
        Last7Days,
        Last30Days
    }
}
