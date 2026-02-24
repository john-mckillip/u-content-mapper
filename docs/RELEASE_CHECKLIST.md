# UContentMapper Release Checklist

This checklist is the source of truth for publishing NuGet releases.

## 1) One-time repository setup

- Ensure `NUGET_API_KEY` is configured in repository secrets.
- Ensure package ownership on NuGet.org includes the publishing account.
- Verify repository URL metadata in `Directory.Build.props` is correct.

## 2) Pre-release validation

- Confirm `main` is green in CI.
- Run local validation:

```sh
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

- Verify pack output locally:

```sh
dotnet pack UContentMapper.Core/UContentMapper.Core.csproj --configuration Release --output ./packages /p:PackageVersion=0.0.0-rc.1
dotnet pack UContentMapper.Umbraco15/UContentMapper.Umbraco15.csproj --configuration Release --output ./packages /p:PackageVersion=0.0.0-rc.1
```

- Confirm generated artifacts include:
  - `UContentMapper.Core.*.nupkg`
  - `UContentMapper.Core.*.snupkg`
  - `UContentMapper.Umbraco15.*.nupkg`
  - `UContentMapper.Umbraco15.*.snupkg`

## 3) Release versioning

- Use semantic versioning tags in format `vX.Y.Z`.
- Patch release: `vX.Y.(Z+1)`
- Minor release: `vX.(Y+1).0`
- Major release: `v(X+1).0.0`

## 4) Publish release

Create and push a release tag:

```sh
git checkout main
git pull
git tag v1.0.0
git push origin v1.0.0
```

The GitHub workflow in `.github/workflows/ci.yml` will:

- build and test,
- pack with tag-derived `PackageVersion`,
- upload package artifacts,
- push `.nupkg` and `.snupkg` to NuGet.

## 5) Post-release verification

- Validate package availability on NuGet.org.
- Validate symbols package availability.
- Install smoke test:

```sh
dotnet new console -n UContentMapper.ReleaseSmoke
cd UContentMapper.ReleaseSmoke
dotnet add package UContentMapper.Core --version 1.0.0
dotnet add package UContentMapper.Umbraco15 --version 1.0.0
dotnet restore
dotnet build
```

- Add release notes summarizing changed behavior.

## 6) Rollback guidance

- Do not delete published NuGet versions.
- Publish a new patch version with the fix.
- If necessary, unlist the problematic version on NuGet.

## 7) New Umbraco major onboarding

When adding support for a new Umbraco major, keep adapter packages versioned by major:

- `UContentMapper.Umbraco15`
- `UContentMapper.Umbraco16`
- `UContentMapper.Umbraco17`

Use the starter scaffold in `templates/UContentMapper.Umbraco16` or `templates/UContentMapper.Umbraco17` and then:

1. Copy scaffold into a new top-level project folder.
2. Update package references for the target Umbraco major.
3. Implement and verify mappings/resolvers against the new APIs.
4. Add project to `UContentMapper.sln`.
5. Add/update tests in `UContentMapper.Tests/Unit/UmbracoXX`.
6. Add new `dotnet pack` line in CI workflow.
7. Update README compatibility and installation guidance.

Framework guidance:

- Umbraco 16 template is currently scaffolded on `net9.0`.
- Umbraco 17 template is currently scaffolded on `net10.0`.
