This file is a merged representation of the entire codebase, combined into a single document by Repomix.

# File Summary

## Purpose
This file contains a packed representation of the entire repository's contents.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Files are sorted by Git change count (files with more changes are at the bottom)

# Directory Structure
```
.gitattributes
.gitignore
RoomBookingApp.API.Tests/RoomBookingApp.API.Tests.csproj
RoomBookingApp.API.Tests/RoomBookingControllerTest.cs
RoomBookingApp.API.Tests/WeatherControllerTests.cs
RoomBookingApp.API/appsettings.Development.json
RoomBookingApp.API/appsettings.json
RoomBookingApp.API/Controllers/RoomBookingController.cs
RoomBookingApp.API/Controllers/WeatherForecastController.cs
RoomBookingApp.API/Program.cs
RoomBookingApp.API/Properties/launchSettings.json
RoomBookingApp.API/RoomBookingApp.API.csproj
RoomBookingApp.API/RoomBookingApp.API.http
RoomBookingApp.API/WeatherForecast.cs
RoomBookingApp.Core.Test/RoomBookingApp.Core.Test.csproj
RoomBookingApp.Core.Test/RoomBookingRequestProcessorTest.cs
RoomBookingApp.Core/DataServices/IRoomBookingService.cs
RoomBookingApp.Core/Enums/BookingResultFlag.cs
RoomBookingApp.Core/Models/RoomBookingRequest.cs
RoomBookingApp.Core/Models/RoomBookingResult.cs
RoomBookingApp.Core/Processors/IRoomBookingRequestProcessor.cs
RoomBookingApp.Core/Processors/RoomBookingRequestProcessor.cs
RoomBookingApp.Core/RoomBookingApp.Core.csproj
RoomBookingApp.Domain/BaseModels/RoomBookingBase.cs
RoomBookingApp.Domain/Room.cs
RoomBookingApp.Domain/RoomBooking.cs
RoomBookingApp.Domain/RoomBookingApp.Domain.csproj
RoomBookingApp.Persistence.Test/RoomBookingApp.Persistence.Test.csproj
RoomBookingApp.Persistence.Test/RoomBookingServiceTest.cs
RoomBookingApp.Persistence/Repositories/RoomBookingService.cs
RoomBookingApp.Persistence/RoomBookingApp.Persistence.csproj
RoomBookingApp.Persistence/RoomBookingAppDbContext.cs
RoomBookingApp.slnx
```

# Files

## File: RoomBookingApp.API.Tests/RoomBookingApp.API.Tests.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.2" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="Moq" Version="4.21.0" />
    <PackageReference Include="Shouldly" Version="4.3.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.API\RoomBookingApp.API.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.API.Tests/RoomBookingControllerTest.cs
```csharp
using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.API.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.API.Tests
{
    public class RoomBookingControllerTest
    {
        private Mock<IRoomBookingRequestProcessor> _roomBookingProcessor;
        private RoomBookingController _controller;
        private RoomBookingRequest _request;
        private RoomBookingResult _result;

        public RoomBookingControllerTest()
        {
            _roomBookingProcessor = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomBookingProcessor.Object);
            _request = new RoomBookingRequest();
            _result = new RoomBookingResult();

            _roomBookingProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
        public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCalls, bool isModelValid,
               Type expectedActionResultType, BookingResultFlag bookingResultFlag)
        {
            // Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");
            }

            _result.Flag = bookingResultFlag;


            // Act
            var result = await _controller.BookRoom(_request);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);
            _roomBookingProcessor.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCalls));

        }
    }
}
```

## File: RoomBookingApp.API.Tests/WeatherControllerTests.cs
```csharp
using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.API.Controllers;
using Shouldly;

namespace RoomBookingApp.API.Tests
{
    public class WeatherControllerTests
    {
        [Fact]
        public void Should_Return_Forecast_Results()
        {
            // Arrange
            var logger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(logger.Object);

            // Act
            var result = controller.Get();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
        }
    }
}
```

