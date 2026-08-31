$root = "C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK"
$results = @()

Get-ChildItem -Path $root -Recurse -Filter *.cs |
  Where-Object { $_.FullName -notmatch '\\_tools\\' } |
  ForEach-Object {
    $file = $_.FullName
    $rel = $file.Substring($root.Length + 1)
    $lines = Get-Content $file
    $content = Get-Content $file -Raw

    for ($i = 0; $i -lt $lines.Count; $i++) {
      if ($lines[$i] -match '\[Obsolete\(') {
        # Collect multi-line Obsolete attribute
        $attr = $lines[$i]
        $j = $i
        while ($attr -notmatch '\]' -and $j -lt ($lines.Count - 1)) {
          $j++
          $attr += " " + $lines[$j].Trim()
        }

        $sch = "?"
        if ($attr -match 'tipo (SmartCoreHub\.Core\.SDK\.[A-Za-z0-9_.]+)') {
          $sch = $Matches[1].TrimEnd('.')
        }

        # Find type declaration after Obsolete
        $kind = "?"
        $name = "?"
        $isStatic = $false
        $generics = ""
        $hasNetIf = $false
        for ($k = [Math]::Max(0, $i - 15); $k -lt [Math]::Min($lines.Count, $j + 20); $k++) {
          if ($lines[$k] -match '#if\s+NET8_0_OR_GREATER') { $hasNetIf = $true }
          if ($lines[$k] -match '^\s*(?:public|internal)\s+(static\s+)?(?:abstract\s+)?(?:sealed\s+)?(?:partial\s+)?(interface|class|enum|record)\s+(\w+)(<[^>]+>)?') {
            $isStatic = [bool]$Matches[1]
            $kind = $Matches[2]
            $name = $Matches[3]
            $generics = if ($Matches[4]) { $Matches[4] } else { "" }
            # Prefer the type right after this Obsolete (skip earlier ones)
            if ($k -ge $i) { break }
          }
        }

        # Namespace
        $ns = "?"
        if ($content -match 'namespace\s+([\w.]+)') { $ns = $Matches[1] }

        $results += [PSCustomObject]@{
          File = $rel
          Namespace = $ns
          Kind = $kind
          Static = $isStatic
          Name = $name
          Generics = $generics
          Sch = $sch
          Net8 = $hasNetIf
        }
      }
    }
  }

$results | Sort-Object File, Name | Format-Table -AutoSize
Write-Host "Total: $($results.Count)"
$results | ConvertTo-Csv -NoTypeInformation | Set-Content "$root\_tools\obsolete-inventory.csv"
Write-Host "Wrote $root\_tools\obsolete-inventory.csv"
