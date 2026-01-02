$files = Get-ChildItem -Path $PSScriptRoot -Filter "*.resx"

foreach ($file in $files) {
    Write-Host "`nChecking $($file.Name)..."
    [xml]$xml = Get-Content $file.FullName -Raw
    
    $dataElements = $xml.root.data
    $names = @()
    
    foreach ($d in $dataElements) {
        $names += $d.name
    }
    
    $grouped = $names | Group-Object | Where-Object { $_.Count -gt 1 }
    
    if ($grouped) {
        foreach ($g in $grouped) {
            Write-Host "  DUPLICATE: '$($g.Name)' appears $($g.Count) times"
        }
    }
    else {
        Write-Host "  No duplicates found"
    }
}
