# Replace [Obsolete] on wrappers with [SdkWrappedSource] across HotelWise.Core.SDK.
# PRE-REQUISITE: SmartCoreHub.Core.SDK NuGet must include SdkWrappedSourceAttribute
# (SmartCoreHub.Core.SDK.Common.Attributes). Do NOT run until a new package version is published.
param(
  [string]$Path = "C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK"
)

$files = Get-ChildItem -Path $Path -Recurse -Filter *.cs |
  Where-Object { $_.FullName -notmatch '\\_tools\\' -and (Select-String -Path $_.FullName -Pattern '\[Obsolete\(' -Quiet) }

$updatedCount = 0

foreach ($file in $files) {
  $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.UTF8Encoding]::new($false))
  
  # Pattern matching multiline [Obsolete("...")]
  $pattern = '(?s)\[Obsolete\("([^"]*tipo\s+([\w\.<>]+)[^"]*)"\)\]'
  
  if ($content -match $pattern) {
    $newContent = [System.Text.RegularExpressions.Regex]::Replace($content, $pattern, {
      param($m)
      $targetType = $m.Groups[2].Value.TrimEnd('.')
      return "[SdkWrappedSource(targetType: `"$targetType`", targetPackage: `"SmartCoreHub.Core.SDK`", description: `"Casca/wrapper delegando para $targetType em SmartCoreHub.Core.SDK.`")]"
    })

    # Also check generic fallback [Obsolete("...")] if any didn't have 'tipo'
    $fallbackPattern = '(?s)\[Obsolete\("([^"]*)"\)\]'
    $newContent = [System.Text.RegularExpressions.Regex]::Replace($newContent, $fallbackPattern, {
      param($m)
      $desc = $m.Groups[1].Value.Replace("`r`n", " ").Replace("`n", " ")
      return "[SdkWrappedSource(targetType: `"SmartCoreHub.Core.SDK`", targetPackage: `"SmartCoreHub.Core.SDK`", description: `"$desc`")]"
    })

    # Ensure using SmartCoreHub.Core.SDK.Common.Attributes; is present if not already
    if ($newContent -notmatch 'using\s+SmartCoreHub\.Core\.SDK\.Common\.Attributes;') {
      if ($newContent -match '(?m)^namespace\s+') {
        $newContent = [System.Text.RegularExpressions.Regex]::Replace($newContent, '(?m)^namespace\s+', "using SmartCoreHub.Core.SDK.Common.Attributes;`r`n`r`nnamespace ")
      }
    }

    [System.IO.File]::WriteAllText($file.FullName, $newContent, [System.Text.UTF8Encoding]::new($false))
    Write-Output "Updated: $($file.Name)"
    $updatedCount++
  }
}

Write-Output "Finished. Total files updated: $updatedCount"
