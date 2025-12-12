# NuGet Package Metadata

This document describes all the metadata included in the Finova NuGet package.

## 📦 Package Information

When users view your package on [NuGet.org](https://www.nuget.org/packages/Finova/), they will see:

### Basic Information
- **Package ID**: `Finova`
- **Version**: Automatically generated (e.g., `1.0.0.123` or `1.0.0-alpha.10`)
- **Authors**: Florian Di Vrusa
- **License**: MIT
- **Project URL**: [https://github.com/fdivrusa/Finova](https://github.com/fdivrusa/Finova)

### Description
Full package description explaining features and capabilities.

### Tags
The following tags help users find your package:
- banking
- belgium
- ogm
- vcs
- payment
- payment-reference
- structured-communication
- ISO11649
- IBAN
- modulo97
- financial
- fintech
- belgian-banking
- fluentvalidation

## 📄 Included Files

### README.md
- **Location**: Root of package
- **Source**: `README.md` from repository root
- **Display**: Automatically shown on NuGet.org package page
- **Content**: Full documentation with examples, usage, and API reference

### Package Icon (Optional)
- **Location**: Root of package
- **File**: `icon.png` (128x128 or 256x256 recommended)
- **Display**: Shown as package thumbnail on NuGet.org
- **Status**: Will be included if `icon.png` exists in repository root

**To add an icon:**
1. Create a 128x128 or 256x256 PNG image
2. Save it as `icon.png` in the repository root
3. Commit and push
4. Next package publish will include it

### Symbol Package (.snupkg)
- **Purpose**: Debugging support
- **Content**: PDB files for source-level debugging
- **Upload**: Automatically uploaded with main package
- **Usage**: Enables "Step Into" for your library code in Visual Studio

## 🔗 Links in Package

### Project URL
Links to GitHub repository homepage.

### Repository
Direct link to source code with:
- Repository URL: `https://github.com/fdivrusa/Finova`
- Repository Type: `git`
- Commit Hash: Embedded in symbol package

### Release Notes
Points to: `https://github.com/fdivrusa/Finova/releases`

Users can click to see all release notes and changelog.

## 📊 What Users See on NuGet.org

When viewing your package, users will see:

1. **Overview Tab**
   - Package ID and version
   - Authors and license
   - Download count
   - Full README (rendered from README.md)

2. **Dependencies Tab**
   - `Microsoft.Extensions.DependencyInjection` (10.0.0+)
   - `FluentValidation` (11.0.0+)
   - Target framework (.NET 10.0)

3. **Versions Tab**
   - All published versions (stable and pre-release)
   - Release dates
   - Download counts per version

4. **Statistics Tab**
   - Total downloads
   - Download trends
   - Version distribution

5. **Used By Tab**
   - Projects and packages depending on yours

## 🔍 Source Link Support

Your package includes Source Link support:
- **PublishRepositoryUrl**: `true`
- **EmbedUntrackedSources**: `true`
- **IncludeSymbols**: `true`

This means:
- Developers can "Step Into" your code while debugging
- Source code locations are embedded in PDB files
- Links directly to GitHub source files
- Works with Visual Studio, VS Code, and Rider

## 💡 Best Practices Implemented

✅ **README included** - Full documentation in package
✅ **License declared** - MIT license clearly specified
✅ **Repository linked** - Easy access to source code
✅ **Tags comprehensive** - Easy to find via search
✅ **Symbols included** - Debug support for consumers
✅ **Source Link enabled** - Direct source code access
✅ **Semantic versioning** - Clear version numbering
✅ **Release notes linked** - Easy to see what changed

## 🎯 Package Quality Score

NuGet.org evaluates packages on:
- ✅ Has README
- ✅ Has license
- ✅ Has project URL
- ✅ Has repository URL
- ✅ Has tags
- ✅ Has symbols
- ✅ Has release notes link
- ⏳ Has icon (optional - add icon.png)

Your package scores highly on NuGet's quality metrics!

## 📝 Updating Metadata

To update package metadata:

1. Edit `src/Finova.Belgium/Finova.Belgium.csproj`
2. Modify the properties in the `PropertyGroup` section
3. Commit and push changes
4. Next package publish will include updated metadata

Common updates:
- **Description**: Update feature descriptions
- **Tags**: Add/remove search tags
- **Authors**: Add contributors
- **Copyright**: Update year
- **PackageReleaseNotes**: Point to specific release
