namespace WebApp.Helpers;

public static class AuthorizationHelper
{
    public static bool IsAdminOrInstructor(ISession session)
    {
        var role = session.GetString("Role");
        return role == "Admin" || role == "Instructor" || role == "Teacher";
    }

    public static bool IsAdmin(ISession session)
    {
        var role = session.GetString("Role");
        return role == "Admin";
    }

    public static bool IsAuthenticated(ISession session)
    {
        var isAuthenticated = session.GetString("IsAuthenticated");
        return isAuthenticated == "true";
    }
}