## File: RoomBookingApp.API/appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## File: RoomBookingApp.API/appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## File: RoomBookingApp.API/Controllers/RoomBookingController.cs
```csharp
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController(IRoomBookingRequestProcessor _roomBookingProcessor) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _roomBookingProcessor.BookRoom(request);
                if (result.Flag == BookingResultFlag.Success)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No Rooms Available For Given Date");
            }

            return BadRequest(ModelState);
        }
    }
}
```

## File: RoomBookingApp.API/Controllers/WeatherForecastController.cs
```csharp
using Microsoft.AspNetCore.Mvc;

namespace RoomBookingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
```

## File: RoomBookingApp.API/Program.cs
```csharp
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure services (equivalent to ConfigureServices in .NET 5)
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RoomBookingApp.Api",
        Version = "v1"
    });
});

// 2. Configure the SQLite in-memory database
var connString = "Filename=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();

builder.Services.AddDbContext<RoomBookingAppDbContext>(
    options => options.UseSqlite(conn));

EnsureDatabaseCreated(conn);

// 3. Register application services
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
builder.Services.AddScoped<IRoomBookingRequestProcessor,
    RoomBookingRequestProcessor>();

var app = builder.Build();

// 4. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "RoomBookingApp.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

// 5. Create the database
static void EnsureDatabaseCreated(SqliteConnection conn)
{
    var options = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
    options.UseSqlite(conn);

    using var context = new RoomBookingAppDbContext(options.Options);
    context.Database.EnsureCreated();
}
```

## File: RoomBookingApp.API/Properties/launchSettings.json
```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5103",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:7117;http://localhost:5103",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## File: RoomBookingApp.API/RoomBookingApp.API.csproj
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.20" />
    <PackageReference Include="Microsoft.Data.Sqlite.Core" Version="9.0.21" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.21">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.21" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.6" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.Core\RoomBookingApp.Core.csproj" />
    <ProjectReference Include="..\RoomBookingApp.Persistence\RoomBookingApp.Persistence.csproj" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.API/RoomBookingApp.API.http
```
@RoomBookingApp.API_HostAddress = http://localhost:5103

GET {{RoomBookingApp.API_HostAddress}}/weatherforecast/
Accept: application/json

###
```

## File: RoomBookingApp.API/WeatherForecast.cs
```csharp
namespace RoomBookingApp.API
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
```

## File: RoomBookingApp.Core/Processors/IRoomBookingRequestProcessor.cs
```csharp
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public interface IRoomBookingRequestProcessor
    {
        RoomBookingResult BookRoom(RoomBookingRequest bookingRequest);
    }
}
```

## File: .gitattributes
```
###############################################################################
# Set default behavior to automatically normalize line endings.
###############################################################################
* text=auto

###############################################################################
# Set default behavior for command prompt diff.
#
# This is need for earlier builds of msysgit that does not have it on by
# default for csharp files.
# Note: This is only used by command line
###############################################################################
#*.cs     diff=csharp

###############################################################################
# Set the merge driver for project and solution files
#
# Merging from the command prompt will add diff markers to the files if there
# are conflicts (Merging from VS is not affected by the settings below, in VS
# the diff markers are never inserted). Diff markers may cause the following 
# file extensions to fail to load in VS. An alternative would be to treat
# these files as binary and thus will always conflict and require user
# intervention with every merge. To do so, just uncomment the entries below
###############################################################################
#*.sln       merge=binary
#*.csproj    merge=binary
#*.vbproj    merge=binary
#*.vcxproj   merge=binary
#*.vcproj    merge=binary
#*.dbproj    merge=binary
#*.fsproj    merge=binary
#*.lsproj    merge=binary
#*.wixproj   merge=binary
#*.modelproj merge=binary
#*.sqlproj   merge=binary
#*.wwaproj   merge=binary

###############################################################################
# behavior for image files
#
# image files are treated as binary by default.
###############################################################################
#*.jpg   binary
#*.png   binary
#*.gif   binary

