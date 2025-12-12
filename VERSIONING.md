# Versioning Strategy

This project uses automatic branch-based versioning for manual deployments and tagged releases.

## Version Formats

### Develop Branch (Alpha Packages)
- **Format**: `{BASE_VERSION}-alpha.{COMMIT_COUNT}+{SHORT_SHA}`
- **Example**: `1.0.0-alpha.42+a1b2c3d`
- **Trigger**: Manual workflow dispatch from `develop` branch
- **Purpose**: Pre-release testing and validation

### main Branch (Release Packages)
- **Format**: `{BASE_VERSION}.{COMMIT_COUNT}`
- **Example**: `1.0.0.123`
- **Trigger**: Manual workflow dispatch from `main` branch
- **Purpose**: Stable production releases

### Tagged Releases
- **Format**: Version from git tag (e.g., `v1.2.0` → `1.2.0`)
- **Trigger**: GitHub release published
- **Purpose**: Official versioned releases

## How It Works

1. **CI Workflow** runs on every push to `develop` or `main` branches
   - Builds the project
   - Runs all tests
   - Generates code coverage reports

2. **CD Workflow** runs manually or on GitHub release
   - Manually trigger from GitHub Actions tab
   - Select branch type (main for stable, develop for alpha)
   - Optionally specify a custom version
   - Automatically calculates the version based on branch selection
   - Builds and packs the NuGet package
   - Publishes to NuGet.org and GitHub Packages

## Publishing a Package

### Manual Workflow Dispatch

1. Go to [GitHub Actions → CD - Publish NuGet Packages](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml)
2. Click "Run workflow" button
3. **Select the branch** from the dropdown:
   - Choose `main` for stable release
   - Choose `develop` for alpha pre-release
4. Click "Run workflow"
5. The version is **automatically generated** based on the selected branch

**No manual version input needed!** The workflow automatically:
- Counts commits in the branch
- Generates the version format based on branch name
- For `develop`: adds alpha suffix and commit SHA
- For `main`: creates stable version number

### GitHub Release

1. Create a new release on GitHub
2. Tag it with version (e.g., `v1.2.0`)
3. Publish the release
4. CD workflow automatically triggers and publishes the package with that version

## NuGet Version Precedence

According to [SemVer 2.0.0](https://semver.org/):
- **Release versions** (e.g., `1.0.0.123`) are considered stable
- **Pre-release versions** (e.g., `1.0.0-alpha.42`) are considered unstable
- Users installing without version constraints will get the latest stable version (from main)
- Users can opt-in to alpha versions with: `dotnet add package Finova --version 1.0.0-alpha.*`

## Updating Base Version

To update the base version:
1. Edit `BASE_VERSION` in `.github/workflows/cd.yml` (line ~38)
2. Update `<Version>` in `src/Finova.Belgium/Finova.Belgium.csproj`

## Example Workflow

```bash
# Work on feature
git checkout develop
git commit -m "Add new feature"
git push origin develop

# Run CI (automatic)
# CI builds and tests the code

# Publish alpha version (manual)
# Go to GitHub Actions → CD workflow
# Click "Run workflow"
# Select branch: develop
# → Publishes: 1.0.0-alpha.42+a1b2c3d

# Merge to main for release
git checkout main
git merge develop
git push origin main

# Run CI (automatic)
# CI builds and tests the code

# Publish stable version (manual)
# Go to GitHub Actions → CD workflow
# Click "Run workflow"
# Select branch: main
# → Publishes: 1.0.0.43

# Create official release (optional)
git tag v1.1.0
git push origin v1.1.0
# Create GitHub release with tag v1.1.0
# → Automatically publishes: 1.1.0
```
