namespace Shared.Common.Models;

public static class Consts
{
    public const long SolutionMaxFileSize = 2097152L;
    
    public static readonly Guid RootUserId = new("51D5F7CA-E2FA-4DF8-96BF-E435EC016920");
    public static readonly Guid PublicUserId = new("4FC26534-803D-4561-A3B6-8E9EB7E99DE4");

    public static readonly Guid RootUserGroupId = new("D5147F6F-C7D7-4ABC-BD4B-9A40FA6F5BEE");
    public static readonly Guid ManageUsersUserGroupId = new("CA0C48E0-8070-404B-9584-D7EC2A95D173");
    public static readonly Guid ManageAlertLevelsUserGroupId = new("EF6E1C3D-CA25-470E-8E71-76634D72CC20");
    public static readonly Guid ManageEventsUserGroupId = new("3C41B80B-3F7C-410A-9863-1395B119D45C");

    public static readonly Guid AuthSignedInEventId = new("D2AEC155-DBCB-44E2-8FCE-641226356032");
    public static readonly Guid AuthSignedOutEventId = new("E239267F-7C7B-4B40-AE73-E24097687C18");
    public static readonly Guid UserCreatedEventId = new("799A1359-FA18-4B3D-8F79-A6078369B3F9");
    public static readonly Guid UserDeletedEventId = new("2CD7EB60-C786-4779-AF2B-AAAE5C29235D");
    public static readonly Guid UserLinkedUserGroupEventId = new("2BF72ACE-A800-4716-AF1D-C63B2DCB63B0");
    public static readonly Guid UserUnlinkedUserGroupEventId = new("C27963BF-305E-414F-B652-E999A06D8A78");
    public static readonly Guid UserUpdatedEventId = new("FBC3C607-303D-4AC7-8B17-93AAD0DF8DF8");

    public static readonly Guid InformationAlertLevelId = new("81BBF701-5F16-4579-9DCC-35454AB07F1A");
    public static readonly Guid VerboseAlertLevelId = new("88439CC8-045F-497E-911F-65C2021549A3");
    public static readonly Guid DebugAlertLevelId = new("430A317D-CAAB-4DC0-B8DC-48BF047E09B5");
    public static readonly Guid WarningAlertLevelId = new("AA1CCC86-4B51-4B1A-B868-42CB00B1EEEA");
    public static readonly Guid ErrorAlertLevelId = new("7A440BEC-27E6-4833-99B2-23DE124CB0EB");
    public static readonly Guid FatalAlertLevelId = new("4DD68AD3-AFD1-4E41-8B52-84A9C7BE7CF2");
}