###############################################################################
# diff behavior for common document formats
# 
# Convert binary document formats to text before diffing them. This feature
# is only available from the command line. Turn it on by uncommenting the 
# entries below.
###############################################################################
#*.doc   diff=astextplain
#*.DOC   diff=astextplain
#*.docx  diff=astextplain
#*.DOCX  diff=astextplain
#*.dot   diff=astextplain
#*.DOT   diff=astextplain
#*.pdf   diff=astextplain
#*.PDF   diff=astextplain
#*.rtf   diff=astextplain
#*.RTF   diff=astextplain
```

## File: .gitignore
```
## Ignore Visual Studio temporary files, build results, and
## files generated by popular Visual Studio add-ons.
##
## Get latest from https://github.com/github/gitignore/blob/master/VisualStudio.gitignore

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# User-specific files (MonoDevelop/Xamarin Studio)
*.userprefs

# Mono auto generated files
mono_crash.*

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Oo]ut/
[Ll]og/
[Ll]ogs/

# Visual Studio 2015/2017 cache/options directory
.vs/
# Uncomment if you have tasks that create the project's static files in wwwroot
#wwwroot/

# Visual Studio 2017 auto generated files
Generated\ Files/

# MSTest test Results
[Tt]est[Rr]esult*/
[Bb]uild[Ll]og.*

# NUnit
*.VisualState.xml
TestResult.xml
nunit-*.xml

# Build Results of an ATL Project
[Dd]ebugPS/
[Rr]eleasePS/
dlldata.c

# Benchmark Results
BenchmarkDotNet.Artifacts/

# .NET Core
project.lock.json
project.fragment.lock.json
artifacts/

# ASP.NET Scaffolding
ScaffoldingReadMe.txt

# StyleCop
StyleCopReport.xml

# Files built by Visual Studio
*_i.c
*_p.c
*_h.h
*.ilk
*.meta
*.obj
*.iobj
*.pch
*.pdb
*.ipdb
*.pgc
*.pgd
*.rsp
*.sbr
*.tlb
*.tli
*.tlh
*.tmp
*.tmp_proj
*_wpftmp.csproj
*.log
*.vspscc
*.vssscc
.builds
*.pidb
*.svclog
*.scc

# Chutzpah Test files
_Chutzpah*

# Visual C++ cache files
ipch/
*.aps
*.ncb
*.opendb
*.opensdf
*.sdf
*.cachefile
*.VC.db
*.VC.VC.opendb

# Visual Studio profiler
*.psess
*.vsp
*.vspx
*.sap

# Visual Studio Trace Files
*.e2e

# TFS 2012 Local Workspace
$tf/

# Guidance Automation Toolkit
*.gpState

# ReSharper is a .NET coding add-in
_ReSharper*/
*.[Rr]e[Ss]harper
*.DotSettings.user

# TeamCity is a build add-in
_TeamCity*

# DotCover is a Code Coverage Tool
*.dotCover

# AxoCover is a Code Coverage Tool
.axoCover/*
!.axoCover/settings.json

# Coverlet is a free, cross platform Code Coverage Tool
coverage*.json
coverage*.xml
coverage*.info

# Visual Studio code coverage results
*.coverage
*.coveragexml

# NCrunch
_NCrunch_*
.*crunch*.local.xml
nCrunchTemp_*

# MightyMoose
*.mm.*
AutoTest.Net/

# Web workbench (sass)
.sass-cache/

# Installshield output folder
[Ee]xpress/

# DocProject is a documentation generator add-in
DocProject/buildhelp/
DocProject/Help/*.HxT
DocProject/Help/*.HxC
DocProject/Help/*.hhc
DocProject/Help/*.hhk
DocProject/Help/*.hhp
DocProject/Help/Html2
DocProject/Help/html

# Click-Once directory
publish/

# Publish Web Output
*.[Pp]ublish.xml
*.azurePubxml
# Note: Comment the next line if you want to checkin your web deploy settings,
# but database connection strings (with potential passwords) will be unencrypted
*.pubxml
*.publishproj

# Microsoft Azure Web App publish settings. Comment the next line if you want to
# checkin your Azure Web App publish settings, but sensitive information contained
# in these scripts will be unencrypted
PublishScripts/

# NuGet Packages
*.nupkg
# NuGet Symbol Packages
*.snupkg
# The packages folder can be ignored because of Package Restore
**/[Pp]ackages/*
# except build/, which is used as an MSBuild target.
!**/[Pp]ackages/build/
# Uncomment if necessary however generally it will be regenerated when needed
#!**/[Pp]ackages/repositories.config
# NuGet v3's project.json files produces more ignorable files
*.nuget.props
*.nuget.targets

