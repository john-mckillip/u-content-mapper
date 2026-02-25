# UContentMapper.Umbraco16 Starter Template

This folder is a starter scaffold for the next Umbraco adapter package.

## How to use

1. Copy this folder to repository root as `UContentMapper.Umbraco16`.
2. Add the project to `UContentMapper.sln`.
3. Implement Umbraco 16 mappings and service registrations by adapting from `UContentMapper.Umbraco15`.
4. Add/adjust tests under `UContentMapper.Tests/Unit/Umbraco16`.
5. Add CI pack step for `UContentMapper.Umbraco16` in `.github/workflows/ci.yml`.

## Notes

- The project is intentionally not part of the solution yet.
- `Umbraco.Cms.*` versions are placeholders and must be aligned with your selected Umbraco 16 release.
