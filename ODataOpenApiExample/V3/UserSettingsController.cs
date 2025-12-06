namespace ApiVersioning.Examples.V3;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using ODataOpenApiExample.Data;
using ODataOpenApiExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.OData.Query.AllowedQueryOptions;

/// <summary>
/// Represents a RESTful people service.
/// </summary>
[ApiVersion(3.0)]
public class UserSettingsController : ODataController
{
    private readonly AppDbContext _context;

    public UserSettingsController(AppDbContext context)
    {
        _context = context;
    }

    // GET odata/UserSettings(UserId=guid,SettingName='name')
    [HttpGet]
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<string>), Status200OK)]

    public async Task<IActionResult> Get([FromODataUri] Guid userId, [FromODataUri] string settingName)
    {
        var setting = await _context.UserSettings
            .FindAsync(userId, settingName);

        if (setting == null)
            return NotFound();

        return Ok(setting);
    }

    // POST odata/UserSettings
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserSetting setting)
    {
        _context.UserSettings.Add(setting);
        await _context.SaveChangesAsync();
        return Created(setting);
    }

    // PUT odata/UserSettings(UserId=guid,SettingName='name')
    [HttpPut]
    public async Task<IActionResult> Put([FromODataUri] Guid userId, [FromODataUri] string settingName, [FromBody] UserSetting updated)
    {
        if (userId != updated.UserId || settingName != updated.SettingName)
            return BadRequest();

        _context.Entry(updated).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Updated(updated);
    }

    // DELETE odata/UserSettings(UserId=guid,SettingName='name')
    [HttpDelete]
    public async Task<IActionResult> Delete([FromODataUri] Guid userId, [FromODataUri] string settingName)
    {
        var setting = await _context.UserSettings.FindAsync(userId, settingName);
        if (setting == null)
            return NotFound();

        _context.UserSettings.Remove(setting);
        await _context.SaveChangesAsync();
        return StatusCode(204); // NoContent
    }
}