# Microsoft Azure Build Output
csx/
*.build.csdef

# Microsoft Azure Emulator
ecf/
rcf/

# Windows Store app package directories and files
AppPackages/
BundleArtifacts/
Package.StoreAssociation.xml
_pkginfo.txt
*.appx
*.appxbundle
*.appxupload

# Visual Studio cache files
# files ending in .cache can be ignored
*.[Cc]ache
# but keep track of directories ending in .cache
!?*.[Cc]ache/

# Others
ClientBin/
~$*
*~
*.dbmdl
*.dbproj.schemaview
*.jfm
*.pfx
*.publishsettings
orleans.codegen.cs

# Including strong name files can present a security risk
# (https://github.com/github/gitignore/pull/2483#issue-259490424)
#*.snk

# Since there are multiple workflows, uncomment next line to ignore bower_components
# (https://github.com/github/gitignore/pull/1529#issuecomment-104372622)
#bower_components/

# RIA/Silverlight projects
Generated_Code/

# Backup & report files from converting an old project file
# to a newer Visual Studio version. Backup files are not needed,
# because we have git ;-)
_UpgradeReport_Files/
Backup*/
UpgradeLog*.XML
UpgradeLog*.htm
ServiceFabricBackup/
*.rptproj.bak

# SQL Server files
*.mdf
*.ldf
*.ndf

# Business Intelligence projects
*.rdl.data
*.bim.layout
*.bim_*.settings
*.rptproj.rsuser
*- [Bb]ackup.rdl
*- [Bb]ackup ([0-9]).rdl
*- [Bb]ackup ([0-9][0-9]).rdl

# Microsoft Fakes
FakesAssemblies/

# GhostDoc plugin setting file
*.GhostDoc.xml

# Node.js Tools for Visual Studio
.ntvs_analysis.dat
node_modules/

# Visual Studio 6 build log
*.plg

# Visual Studio 6 workspace options file
*.opt

# Visual Studio 6 auto-generated workspace file (contains which files were open etc.)
*.vbw

