# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.1.10

### Added

- Added color parameters to `IconButton` component.

## 0.1.9

### Added

- Added `Blob` content type.

## 0.1.8

### Added

- Added additional debug information to `NestingContext.UpdateQueue` exceptions.

## 0.1.7

### Added

- Added `Bounds.CreateNested(float x, float y)` overload.
- Added optional `Random` argument to `QuaternionExtensions.Randomize` method.
- Re-added `IBounds`.
- Re-added `ViewportState`.
- Added `PixelBounds`.

### Changed

- Changed `ScrollViewer` constructor and added `scrollbarWidth` parameter.
- Renamed `Bounds` to `NormalizedBounds`.

### Fixed

- Fixed not returning in `Label.Render` when `Text` was empty.
- Fixed not setting `CharWidth` and `TextRenderingHorizontalOffset` in `AbstractTextInput`. The properties are now abstract.

### Removed

- Removed `RandomUtils`; use `Random.Shared` with `RandomExtensions` instead.

## 0.1.6

### Added

- Added `ScrollViewer` component to `RenderImpl.Ui`.
- Added `Graphics.SetWindowSizeLimits`.
- Added `Bounds` class.

### Changed

- Changed the way UI positioning is resolved. Use `Bounds.CreateNested` for nested components.

### Removed

- Removed `IBounds`.
- Removed `GameParameters`.
- Removed `Fraction`.
- Removed `Grid`.
- Removed `Rectangle`; use `Bounds` instead.
- Removed `ViewportState`.
- Removed `Bootstrapper.CreateWindow`; call `Graphics.CreateWindow` instead.
- Removed `GameBase.InitialWindowTitle`, `GameBase.InitialWindowWidth`, `GameBase.InitialWindowHeight`, `GameBase.InitialWindowFullScreen`; use `Graphics.CurrentWindowState` instead.

## 0.1.5

### Added

- Added `MonoSpaceFontRenderer.Font` property.
- Added `MonoSpaceFont.CharWidth` property.

### Changed

- Rewrote the `Bootstrapper` class:
  - There are now 3 methods:
    - `CreateWindow`
    - `GetDecompiledContent`
    - `CreateGame`
  - The shader uniform and content type parameters have been removed from `CreateGame`. The shader uniforms and content containers now need to be initialized manually using the `GetDecompiledContent` method. This allows for multiple content files and flexible content initialization.

## 0.1.4

### Added

- Added `Warp.NET.RenderImpl.Ui` library which contains a basic implementation of UI rendering, including shaders and fonts.

### Changed

- The base class `GameBase` is no longer added to the generated game code and now needs to be defined explicitly. This allows for different game base classes.

### Fixed

- Added missing using to generated game class.

### Removed

- Removed `IBounds.Move` static method.

## 0.1.3

### Added

- Added `Charset` content type.
- Added `TextAlign` enum.
- Added more default colors:
  - `HalfTransparentWhite` (1, 1, 1, 0.5)
  - `HalfTransparentBlack` (0, 0, 0, 0.5)
  - `Orange` (1, 0.5, 0, 1)

### Changed

- Renamed `Color.Transparent` to `Color.Invisible`.
- Changed `DebugStack.Add` signatures.

### Fixed

- `MonoSpaceFont.GetTextureOffset` now returns null when the character does not exist in the charset.

### Removed

- Removed `GenerateMenuAttribute` and generating menus; use `GenerateSingletonAttribute` instead.

## 0.1.2

### Added

- Added `GameParameters` record which holds the constructor parameters for game classes.

### Changed

- The game class generator now generates the static `Construct` method.
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
