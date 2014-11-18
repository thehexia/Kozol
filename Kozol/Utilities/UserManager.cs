using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kozol.Models;

namespace Kozol.Utilities {

    #region Status enums as return values.  Positive = successful, negative = not.

    public enum CreateUserStatus : sbyte {
        Success = 1,
        Failure = -1,
        EmailTaken = -2
    }

    public enum ValidateUserStatus : sbyte {
        Success = 1,
        Failure = -1,
        NotFound = -2,
        EmailInvalid = -3,
        PassInvalid = -4,
        UserIdInvalid = -5
    }

    public enum SetLoginStatus : sbyte {
        Success = 1,
        Failure = -1,
        NotFound = -2,
        ValidateFailure = -3
    }

    public enum AssignUserToRoleStatus : sbyte {
        Success = 1,
        RoleAlreadyAssigned = 2,
        Failure = -1,
        UserNotFound = -2,
        RoleNotFound = -3
    }

    public enum RemoveUserFromRoleStatus : sbyte {
        Success = 1,
        RoleNotAssigned = 2,
        Failure = -1,
        UserNotFound = -2,
        RoleNotFound = -3
    }

    public enum EditUserInfoStatus : sbyte {
        Success = 1,
        Failure = -1,
        UserNotFound = -2,
        InvalidFormat = -3
    }

    public enum ChangePasswordStatus : sbyte {
        Success = 1,
        Failure = -1,
        UserNotFound = -2,
        IncorrectPassword = -3,
        InvalidPassword = -4
    }

    public enum LinkFacebookStatus : sbyte {
        Success = 1,
        Failure = -1
    }

    #endregion

    public class UserManager {

        private static int GetNextUserId() {
            using (KozolContainer db = new KozolContainer()) {
                return db.Users.Any() ? db.Users.Max(u => u.ID) + 1 : 1;
            }
        }

        public static CreateUserStatus CreateUser(string email, string password, string username) {
            try {
                using (KozolContainer db = new KozolContainer()) {
                    if (db.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                        return CreateUserStatus.EmailTaken;

                    byte[] salt = KozolUtilities.CreateSalt();
                    int userId = GetNextUserId();

                    User user = new User {
                        ID = userId,
                        Email = email,
                        Salt = Convert.ToBase64String(salt),
                        Password = KozolUtilities.HashStringSHA256(password, salt),
                        Created = DateTime.Now,
                        Username = username
                    };

                    UserRoleMap defaultRole = new UserRoleMap {
                        UserID = userId,
                        RoleID = 1
                    };

                    db.UserRoleMaps.Add(defaultRole);
                    db.Users.Add(user);

                    db.SaveChanges();
                }
            } catch (Exception e) {
                // I don't care what exception we catch, I just don't want the page to break.
                return CreateUserStatus.Failure;
            }

            return CreateUserStatus.Success;
        }

        public static User GetUser(string email) {
            using (KozolContainer db = new KozolContainer()) {
                return db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .FirstOrDefault();
            }
        }

        public static int? GetUserId(string email) {
            using (KozolContainer db = new KozolContainer()) {
                return db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();
            }
        }

        public static ValidateUserStatus ValidateUser(string email, string password) {
            if (String.IsNullOrWhiteSpace(email))
                return ValidateUserStatus.EmailInvalid;
            if (String.IsNullOrWhiteSpace(password))
                return ValidateUserStatus.PassInvalid;

            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return ValidateUserStatus.NotFound;

                return ValidateUser(userId.Value, password);
            }
        }

        public static ValidateUserStatus ValidateUser(int userId, string password) {
            if (userId < 1)
                return ValidateUserStatus.UserIdInvalid;
            if (String.IsNullOrWhiteSpace(password))
                return ValidateUserStatus.PassInvalid;

            using (KozolContainer db = new KozolContainer()) {
                User user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return ValidateUserStatus.NotFound;

                byte[] salt = Convert.FromBase64String(user.Salt);
                if (KozolUtilities.HashStringSHA256(password, salt) == user.Password)
                    return ValidateUserStatus.Success;
                else
                    return ValidateUserStatus.Failure;
            }
        }

        public static SetLoginStatus SetLogin(string email, string password) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return SetLoginStatus.NotFound;

