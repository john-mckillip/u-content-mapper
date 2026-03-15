# UContentMapper

UContentMapper is a modern, flexible .NET library for mapping content models, designed to simplify and automate the mapping of Umbraco CMS content and other .NET objects. It provides a robust, testable infrastructure for mapping, conversion, and configuration, supporting advanced scenarios and integration with Umbraco 15 and beyond. Inspired by Andy Butland's [Anaximapper](https://github.com/AndyButland/Anaximapper), which works for lower versions of Umbraco.

## Features

- **Automated Content Mapping**: Map Umbraco content to strongly-typed .NET models with minimal configuration.
- **Attribute-Based Mapping**: Use attributes to control mapping behavior, property conversion, and content type aliases.
- **Extensible Configuration**: Customize mapping logic via configuration classes and interfaces.
- **Integration with Dependency Injection**: Register mappers and configurations with DI containers for scalable applications.
- **Comprehensive Testing**: 100% code coverage, including unit, integration, and edge case tests.
- **Mocking Infrastructure**: Built-in mocks for Umbraco content and elements for reliable test scenarios.
- **Coverage Reporting**: Integrated with Coverlet and Codecov for coverage enforcement and reporting.
- **CI/CD Pipeline**: Automated testing, coverage, security scanning, packaging, and tag-driven NuGet deployment via GitHub Actions.

## Installation

Install the core library and Umbraco integration via NuGet:

```sh
# Core library
 dotnet add package UContentMapper.Core
# Umbraco 15 integration
 dotnet add package UContentMapper.Umbraco15
```

## Usage

### 1. Define Content Models

Annotate your models with mapping attributes:

```csharp
using UContentMapper.Core.Attributes;

[ContentTypeAlias("page")]
public class PageModel : BaseContentModel
{
    [MapFrom("title")]
    public string Title { get; set; }

    [MapFrom("body")]
    public string Body { get; set; }
}
```

### 2. Configure Mapping

Create a mapping configuration class if you need custom logic:

```csharp
public class PageMappingConfiguration : MappingConfigurationBase<PageModel>
{
    public override void Configure()
    {
        Map(p => p.Title).From("title");
        Map(p => p.Body).From("body");
    }
}
```

### 3. Register Services (Umbraco 15 Example)

Register mappers and configurations in your DI container:

```csharp
services.AddUContentMapper(options =>
{
    options.AddConfiguration<PageMappingConfiguration>();
});
```

### 4. Map Content

Use the mapper to convert Umbraco content to your models:

```csharp
var pageModel = mapper.Map<PageModel>(umbracoContent);
```

### 5. Advanced Scenarios

- **Type Conversion**: Automatic conversion for supported types.
- **Error Handling**: Custom exceptions for mapping failures.
- **Integration Testing**: Use provided mocks for Umbraco content in tests.

## Testing

- All public methods, properties, and constructors are covered by unit and integration tests.
- Mock infrastructure for Umbraco content and elements.
- Parametrized tests for multiple mapping scenarios.
- In-memory database support for future enhancements.

Run tests locally:

```sh
dotnet test --collect:"XPlat Code Coverage"
```

## CI/CD and Coverage

- Automated pipeline with stages for testing, building, security scanning, quality gate, and deployment.
- Coverage enforcement (90% minimum in CI, 100% target locally).
- Codecov integration for coverage tracking and PR comments.
- NuGet publish is tag-driven: create a `vX.Y.Z` git tag to publish release packages.

## Release and Package Strategy

- `UContentMapper.Core` is the stable core package.
- Umbraco integration is versioned per major, starting with `UContentMapper.Umbraco15`.
- New Umbraco majors should ship as new adapter packages (for example, `UContentMapper.Umbraco16` and `UContentMapper.Umbraco17`).

Create a release package version by tagging:

```sh
git tag v1.0.0
git push origin v1.0.0
```

Detailed release steps are in [docs/RELEASE_CHECKLIST.md](docs/RELEASE_CHECKLIST.md).

For future Umbraco majors, use the scaffolds in [templates/UContentMapper.Umbraco16](templates/UContentMapper.Umbraco16) and [templates/UContentMapper.Umbraco17](templates/UContentMapper.Umbraco17).

## Contributing

Contributions are welcome! Please see `CONTRIBUTING.md` for guidelines. All code must be covered by tests and pass CI/CD quality gates.

## License

This project is licensed under the MIT License. See `LICENSE` for details.

---

For more details, see `TEST_IMPLEMENTATION_SUMMARY.md` and `TESTING.md`.
