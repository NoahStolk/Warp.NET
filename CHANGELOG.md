# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## TODO

### Changed

- The game class generator now generates the game constructor and the static `Construct` method.
- `Base` classes are no longer required. The game instance can now be accessed using the `Game.Self` property.

### Fixed

- Fixed not always generating required types.

## 0.1.1

### Added

- Added `Model.MainMesh` property.

### Changed

- Reflection is no longer used to assign content.
- `Bootstrapper.CreateGame` now requires the content container types as type parameters.

### Fixed

- Fixed incorrect namespaces in source-generated code.

### Removed

- Removed NoahStolk.WaveParser dependency.

## 0.1.0

- Initial alpha release.
