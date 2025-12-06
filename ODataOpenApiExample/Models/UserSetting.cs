using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace ODataOpenApiExample.Models
{
    //CREATE TABLE UserSettings(
    //UserId UNIQUEIDENTIFIER NOT NULL,
    //SettingName NVARCHAR(200) NOT NULL,
    //SettingValue NVARCHAR(MAX) NOT NULL,
    //CONSTRAINT PK_UserSettings PRIMARY KEY(UserId, SettingName)
    public class UserSetting
    {
        public Guid UserId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