# Visual Studio LightSwitch build output
**/*.HTMLClient/GeneratedArtifacts
**/*.DesktopClient/GeneratedArtifacts
**/*.DesktopClient/ModelManifest.xml
**/*.Server/GeneratedArtifacts
**/*.Server/ModelManifest.xml
_Pvt_Extensions

# Paket dependency manager
.paket/paket.exe
paket-files/

# FAKE - F# Make
.fake/

# CodeRush personal settings
.cr/personal

# Python Tools for Visual Studio (PTVS)
__pycache__/
*.pyc

# Cake - Uncomment if you are using it
# tools/**
# !tools/packages.config

# Tabs Studio
*.tss

# Telerik's JustMock configuration file
*.jmconfig

# BizTalk build output
*.btp.cs
*.btm.cs
*.odx.cs
*.xsd.cs

# OpenCover UI analysis results
OpenCover/

# Azure Stream Analytics local run output
ASALocalRun/

# MSBuild Binary and Structured Log
*.binlog

# NVidia Nsight GPU debugger configuration file
*.nvuser

# MFractors (Xamarin productivity tool) working folder
.mfractor/

# Local History for Visual Studio
.localhistory/

# BeatPulse healthcheck temp database
healthchecksdb

# Backup folder for Package Reference Convert tool in Visual Studio 2017
MigrationBackup/

# Ionide (cross platform F# VS Code tools) working folder
.ionide/

# Fody - auto-generated XML schema
FodyWeavers.xsd
```

## File: RoomBookingApp.Core/Enums/BookingResultFlag.cs
```csharp
namespace RoomBookingApp.Core.Enums
{
    public enum BookingResultFlag
    {
        Success,
        Failure
    }
}
```

## File: RoomBookingApp.Domain/BaseModels/RoomBookingBase.cs
```csharp
using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Domain.BaseModels
{
    public abstract class RoomBookingBase : IValidatableObject
    {
        [Required]
        [StringLength(80)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Date Must be In The Future", new[] { nameof(Date) });
            }
        }
    }
}
```

## File: RoomBookingApp.Domain/Room.cs
```csharp
namespace RoomBookingApp.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<RoomBooking> RoomBookings { get; set; } = new List<RoomBooking>();
        // 1:M relationship with RoomBooking
    }
}
```

## File: RoomBookingApp.Domain/RoomBooking.cs
```csharp
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Domain
{
    public class RoomBooking : RoomBookingBase
    {
        public int Id { get; set; }

        public Room Room { get; set; } 
        public int RoomId { get; set; }
    }
}
```

## File: RoomBookingApp.Domain/RoomBookingApp.Domain.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

## File: RoomBookingApp.Persistence.Test/RoomBookingApp.Persistence.Test.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="9.0.21" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.Persistence\RoomBookingApp.Persistence.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.Persistence.Test/RoomBookingServiceTest.cs
```csharp
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Test
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // arrange
            var date = new DateTime(2026, 10, 23);
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase(databaseName: "AvailableRoomsTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { Id = 1, RoomId = 1, Date = date });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, Date = date.AddDays(-1) });
            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // assert
            Assert.Equal(2, availableRooms.Count());
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase(databaseName: "ShouldSaveTest")
            .Options;

            var roomBooking = new RoomBooking
            {
                RoomId = 1,
                Date = new DateTime(2026, 10, 23)
            };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);

            // act
            roomBookingService.Save(roomBooking);
            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            // assert
            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
        }


    }
}
```

## File: RoomBookingApp.Persistence/Repositories/RoomBookingService.cs
```csharp
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService (RoomBookingAppDbContext _context): IRoomBookingService
    {
        public IEnumerable<Room> GetAvailableRooms(DateTime date)
            => _context.Rooms
                .Where(r => !r.RoomBookings.Any(rb => rb.Date == date))
                .ToList();

        public void Save(RoomBooking roomBooking)
        {
            _context.RoomBookings.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
```

## File: RoomBookingApp.Persistence/RoomBookingApp.Persistence.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.21">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.21" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.21" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.Core\RoomBookingApp.Core.csproj" />
    <ProjectReference Include="..\RoomBookingApp.Domain\RoomBookingApp.Domain.csproj" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.Persistence/RoomBookingAppDbContext.cs
```csharp
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence
{
    public class RoomBookingAppDbContext : DbContext
    {
        public RoomBookingAppDbContext(DbContextOptions<RoomBookingAppDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Conference Room A" },
                new Room { Id = 2, Name = "Conference Room B" },
                new Room { Id = 3, Name = "Conference Room C" }
            );

        }
    }
}
```

## File: RoomBookingApp.Core.Test/RoomBookingApp.Core.Test.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.2" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="Moq" Version="4.21.0" />
    <PackageReference Include="Shouldly" Version="4.3.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.Core\RoomBookingApp.Core.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.Core/DataServices/IRoomBookingService.cs
```csharp
using RoomBookingApp.Domain;

namespace RoomBookingApp.Core.DataServices
{
    public interface IRoomBookingService
    {
        void Save(RoomBooking roomBooking);

        IEnumerable<Room> GetAvailableRooms(DateTime date);
    }
}
```

## File: RoomBookingApp.Core/RoomBookingApp.Core.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\RoomBookingApp.Domain\RoomBookingApp.Domain.csproj" />
  </ItemGroup>

</Project>
```

## File: RoomBookingApp.slnx
```
<Solution>
  <Project Path="RoomBookingApp.API.Tests/RoomBookingApp.API.Tests.csproj" Id="7605162b-d004-4407-b452-13211492924e" />
  <Project Path="RoomBookingApp.API/RoomBookingApp.API.csproj" Id="8d26a568-d179-45af-b547-3dca8581bdaf" />
  <Project Path="RoomBookingApp.Core.Test/RoomBookingApp.Core.Test.csproj" />
  <Project Path="RoomBookingApp.Core/RoomBookingApp.Core.csproj" />
  <Project Path="RoomBookingApp.Domain/RoomBookingApp.Domain.csproj" Id="dc9b6547-e328-47a2-92bf-86c126844bb2" />
  <Project Path="RoomBookingApp.Persistence.Test/RoomBookingApp.Persistence.Test.csproj" Id="20ac6e3a-7b7f-481b-bd02-2ceb22f7586b" />
  <Project Path="RoomBookingApp.Persistence/RoomBookingApp.Persistence.csproj" Id="895a4c9d-8bbd-42c6-9bd1-e5a6300c9186" />
</Solution>
```

## File: RoomBookingApp.Core.Test/RoomBookingRequestProcessorTest.cs
```csharp
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;

namespace RoomBookingApp.Core.Test
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {
            // Arrange
            _request = new RoomBookingRequest
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Date = new DateTime(2024, 6, 1)
            };

            _availableRooms = new List<Room>() { new Room() { Id = 1 } };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();

            _roomBookingServiceMock.Setup(x => x.GetAvailableRooms(_request.Date))
                .Returns(_availableRooms);

            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
            // Object is to reach the actual concrete implementation of the IRoomBookingService interface, which is mocked using Moq. The mock allows us to simulate the behavior of the service without relying on a real implementation, making it easier to test the RoomBookingRequestProcessor in isolation.
        }

        [Fact]
        public void Should_Return_RoomBooking_Response_With_Request_Values()
        {
            // Arrange globally

            // Act
            RoomBookingResult result = _processor.BookRoom(_request);
            // Assert
            #region using built in Assertion.
            //Assert.NotNull(result);
            //Assert.Equal(request.FullName, result.FullName);
            //Assert.Equal(request.Email, result.Email);
            //Assert.Equal(request.Date, result.Date); 
            #endregion

            // or i can use Shouldly assertion library to make it more readable
            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.Date.ShouldBe(_request.Date);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Request_Is_Null()
        {
            // here the arrange is not needed because we are testing the null case, so we can directly call the method with null and assert the exception. as all the arrange we needed is the processor object and we have done it once using the constructor.

            // Act & Assert
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_RoomBooking_Request()
        {
            // Arrange
          RoomBooking savedBooking = null;

            // Setup the mock to capture the saved booking
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking => savedBooking = booking);

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);
            // Assert
            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_request.FullName);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.Date.ShouldBe(_request.Date);
            savedBooking.RoomId.ShouldBe(_availableRooms.First().Id); 
        }

        [Fact]
        public void Should_Not_Save_RoomBooking_Request_If_Room_Is_Unavailable()
        {
            _availableRooms.Clear(); // Simulate no available rooms
            // Act
            _processor.BookRoom(_request);
            // Assert
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure , false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingResultFlag , bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableRooms.Clear(); // Simulate no available rooms
            }
            // Act
            var result = _processor.BookRoom(_request);
            // Assert
            bookingResultFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1,true)]
        [InlineData(null,false)]
        public void Should_Return_BookingId_In_Result(int? roomBookingId , bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableRooms.Clear(); // Simulate no available rooms
            }
            else
            {
                _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking => booking.Id = roomBookingId.Value);
            }
            var result = _processor.BookRoom(_request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
```

## File: RoomBookingApp.Core/Models/RoomBookingRequest.cs
```csharp
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingRequest : RoomBookingBase
    {
    }
}
```

## File: RoomBookingApp.Core/Models/RoomBookingResult.cs
```csharp
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResult : RoomBookingBase
    {
        public BookingResultFlag Flag { get; set; }
        public int? RoomBookingId { get; set; }
    }
}
```

## File: RoomBookingApp.Core/Processors/RoomBookingRequestProcessor.cs
```csharp
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor : IRoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }
        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if (bookingRequest is null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);

            var result = CreateRoomBookingObj<RoomBookingResult>(bookingRequest);
            if (availableRooms.Any())
            {
                var rooms = availableRooms.First();
                var roomBooking = CreateRoomBookingObj<RoomBooking>(bookingRequest);
                roomBooking.RoomId = rooms.Id;
                // Save the booking request to the database using the IRoomBookingService
                _roomBookingService.Save(roomBooking);

                result.RoomBookingId = roomBooking.Id;
                result.Flag = BookingResultFlag.Success;
            }
            else
            {
                result.Flag = BookingResultFlag.Failure;
            }

            // Return a RoomBookingResult object with the same values as the bookingRequest
            return result;
        }

        private static TRoomBooking CreateRoomBookingObj<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date
            };
        }
    }
}
```
