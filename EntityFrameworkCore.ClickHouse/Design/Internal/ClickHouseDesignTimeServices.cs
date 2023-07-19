using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouse.EntityFrameworkCore.Migrations.Design;
using ClickHouse.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ClickHouse.EntityFrameworkCore.Design.Internal;

public class ClickHouseDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        Debugger.Launch();
        Console.WriteLine("IDesignTimeServices runned");
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddEntityFrameworkClickHouse()
            .AddSingleton<IAnnotationCodeGenerator, ClickHouseAnnotationCodeGenerator>()
            .AddSingleton<IDatabaseModelFactory, ClickHouseDatabaseModelFactory>()
            .AddSingleton<ICSharpHelper, ClickHouseCSharpHelper>()
            .AddSingleton<AnnotationCodeGeneratorDependencies, AnnotationCodeGeneratorDependencies>()
            .AddSingleton<ICSharpMigrationOperationGenerator, ClickHouseCSharpMigrationOperationGenerator>()
            .AddSingleton<IMigrationsCodeGenerator, ClickHouseCSharpMigrationsGenerator>()
            ;
    }
}