                return SetLogin(userId.Value, password);
            }
        }

        public static SetLoginStatus SetLogin(int userId, string password) {
            if (ValidateUser(userId, password) < ValidateUserStatus.Success)
                return SetLoginStatus.ValidateFailure;

            try {
                // Login status and currently logged in user.
                HttpContext.Current.Session["loggedIn"] = true;
                HttpContext.Current.Session["userId"] = userId;

                // Commonly accessed info.
                using (KozolContainer db = new KozolContainer()) {
                    User user = db.Users
                        .Where(u => u.ID == userId)
                        .First();

                    HttpContext.Current.Session["userName"] = GetUserName(user.ID);
                    // More commonly used info will be stored here.
                }
            } catch (Exception e) {
                // I don't care what exception we catch, I just don't want the page to break.
                return SetLoginStatus.Failure;
            }

            return SetLoginStatus.Success;
        }

        public static string GetUserName(string email) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return null;

                return GetUserName(userId.Value);
            }
        }

        public static string GetUserName(int userId) {
            using (KozolContainer db = new KozolContainer()) {
                User user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user != null) {
                    return user.Username;
                } else {
                    return null;
                }
            }
        }

        public static string GetFullName(string email) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return null;

                return GetFullName(userId.Value);
            }
        }

        public static string GetFullName(int userId) {
            using (KozolContainer db = new KozolContainer()) {
                User user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user != null) {
                    return user.NameFirst + " " + user.NameLast;
                } else {
                    return null;
                }
            }
        }

        public static List<User> GetUsersOfRole(int roleId) {
            using (KozolContainer db = new KozolContainer()) {
                return db.Users
                    .Where(u => u.Roles.Any(r => r.RoleID == roleId))
                    .ToList() ?? new List<User>();
            }
        }

        public static List<User> GetUsersOfRole(string role) {
            using (KozolContainer db = new KozolContainer()) {
                return db.Users
                    .Where(u => u.Roles
                        .Any(r => r.Role.Name.ToLower() == role.ToLower()))
                    .ToList() ?? new List<User>();
            }
        }

        // Very inefficient, needs to run a separate query for each user.
        public static List<AssignUserToRoleStatus> AssignUsersToRole(List<string> emails, int userRole) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (string email in emails)
                statuses.Add(AssignUserToRole(email, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each user.
        public static List<AssignUserToRoleStatus> AssignUsersToRole(List<int> userIds, int userRole) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (int userId in userIds)
                statuses.Add(AssignUserToRole(userId, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each user.
        public static List<AssignUserToRoleStatus> AssignUsersToRole(List<string> emails, string userRole) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (string email in emails)
                statuses.Add(AssignUserToRole(email, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each user.
        public static List<AssignUserToRoleStatus> AssignUsersToRole(List<int> userIds, string userRole) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (int userId in userIds)
                statuses.Add(AssignUserToRole(userId, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each role.
        public static List<AssignUserToRoleStatus> AssignUserToRoles(string email, List<int> userRoles) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (int userRole in userRoles)
                statuses.Add(AssignUserToRole(email, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each role.
        public static List<AssignUserToRoleStatus> AssignUserToRoles(int userId, List<int> userRoles) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (int userRole in userRoles)
                statuses.Add(AssignUserToRole(userId, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each role.
        public static List<AssignUserToRoleStatus> AssignUserToRoles(string email, List<string> userRoles) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (string userRole in userRoles)
                statuses.Add(AssignUserToRole(email, userRole));
            return statuses;
        }

        // Very inefficient, needs to run a separate query for each role.
        public static List<AssignUserToRoleStatus> AssignUserToRoles(int userId, List<string> userRoles) {
            List<AssignUserToRoleStatus> statuses = new List<AssignUserToRoleStatus>();
            foreach (string userRole in userRoles)
                statuses.Add(AssignUserToRole(userId, userRole));
            return statuses;
        }

        public static AssignUserToRoleStatus AssignUserToRole(string email, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return AssignUserToRoleStatus.UserNotFound;

                return AssignUserToRole(userId.Value, userRole);
            }
        }

        public static AssignUserToRoleStatus AssignUserToRole(int userId, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return AssignUserToRoleStatus.UserNotFound;

                if (user.Roles.Any(r => r.RoleID == userRole))
                    return AssignUserToRoleStatus.RoleAlreadyAssigned;

                user.Roles.Add(new UserRoleMap {
                    RoleID = userRole,
                    UserID = user.ID
                });

                db.SaveChanges();
                return AssignUserToRoleStatus.Success;
            }
        }

        public static AssignUserToRoleStatus AssignUserToRole(string email, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return AssignUserToRoleStatus.UserNotFound;

                return AssignUserToRole(userId.Value, userRole);
            }
        }

        public static AssignUserToRoleStatus AssignUserToRole(int userId, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return AssignUserToRoleStatus.UserNotFound;

                var role = db.UserRoles
                    .Where(r => r.Name.ToLower() == userRole.ToLower())
                    .FirstOrDefault();

                if (role == null)
                    return AssignUserToRoleStatus.RoleNotFound;

                if (user.Roles.Any(r => r.RoleID == role.ID))
                    return AssignUserToRoleStatus.RoleAlreadyAssigned;

                var roleMap = role.Roles
                    .Where(ur => ur.UserID == userId)
                    .FirstOrDefault();

                if (roleMap == null)
                    roleMap = new UserRoleMap {
                        UserID = userId,
                        RoleID = role.ID
                    };

                user.Roles.Add(roleMap);

                db.SaveChanges();
                return AssignUserToRoleStatus.Success;
            }
        }

        public static RemoveUserFromRoleStatus RemoveUserFromRole(string email, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return RemoveUserFromRoleStatus.UserNotFound;

                return RemoveUserFromRole(userId.Value, userRole);
            }
        }

        public static RemoveUserFromRoleStatus RemoveUserFromRole(int userId, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return RemoveUserFromRoleStatus.UserNotFound;

                var role = user.Roles
                    .Where(r => r.RoleID == userRole)
                    .FirstOrDefault();

                if (role == null)
                    return RemoveUserFromRoleStatus.RoleNotAssigned;

                db.UserRoles.Remove(
                    db.UserRoles
                        .Where(r => r.ID == userRole && r.ID == userId)
                        .FirstOrDefault()
                );

                db.SaveChanges();
                return RemoveUserFromRoleStatus.Success;
            }
        }

        public static RemoveUserFromRoleStatus RemoveUserFromRole(string email, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return RemoveUserFromRoleStatus.UserNotFound;

                return RemoveUserFromRole(userId.Value, userRole);
            }
        }

        public static RemoveUserFromRoleStatus RemoveUserFromRole(int userId, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return RemoveUserFromRoleStatus.UserNotFound;

                var role = db.UserRoles
                    .Where(r => r.Name.ToLower() == userRole.ToLower())
                    .FirstOrDefault();

                if (role == null)
                    return RemoveUserFromRoleStatus.RoleNotFound;

                var roleMap = role.Roles
                    .Where(ur => ur.UserID == userId)
                    .FirstOrDefault();

                if (roleMap == null)
                    return RemoveUserFromRoleStatus.RoleNotAssigned;

                user.Roles.Remove(roleMap);

                db.SaveChanges();
                return RemoveUserFromRoleStatus.Success;
            }
        }

        public static bool IsUserOfRole(string email, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return false;

                return IsUserOfRole(userId.Value, userRole);
            }
        }

        public static bool IsUserOfRole(int userId, int userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return false;

                var role = db.UserRoles
                    .Where(r => r.ID == userRole)
                    .FirstOrDefault();

                if (role == null)
                    return false;

                return role.Roles
                    .Where(r => r.UserID == userId)
                    .Any();
            }
        }

        public static bool IsUserOfRole(string email, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return false;

                return IsUserOfRole(userId.Value, userRole);
            }
        }

        public static bool IsUserOfRole(int userId, string userRole) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return false;

                var role = db.UserRoles
                    .Where(r => r.Name.ToLower() == userRole.ToLower())
                    .FirstOrDefault();

                if (role == null)
                    return false;

                return role.Roles
                    .Where(r => r.UserID == userId)
                    .Any();
            }
        }

        public static EditUserInfoStatus SetNameFirst(string email, string name) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return EditUserInfoStatus.UserNotFound;

                return SetNameFirst(userId.Value, name);
            }
        }

        public static EditUserInfoStatus SetNameFirst(int userId, string name) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.NameFirst = name;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static EditUserInfoStatus SetNameLast(string email, string name) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return EditUserInfoStatus.UserNotFound;

                return SetNameLast(userId.Value, name);
            }
        }

        public static EditUserInfoStatus SetNameLast(int userId, string name) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.NameLast = name;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static EditUserInfoStatus SetEmail(string email, string newEmail) {
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return EditUserInfoStatus.UserNotFound;

                return SetEmail(userId.Value, newEmail);
            }
        }

        public static EditUserInfoStatus SetEmail(int userId, string newEmail) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.Email = newEmail;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static ChangePasswordStatus ChangePassword(string email, string oldPassword, string newPassword) {
            if (ValidateUser(email, oldPassword) < 0)
                return ChangePasswordStatus.IncorrectPassword;
            if (ValidateUser(email, newPassword) > 0)
                return ChangePasswordStatus.InvalidPassword;
            using (KozolContainer db = new KozolContainer()) {
                int? userId = db.Users
                    .Where(u => u.Email.ToLower() == email.ToLower())
                    .Select(u => u.ID)
                    .FirstOrDefault();

                if (userId == null)
                    return ChangePasswordStatus.UserNotFound;

                return ChangePassword(userId.Value, oldPassword, newPassword);
            }
        }

        public static ChangePasswordStatus ChangePassword(int userId, string oldPassword, string newPassword) {
            if (ValidateUser(userId, oldPassword) < 0)
                return ChangePasswordStatus.IncorrectPassword;
            if (ValidateUser(userId, newPassword) > 0)
                return ChangePasswordStatus.InvalidPassword;

            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == userId)
                    .FirstOrDefault();

                if (user == null)
                    return ChangePasswordStatus.UserNotFound;

                byte[] salt = KozolUtilities.CreateSalt();
                user.Salt = Convert.ToBase64String(salt);
                user.Password = KozolUtilities.HashStringSHA256(newPassword, salt);

                db.SaveChanges();
                return ChangePasswordStatus.Success;
            }
        }
    }

    /*
     * Set of methods that perform the functionality of numerous above methods,
     * but with the extension syntax.
     * 
     * Ex: user.ValidateUser(password)
     */
    public static class UserExtensionMethods {
        public static EditUserInfoStatus SetNameFirst(this User passedUser, string name) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == passedUser.ID)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.NameFirst = name;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static EditUserInfoStatus SetNameLast(this User passedUser, string name) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == passedUser.ID)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.NameLast = name;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static EditUserInfoStatus SetEmail(this User passedUser, string email) {
            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == passedUser.ID)
                    .FirstOrDefault();

                if (user == null)
                    return EditUserInfoStatus.UserNotFound;

                user.Email = email;
                db.SaveChanges();

                return EditUserInfoStatus.Success;
            }
        }

        public static ValidateUserStatus ValidateUser(this User passedUser, string password) {
            if (passedUser.ID < 1)
                return ValidateUserStatus.UserIdInvalid;
            if (String.IsNullOrWhiteSpace(password))
                return ValidateUserStatus.PassInvalid;

            using (KozolContainer db = new KozolContainer()) {
                User user = db.Users
                    .Where(u => u.ID == passedUser.ID)
                    .FirstOrDefault();

                if (user == null)
                    return ValidateUserStatus.NotFound;

                byte[] salt = Convert.FromBase64String(user.Salt);
                if (KozolUtilities.HashStringSHA256(password, salt) == user.Password)
                    return ValidateUserStatus.Success;
                else
                    return ValidateUserStatus.Failure;
            }
        }

        public static ChangePasswordStatus ChangePassword(this User passedUser, string oldPassword, string newPassword) {
            if (passedUser.ValidateUser(oldPassword) < 0)
                return ChangePasswordStatus.IncorrectPassword;
            if (passedUser.ValidateUser(newPassword) > 0)
                return ChangePasswordStatus.InvalidPassword;

            using (KozolContainer db = new KozolContainer()) {
                var user = db.Users
                    .Where(u => u.ID == passedUser.ID)
                    .FirstOrDefault();

                if (user == null)
                    return ChangePasswordStatus.UserNotFound;

                byte[] salt = KozolUtilities.CreateSalt();
                user.Salt = Convert.ToBase64String(salt);
                user.Password = KozolUtilities.HashStringSHA256(newPassword, salt);

                db.SaveChanges();
                return ChangePasswordStatus.Success;
            }
        }
    }
}