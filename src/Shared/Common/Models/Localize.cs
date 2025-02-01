using System.Collections.Frozen;
using Shared.Common.Enums;

namespace Shared.Common.Models;

public static class Localize
{
    public static class Keys
    {
        public static readonly FrozenDictionary<Language, FrozenDictionary<string, string>> Dictionary = Dictionary =
            new Dictionary<Language, FrozenDictionary<string, string>>
            {
                {
                    Language.en_US, new Dictionary<string, string>
                    {
                        {
                            Events.UserDeleted.Name,
                            "User deleted"
                        },
                        {
                            Events.UserDeleted.Format,
                            "User {UserId} has been deleted"
                        },
                        {
                            Events.UserUpdated.Name,
                            "User updated"
                        },
                        {
                            Events.UserUpdated.Format,
                            "User {Email} has been updated"
                        },
                        {
                            Events.UserCreated.Name,
                            "User created"
                        },
                        {
                            Events.UserCreated.Format,
                            "User {Email} has been created"
                        },
                        {
                            Events.AuthSignedIn.Name,
                            "User signed in"
                        },
                        {
                            Events.AuthSignedIn.Format,
                            "User {UserId} has been signed in"
                        },
                        {
                            Events.UserLinkedUserGroup.Name,
                            "User linked to user group"
                        },
                        {
                            Events.UserLinkedUserGroup.Format,
                            "User {UserId} has been linked to user group {UserGroupId}"
                        },
                        {
                            Events.AuthSignedOut.Name,
                            "User signed out"
                        },
                        {
                            Events.AuthSignedOut.Format,
                            "User {UserId} has been signed out"
                        },
                        {
                            Events.UserUnlinkedUserGroup.Name,
                            "User unlinked from user group"
                        },
                        {
                            Events.UserUnlinkedUserGroup.Format,
                            "User {UserId} has been unlinked from user group {UserGroupId}"
                        }
                    }.ToFrozenDictionary()
                },
                {
                    Language.ru_RU, new Dictionary<string, string>
                    {
                        {
                            Events.UserDeleted.Name,
                            "Пользователь удален"
                        },
                        {
                            Events.UserDeleted.Format,
                            "Пользователь {0} был удален"
                        },
                        {
                            Events.UserUpdated.Name,
                            "Пользоваль обновлен"
                        },
                        {
                            Events.UserUpdated.Format,
                            "Данные пользователя {0} были обновлены"
                        },
                        {
                            Events.UserCreated.Name,
                            "Создан пользователь"
                        },
                        {
                            Events.UserCreated.Format,
                            "Был создан новый пользователь {0}"
                        },
                        {
                            Events.AuthSignedIn.Name,
                            "Пользователь вошел"
                        },
                        {
                            Events.AuthSignedIn.Format,
                            "Пользователь {0} вошел в систему"
                        },
                        {
                            Events.UserLinkedUserGroup.Name,
                            "Пользователь добавлен в группу"
                        },
                        {
                            Events.UserLinkedUserGroup.Format,
                            "Пользователь {0} добавлен в группу {1}"
                        },
                        {
                            Events.AuthSignedOut.Name,
                            "Пользователь вышел из системы"
                        },
                        {
                            Events.AuthSignedOut.Format,
                            "Пользователь {0} вышел из системы"
                        },
                        {
                            Events.UserUnlinkedUserGroup.Format,
                            "Пользоватль удален из группы"
                        },
                        {
                            Events.UserUnlinkedUserGroup.Format,
                            "Пользователь {0} был удален из группы {1}"
                        }
                    }.ToFrozenDictionary()
                }
            }.ToFrozenDictionary();

        public static class Error
        {
            #region HttpClient

            public const string ResponseStatusCodeUnsuccessful = "#UI_ResponseStatusCodeUnsuccessful";
            public const string RequestTimedOut = "#UI_RequestTimedOut";

            #endregion

            #region Exception

            public const string UnhandledExceptionContactSystemAdministrator =
                "#UI_UnhandledExceptionContactSystemAdministrator";

            public const string HandledExceptionContactSystemAdministrator =
                "#UI_HandledExceptionContactSystemAdministrator";

            #endregion

            #region Generic

            public const string SignalRDisconnectFiltered = "#UI_SignalRConnectionExpired";

            #endregion
        }

        public static class Warning
        {
            public static string XssVulnerable => "#UI_XssVulnerable";
            public static string MappingAlreadyExists => "#UI_MappingAlreadyExists";
            public static string MappingNotFound => "#UI_MappingNotFound";
        }

        public static class Descriptions
        {
            public const string SomeTypeDescription = "#UI_SomeTypeDescription";
            public const string SomePropertyDescription = "#UI_SomePropertyDescription";
        }

        public static class Events
        {
            public static class AuthSignedIn
            {
                public const string Name = "#UI_Events_AuthSignIn_EventName";
                public const string Format = "#UI_Events_AuthSignIn_Format";
            }

            public static class AuthSignedOut
            {
                public const string Name = "#UI_Events_AuthSignOut_EventName";
                public const string Format = "#UI_Events_AuthSignOut_Format";
            }

            public static class UserCreated
            {
                public const string Name = "#UI_Events_UserCreated_EventName";
                public const string Format = "#UI_Events_UserCreated_Format";
            }

            public static class UserDeleted
            {
                public const string Name = "#UI_Events_UserDeleted_EventName";
                public const string Format = "#UI_Events_UserDeleted_Format";
            }

            public static class UserLinkedUserGroup
            {
                public const string Name = "#UI_Events_UserLinkedUserGroup_EventName";
                public const string Format = "#UI_Events_UserLinkedUserGroup_Format";
            }

            public static class UserUnlinkedUserGroup
            {
                public const string Name = "#UI_Events_UserUnlinkedUserGroup_EventName";
                public const string Format = "#UI_Events_UserUnlinkedUserGroup_Format";
            }

            public static class UserUpdated
            {
                public const string Name = "#UI_Events_UserUpdated_EventName";
                public const string Format = "#UI_Events_UserUpdated_Format";
            }
        }
    }

    public static class Log
    {
        public const string BackgroundServiceStarting = "(Bacgkround service: Starting)";
        public const string BackgroundServiceStopping = "(Bacgkround service: Stopping)";
        public const string BackgroundServiceWorking = "(Bacgkround service: Working)";
    }
}