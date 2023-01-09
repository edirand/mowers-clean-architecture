﻿using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Mowers.CleanArchitecture.Api.IntegrationTests.Base;
using Mowers.CleanArchitecture.Api.Models;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Api.IntegrationTests.Controllers;

public class FilesControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public FilesControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task ReturnsSuccessfulResult()
    {
        var client = _factory.CreateClient();
        var instructionFile = File.OpenRead("Samples/SampleInstructionFile.txt");
        var expectedResultFile = File.ReadLines("Samples/SampleInstructionFileExpectedResult.txt");
        var response = await client.PostAsync("/api/files", new MultipartFormDataContent
        {
            { new StreamContent(instructionFile), "file", "InstructionFile.txt" }
        });

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var result = Deserialize<FileProcessingResult>(responseBody);

        result.ShouldNotBeNull();
        result.Mowers.ShouldBe(expectedResultFile);
    }

    private static T Deserialize<T>(string content) => JsonSerializer.Deserialize<T>(content,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
}