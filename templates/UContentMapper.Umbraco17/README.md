# UContentMapper.Umbraco17 Starter Template

This folder is a starter scaffold for the Umbraco 17 adapter package.

## How to use

1. Copy this folder to repository root as `UContentMapper.Umbraco17`.
2. Add the project to `UContentMapper.sln`.
3. Implement Umbraco 17 mappings and service registrations by adapting from `UContentMapper.Umbraco15` or `UContentMapper.Umbraco16`.
4. Add/adjust tests under `UContentMapper.Tests/Unit/Umbraco17`.
5. Add CI pack step for `UContentMapper.Umbraco17` in `.github/workflows/ci.yml`.

## Notes

- This starter targets `.NET 10` (`net10.0`) by default; ensure your local SDK and CI/tooling are updated to .NET 10, or retarget the project to `.NET 9.0` (`net9.0`) to match the current repo before adding it to the solution and CI.
- The project is intentionally not part of the solution yet.
- `Umbraco.Cms.*` versions are placeholders and must be aligned with your selected Umbraco 17 release.
