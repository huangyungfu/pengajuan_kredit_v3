using KreditService.DTOs;
using Xunit;
using KreditService.Data; 

namespace KreditService.Tests;

public class KreditTests
{
    [Fact]
    public void Test_ShouldFail_WhenBungaExceedsOneHundredPercent()
    {
        // Scenario 1: Interest rate upper limit check
        var result = KreditValidator.ValidateCoreMetrics(10000000, 101.00m, 12);
        Assert.Equal("Bunga must be greater than 0 and maximum 100.", result);
    }

    [Fact]
    public void Test_ShouldFail_WhenPlafonIsNegativeOrZero()
    {
        // Scenario 2: Principle minimum volume boundary check
        var result = KreditValidator.ValidateCoreMetrics(0, 10m, 12);
        Assert.Equal("Plafon must be greater than 0.", result);
    }

    [Fact]
    public void Test_ShouldPass_WhenInputsAreEntirelyValid()
    {
        // Scenario 3: Standard workflow validation handling test
        var result = KreditValidator.ValidateCoreMetrics(75000000, 11.25m, 36);
        Assert.Null(result);
    }
}