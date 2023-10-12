var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<MoneyTransferOptions>(builder.Configuration.GetSection(nameof(MoneyTransferOptions)));

// Hook up driven ports and implemented adapters, will automatically be scaned by Scrutor library
builder.Services.Scan(selector =>
{
    selector.FromAssemblies(
            Me.Acheddir.Hexagonal.Persistence.AssemblyReference.Assembly,
            Me.Acheddir.Hexagonal.Application.AssemblyReference.Assembly)
        .AddClasses(classes => classes.AssignableTo<IDrivenPort>())
        .AsImplementedInterfaces()
        .WithScopedLifetime();
});

// MediatR library is handling the driving side of the hexagon using Mediator and CQRS patterns
builder.Services.AddMediatR(config =>
{
    // Automatically scans commands/queries and their respective handlers
    config.RegisterServicesFromAssembly(Me.Acheddir.Hexagonal.Application.AssemblyReference.Assembly);
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