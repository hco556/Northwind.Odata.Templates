namespace ApiVersioning.Examples.Configuration;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.OData.ModelBuilder;
using ODataOpenApiExample.Models;

/// <summary>
/// Represents the model configuration for suppliers.
/// </summary>
public class UserSettingsConfiguration : IModelConfiguration
{
    /// <inheritdoc />
    public void Apply( ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix )
    {
        if ( apiVersion < ApiVersions.V3 )
        {
            return;
        }

        var userSetting = builder.EntitySet<UserSetting>( "UserSettings" ).EntityType;
        userSetting.HasKey( p => p.SettingName );

        
        //userSetting.Page( maxTopValue: 100, pageSizeValue: default );

        //builder.Singleton<Supplier>( "Acme" );
    }
}
