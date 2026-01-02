
$files = @(
    @{
        Path = "c:\Users\flori\source\repos\Finova\Finova Nuget\src\Finova.Core\Resources\ValidationMessages.de.resx"
        Replacements = @{
            'Ã¼' = 'ü'
            'Ã¤' = 'ä'
            'Ã¶' = 'ö'
            'ÃŸ' = 'ß'
            'Ã„' = 'Ä'
            'Ã–' = 'Ö'
            'Ãœ' = 'Ü'
        }
    },
    @{
        Path = "c:\Users\flori\source\repos\Finova\Finova Nuget\src\Finova.Core\Resources\ValidationMessages.fr.resx"
        Replacements = @{
            'Ã©' = 'é'
            'Ã¨' = 'è'
            'Ãª' = 'ê'
            'Ã´' = 'ô'
            'Ã ' = 'à'
            'Ã§' = 'ç'
            'Ã¯' = 'ï'
            'Ã»' = 'û'
            'Ã®' = 'î'
            'Ã¢' = 'â'
        }
    }
)

foreach ($file in $files) {
    Write-Host "Processing $($file.Path)..."
    if (Test-Path $file.Path) {
        $content = Get-Content $file.Path -Raw
        foreach ($key in $file.Replacements.Keys) {
            $content = $content.Replace($key, $file.Replacements[$key])
        }
        [System.IO.File]::WriteAllText($file.Path, $content, [System.Text.Encoding]::UTF8)
        Write-Host "Done."
    } else {
        Write-Host "File not found: $($file.Path)"
    }
}
