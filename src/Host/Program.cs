var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BuckPalDatabase")));

builder.Services.AddAutoMapper(Me.Acheddir.Hexagonal.Persistence.AssemblyReference.Assembly);
builder.Services.AddValidatorsFromAssembly(Me.Acheddir.Hexagonal.Application.AssemblyReference.Assembly);

// MediatR library is handling the driving side of the hexagon using Mediator and CQRS patterns
builder.Services.AddMediatR(config =>
{
    // Automatically scans commands/queries and their respective handlers
    config.RegisterServicesFromAssembly(Me.Acheddir.Hexagonal.Application.AssemblyReference.Assembly);
    // Register pipelines
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services
    .Configure<MoneyTransferOptions>(builder.Configuration.GetSection(nameof(MoneyTransferOptions)));

// Hook up dependencies using Scrutor library
builder.Services.Scan(selector =>
{
    // Repositories
    selector.FromAssemblies(Me.Acheddir.Hexagonal.Persistence.AssemblyReference.Assembly)
        .AddClasses(classes => classes.AssignableTo<IRepository>())
        .AsMatchingInterface()
        .WithScopedLifetime();

    // Unit of works
    selector.FromAssemblies(Me.Acheddir.Hexagonal.Persistence.AssemblyReference.Assembly)
        .AddClasses(classes => classes.AssignableTo<IUnitOfWork>())
        .AsMatchingInterface()
        .WithScopedLifetime();

    // Driven ports
    selector.FromAssemblies(
            Me.Acheddir.Hexagonal.Persistence.AssemblyReference.Assembly,
            Me.Acheddir.Hexagonal.Application.AssemblyReference.Assembly)
        .AddClasses(classes => classes.AssignableTo<IDrivenPort>())
        .AsImplementedInterfaces()
        .WithScopedLifetime();
});

builder.Services
    .AddControllers()
    .AddApplicationPart(Me.Acheddir.Hexagonal.Rest.AssemblyReference.Assembly);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();