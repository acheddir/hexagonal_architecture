global using Xunit;
global using FluentAssertions;
global using Moq;

global using static Me.Acheddir.Hexagonal.UnitTests.Common.AccountTestData;
global using static Me.Acheddir.Hexagonal.UnitTests.Common.ActivityTestData;
global using Me.Acheddir.Hexagonal.Domain.Account;
global using Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;
global using Me.Acheddir.Hexagonal.Application.Ports.Driven;
