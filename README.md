[![Publish to NuGet Gallery](https://github.com/OlegGavrilov/AspNetCore.Identity.Marten.Updated/actions/workflows/build_test_deploy.yml/badge.svg)](https://github.com/OlegGavrilov/AspNetCore.Identity.Marten.Updated/actions/workflows/build_test_deploy.yml)

# AspNetCore.Identity.Marten.Updated
ASP.NET Identity Stores using latest Marten and .NET

The users and roles stores are tested by using the offical Microsoft.AspNetCore.Identity.Specification.Tests packages. Have a look at the Marten.AspNetCore.Identity.Tests project. 
To run the tests you should have docker installed. A PostgreSQL image will be automatically downloaded and used during the test. 

 
# How to configure it

This repository has a small example project which setup the Microsoft Identity to use this package.


1. Add this [package](https://www.nuget.org/packages/Marten.AspNetCore.Identity/) to your project.

2. While adding the default Marten support, you should configure the MartenIdentityUser and MartenIdentityRole entities:
```c#
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Marten"));
    // Configure the MartenIdentityUser and MartenIdentityRole mappings
    options.ConfigureMartenIdentityMapping();
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});
```

3. Add the MartenIdentityStores
```c#
builder.Services.AddMartenIdentityStores();
```

4. Configure the default identity to be MartenIdentityUser.
```c#
builder.Services.AddDefaultIdentity<MartenIdentityUser>();
```
