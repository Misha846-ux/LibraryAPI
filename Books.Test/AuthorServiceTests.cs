using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.AuthorDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Application.Mapping;
using Books.Application.Service;
using Books.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static StackExchange.Redis.Role;
namespace Books.Tests;

public class AuthorServiceTests
{
    private readonly AuthorService _service;
    private readonly Mock<IAuthorRepository> _authorRepoMock;
    private readonly Mock<ICachingServices> _cacheMock;
    private readonly IMapper _mapper;
    private readonly ILoggerFactory _loggerFactory;


    public AuthorServiceTests()
    {
        _authorRepoMock = new Mock<IAuthorRepository>();
        _cacheMock = new Mock<ICachingServices>();

        _loggerFactory = LoggerFactory.Create(builder => { });

        // Налаштовуємо AutoMapper для простого тесту
        // Створюємо конфігурацію і додаємо твій профіль

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AuthorProfile>();
        }, _loggerFactory);

        // (опціонально) перевіряємо, що всі мапінги валідні
        config.AssertConfigurationIsValid();
        _mapper = new Mapper(config);
        _service = new AuthorService(_authorRepoMock.Object, _mapper, _cacheMock.Object);
    }

    [Fact]
    public async Task GetAllAuthorsAsync_ShouldReturnAuthors_FromCache_WhenCacheExists()
    {
        // Arrange: кеш вже містить авторів
        var cachedAuthors = new List<AuthorReadDto>
        {
            new AuthorReadDto { Id = 1, Name = "Author1" }
        };

        _cacheMock.Setup(c => c.GetAsync<ICollection<AuthorReadDto>>("Authors", new CancellationToken()))
                  .ReturnsAsync(cachedAuthors);

        // Act
        var result = await _service.GetAllAuthorsAsync(new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); //колекція містить лише 1 елемент
        Assert.Equal("Author1", result.First().Name);

        // Репозиторій не має викликатися, бо кеш є
        _authorRepoMock.Verify(r => r.GetAllAuthorsAsync(new CancellationToken()), Times.Never);
    }


    [Fact]
    public async Task GetAllAuthorsAsync_CacheEmpty_FetchesFromRepositoryAndSetsCache()
    {
        // Arrange
        var authorsFromRepo = new List<AuthorEntity>
        {
            new AuthorEntity { Id = 1, Name = "Author1" },
            new AuthorEntity { Id = 2, Name = "Author2" }
        };

        // Кеш порожній
        _cacheMock.Setup(c => c.GetAsync<ICollection<AuthorReadDto>>("Authors", new CancellationToken()))
                  .ReturnsAsync((ICollection<AuthorReadDto>)null);

        // Репозиторій повертає дані
        _authorRepoMock.Setup(r => r.GetAllAuthorsAsync(new CancellationToken()))
                       .ReturnsAsync(authorsFromRepo);

        var service = new AuthorService(_authorRepoMock.Object, _mapper, _cacheMock.Object);

        // Act
        var result = await service.GetAllAuthorsAsync(new CancellationToken());

        // Assert
        Assert.Equal(2, result.Count); // перевіряємо, що повернуло два елементи
        Assert.Contains(result, a => a.Name == "Author1");
        Assert.Contains(result, a => a.Name == "Author2");

        // Перевіряємо, що кеш було встановлено (TimesOnce перевірка, щоб метод був викликаний 1 раз)
        _cacheMock.Verify(c => c.SetAsync("Authors", It.IsAny<ICollection<AuthorReadDto>>(), null, new CancellationToken()), Times.Once);
    }
}
