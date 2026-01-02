$basePath = "c:\Users\flori\source\repos\Finova\Finova Nuget\src\Finova.Core\Resources"
$files = @{
    "en" = "$basePath\ValidationMessages.resx";
    "fr" = "$basePath\ValidationMessages.fr.resx";
    "de" = "$basePath\ValidationMessages.de.resx";
    "nl" = "$basePath\ValidationMessages.nl.resx"
}

$data = @{}

# 1. Load and Validate XML
Write-Host "--- XML Validation ---"
foreach ($lang in $files.Keys) {
    try {
        [xml]$xml = Get-Content $files[$lang]
        $data[$lang] = @{}
        foreach ($node in $xml.root.data) {
            $val = if ($node.value) { $node.value.Trim() } else { "" }
            $data[$lang][$node.name] = $val
        }
        Write-Host "[$lang] XML Valid. Keys found: $($data[$lang].Count)"
    }
    catch {
        Write-Host "[$lang] XML INVALID: $_"
        exit 1
    }
}

# 2. Check for Missing Keys (vs English)
Write-Host "`n--- Missing Keys Check (Base: English) ---"
$enKeys = $data["en"].Keys
foreach ($lang in $files.Keys) {
    if ($lang -eq "en") { continue }
    
    $missing = $enKeys | Where-Object { -not $data[$lang].ContainsKey($_) }
    if ($missing) {
        Write-Host "[$lang] Missing Keys ($($missing.Count)):"
        $missing | ForEach-Object { Write-Host "  - $_" }
    }
    else {
        Write-Host "[$lang] All English keys present."
    }
}

# 3. Check for Encoding Issues (Common Mojibake)
Write-Host "`n--- Encoding Check ---"
$encodingRegex = [regex]"Ã"
foreach ($lang in $files.Keys) {
    $issues = 0
    foreach ($key in $data[$lang].Keys) {
        $val = $data[$lang][$key]
        if ($val -match $encodingRegex) {
            Write-Host "[$lang] Encoding Issue in '$key': $val"
            $issues++
        }
    }
    if ($issues -eq 0) { Write-Host "[$lang] No encoding issues found." }
}

# 4. Check for Potential Untranslated Strings (Value == English)
Write-Host "`n--- Potential Untranslated Strings (Value == English) ---"
foreach ($lang in $files.Keys) {
    if ($lang -eq "en") { continue }
    
    $untranslated = 0
    foreach ($key in $data[$lang].Keys) {
        if ($data["en"].ContainsKey($key)) {
            $enVal = $data["en"][$key]
            $localVal = $data[$lang][$key]
            
            # Ignore short values or identical codes (e.g. "IBAN")
            if ($enVal -eq $localVal -and $enVal.Length -gt 5) {
                Write-Host "[$lang] Potential Untranslated '$key': '$localVal'"
                $untranslated++
            }
        }
    }
    if ($untranslated -eq 0) { Write-Host "[$lang] No obvious untranslated strings found." }
}
