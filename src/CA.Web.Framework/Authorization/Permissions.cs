using System.Collections.Generic;

namespace CA.Web.Framework.Authorization
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static class Posts
        {
            public const string View = "Permissions.Postss.View";
            public const string Create = "Permissions.Posts.Create";
            public const string Edit = "Permissions.Posts.Edit";
            public const string Delete = "Permissions.Posts.Delete";
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
            public const string Create = "Permissions.Dashboards.Create";
            public const string Edit = "Permissions.Dashboards.Edit";
            public const string Delete = "Permissions.Dashboards.Delete";
        }

       

        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
        }

        public static class Tags
        {
            public const string View = "Permissions.Tags.View";
            public const string Create = "Permissions.Tags.Create";
            public const string Edit = "Permissions.Tags.Edit";
            public const string Delete = "Permissions.Tags.Delete";
        }

        public static class Comments
        {
            public const string View = "Permissions.Comments.View";
            public const string Create = "Permissions.Comments.Create";
            public const string Edit = "Permissions.Comments.Edit";
            public const string Delete = "Permissions.Comments.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

    }
}
