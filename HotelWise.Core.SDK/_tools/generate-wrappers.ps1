# Thin-wrapper generator: parses HW source (no SCH assembly load required).
param(
  [ValidateSet('contracts','statics','runtime','all')]
  [string]$Wave = 'all'
)

$ErrorActionPreference = 'Stop'
$hwRoot = "C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK"

function Classify-File([string]$RelPath) {
  $p = $RelPath -replace '\\','/'
  if ($p -match '^(Abstractions/|Common/|Security/Token(ConfigurationDto|VO)|AI/Abstractions/|AI/Configuration/|AI/DTO/|AI/Enums/|AI/Constants/|CoreSdkInfo)') {
    return 'contracts'
  }
  if ($p -match '^(Helpers/|Extensions/|Logging/|Validation/|AI/Helpers/|AI/Validation/)') {
    return 'statics'
  }
  return 'runtime'
}

function Get-Namespace([string]$Content) {
  if ($Content -match 'namespace\s+([\w.]+)') { return $Matches[1] }
  return $null
}

function Extract-ObsoleteTypes {
  param([string[]]$Lines)
  $blocks = @()
  $i = 0
  while ($i -lt $Lines.Count) {
    if ($Lines[$i] -match '^\s*\[Obsolete\(') {
      $attrStart = $i
      $j = $i
      while ($Lines[$j] -notmatch '\]\s*$' -and $j -lt $Lines.Count - 1) { $j++ }
      $attr = ($Lines[$attrStart..$j] -join "`n").Trim()
      $sch = if ($attr -match 'tipo (SmartCoreHub\.Core\.SDK\.[A-Za-z0-9_.]+)') { $Matches[1].TrimEnd('.') } else { $null }

      # xml doc above
      $docStart = $attrStart - 1
      while ($docStart -ge 0 -and ($Lines[$docStart] -match '^\s*///' -or $Lines[$docStart] -match '^\s*$' -or $Lines[$docStart] -match '^\s*\[JsonConverter' -or $Lines[$docStart] -match '^\s*\[Required')) {
        $docStart--
      }
      $docStart++
      while ($docStart -lt $attrStart -and $Lines[$docStart] -match '^\s*$') { $docStart++ }
      # Include attributes that are NOT Obsolete between docs and Obsolete (e.g. JsonConverter)
      $preAttrs = @()
      for ($pa = $docStart; $pa -lt $attrStart; $pa++) {
        if ($Lines[$pa] -match '^\s*\[' -and $Lines[$pa] -notmatch 'Obsolete') {
          $preAttrs += $Lines[$pa].Trim()
        }
      }
      $xmlDoc = @()
      for ($d = $docStart; $d -lt $attrStart; $d++) {
        if ($Lines[$d] -match '^\s*///') { $xmlDoc += $Lines[$d].TrimEnd() }
      }

      $k = $j + 1
      while ($k -lt $Lines.Count -and $Lines[$k] -match '^\s*$') { $k++ }
      if ($k -ge $Lines.Count) { $i = $j + 1; continue }
      if ($Lines[$k] -notmatch '^\s*(public|internal)\s+(static\s+)?(abstract\s+)?(sealed\s+)?(partial\s+)?(interface|class|enum|record)\s+(\w+)(<[^>]+>)?') {
        $i = $j + 1; continue
      }
      $isStatic = [bool]$Matches[2]
      $isAbstract = [bool]$Matches[3]
      $kind = $Matches[6]
      $name = $Matches[7]
      $generics = if ($Matches[8]) { $Matches[8] } else { "" }

      $bodyStart = $k
      while ($bodyStart -lt $Lines.Count -and $Lines[$bodyStart] -notmatch '\{') { $bodyStart++ }
      $depth = 0
      $bodyEnd = $bodyStart
      for ($b = $bodyStart; $b -lt $Lines.Count; $b++) {
        $depth += ([regex]::Matches($Lines[$b], '\{')).Count
        $depth -= ([regex]::Matches($Lines[$b], '\}')).Count
        if ($depth -eq 0 -and $b -ge $bodyStart) { $bodyEnd = $b; break }
      }

      $declLines = @()
      for ($d = $k; $d -le $bodyStart; $d++) { $declLines += $Lines[$d] }
      $constraints = @()
      $hwInterfaces = @()
      for ($d = $k; $d -le $bodyStart; $d++) {
        if ($d -ge $Lines.Count) { break }
        $line = $Lines[$d]
        if ($line -match '\bwhere\s+(.+)$') {
          $clause = "where " + $Matches[1].Trim()
          $clause = $clause -replace '\s*\{.*$',''
          $constraints += $clause.Trim()
        }
        # Capture HW interfaces from ": IFoo, IBar" (not SCH)
        if ($line -match '(?:interface|class)\s+\w+(?:<[^>]+>)?\s*:\s*(.+)$') {
          $bases = $Matches[1] -replace '\s*where\s+.*$','' -replace '\s*\{.*$',''
          foreach ($baseItem in ($bases -split ',')) {
            $baseItem = $baseItem.Trim()
            if ($baseItem -match '^I[A-Z]\w*' -and $baseItem -notmatch '^SmartCoreHub') {
              $hwInterfaces += $baseItem
            }
          }
        }
      }

      $bodyLines = @()
      if ($bodyEnd -gt $bodyStart) {
        $bodyLines = $Lines[($bodyStart)..$bodyEnd]
      }

      $blocks += [PSCustomObject]@{
        Attr = $attr
        PreAttrs = $preAttrs
        Sch = $sch
        Kind = $kind
        Name = $name
        Generics = $generics
        IsStatic = $isStatic
        IsAbstract = $isAbstract
        XmlDoc = $xmlDoc
        Constraints = $constraints
        HwInterfaces = $hwInterfaces
        BodyLines = $bodyLines
        DeclStart = $k
        BodyStart = $bodyStart
        BodyEnd = $bodyEnd
        FileStart = $(if ($xmlDoc.Count -gt 0) { $docStart } else { $attrStart })
      }
      $i = $bodyEnd + 1
      continue
    }
    $i++
  }
  return $blocks
}

function Extract-PublicCtors([string[]]$BodyLines, [string]$TypeName) {
  $text = $BodyLines -join "`n"
  $ctors = @()
  # Allow newline between ) and { ; also multi-line params
  $matches = [regex]::Matches($text, "(?s)public\s+$TypeName\s*\((.*?)\)\s*(?::\s*base\s*\((.*?)\))?\s*\{")
  foreach ($m in $matches) {
    $params = ($m.Groups[1].Value -replace '\s+', ' ').Trim()
    if ([string]::IsNullOrWhiteSpace($params)) { continue }
    $args = @()
    foreach ($part in ($params -split ',')) {
      $part = $part.Trim()
      if (-not $part) { continue }
      $withoutDefault = ($part -split '=')[0].Trim()
      $tokens = $withoutDefault -split '\s+'
      $args += $tokens[-1].TrimStart('@')
    }
    $ctors += [PSCustomObject]@{ Params = $params; Args = ($args -join ', ') }
  }
  return $ctors
}

function Extract-StaticMembers([string[]]$BodyLines, [string]$SchFqn) {
  $out = @()
  $text = $BodyLines -join "`n"
  # const fields - keep literals
  foreach ($m in [regex]::Matches($text, '(?m)^\s*public\s+const\s+(\S+)\s+(\w+)\s*=\s*([^;]+);')) {
    $out += "    public const $($m.Groups[1].Value) $($m.Groups[2].Value) = $($m.Groups[3].Value.Trim());"
  }
  # static readonly
  foreach ($m in [regex]::Matches($text, '(?m)^\s*public\s+static\s+readonly\s+(\S+)\s+(\w+)\s*=')) {
    $out += "    public static readonly $($m.Groups[1].Value) $($m.Groups[2].Value) = $SchFqn.$($m.Groups[2].Value);"
  }
  # static properties with get/set (simple)
  foreach ($m in [regex]::Matches($text, '(?m)^\s*public\s+static\s+(\S+)\s+(\w+)\s*\{\s*get;\s*set;\s*\}')) {
    $out += "    public static $($m.Groups[1].Value) $($m.Groups[2].Value) { get => $SchFqn.$($m.Groups[2].Value); set => $SchFqn.$($m.Groups[2].Value) = value; }"
  }
  foreach ($m in [regex]::Matches($text, '(?m)^\s*public\s+static\s+(\S+)\s+(\w+)\s*=>')) {
    $out += "    public static $($m.Groups[1].Value) $($m.Groups[2].Value) => $SchFqn.$($m.Groups[2].Value);"
  }
  # Methods: public static [async] ReturnType Name<...>(params)
  $methodMatches = [regex]::Matches($text, '(?m)^\s*public\s+static\s+(?:async\s+)?([\w.<>,\[\]\?]+)\s+(\w+)(<[^>]+>)?\s*\(([^)]*)\)')
  foreach ($m in $methodMatches) {
    $ret = $m.Groups[1].Value
    if ($ret -eq 'async') { continue }
    $name = $m.Groups[2].Value
    # skip if this looks like a property we already handled
    $gen = $m.Groups[3].Value
    $params = $m.Groups[4].Value.Trim()
    $args = @()
    if ($params) {
      foreach ($part in ($params -split ',')) {
        $part = $part.Trim()
        if (-not $part) { continue }
        $isThis = $part.StartsWith('this ')
        $p = $part -replace '^this\s+','' -replace '^out\s+','' -replace '^ref\s+','' -replace '^in\s+',''
        $withoutDefault = ($p -split '=')[0].Trim()
        $tokens = $withoutDefault -split '\s+'
        $pname = $tokens[-1].TrimStart('@')
        if ($part -match '^\s*out\s+') { $args += "out $pname" }
        elseif ($part -match '^\s*ref\s+') { $args += "ref $pname" }
        else { $args += $pname }
      }
    }
    $argStr = $args -join ', '
    # Preserve 'this' on first param for extensions
    $paramStr = $params
    if ($ret -eq 'void') {
      $out += "    public static void $name$gen($paramStr) =>`n        $SchFqn.$name$gen($argStr);"
    } else {
      $out += "    public static $ret $name$gen($paramStr) =>`n        $SchFqn.$name$gen($argStr);"
    }
  }
  return $out
}

function Build-WrapperType($b) {
  $doc = if ($b.XmlDoc.Count) { ($b.XmlDoc -join "`n") + "`n" } else { "" }
  $pre = if ($b.PreAttrs.Count) { ($b.PreAttrs -join "`n") + "`n" } else { "" }
  $attr = $b.Attr + "`n"
  $sch = $b.Sch
  $gen = $b.Generics
  $constraints = if ($b.Constraints.Count) { "`n    " + ($b.Constraints -join "`n    ") } else { "" }

  if ($b.Kind -eq 'enum') {
    # Keep original enum intact (from xml through body)
    return $null  # signal keep original slice
  }

  if ($b.Kind -eq 'interface') {
    return "${doc}${pre}${attr}public interface $($b.Name)$gen : $sch$gen$constraints`n{`n}"
  }

  if ($b.IsStatic) {
    $members = Extract-StaticMembers -BodyLines @($b.BodyLines) -SchFqn $sch
    $body = if ($members.Count) { ($members -join "`n`n") } else { "    // no public members detected" }
    return "${doc}${pre}${attr}public static class $($b.Name)`n{`n$body`n}"
  }

  # Sealed SCH types cannot be inherited — skip (restore originals after gen)
  if ($b.Name -in @('ApplicationIAConfig','RagConfig')) {
    return $null
  }

  # Special SearchCriteria
  if ($b.Name -eq 'SearchCriteria') {
    return @"
${doc}${pre}${attr}public class SearchCriteria : $sch
{
    /// <summary>Alias legado HW → MaxRetrieve.</summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve;
        set => MaxRetrieve = value;
    }
}
"@.TrimEnd()
  }

  $abs = if ($b.IsAbstract) { "abstract " } else { "" }
  $ctors = @(Extract-PublicCtors -BodyLines @($b.BodyLines) -TypeName $b.Name)
  # include protected ctors for abstract bases
  $textAll = (@($b.BodyLines) -join "`n")
  $prot = [regex]::Matches($textAll, "(?s)protected\s+$($b.Name)\s*\((.*?)\)\s*(?::\s*base\s*\((.*?)\))?\s*\{")
  foreach ($m in $prot) {
    $params = ($m.Groups[1].Value -replace '\s+', ' ').Trim()
    if ([string]::IsNullOrWhiteSpace($params)) { continue }
    $args = @()
    foreach ($part in ($params -split ',')) {
      $part = $part.Trim(); if (-not $part) { continue }
      $tokens = (($part -split '=')[0].Trim() -split '\s+')
      $args += $tokens[-1].TrimStart('@')
    }
    $ctors += [PSCustomObject]@{ Params = $params; Args = ($args -join ', '); Mod = 'protected' }
  }
  if ($b.Name -eq 'GroqApiAdapter') {
    Write-Host "DEBUG Groq BodyLines=$(@($b.BodyLines).Count) ctors=$($ctors.Count)"
  }
  $ctorText = ""
  if ($ctors.Count) {
    $ctorParts = @()
    foreach ($c in $ctors) {
      $mod = if ($c.Mod) { $c.Mod } else { "public" }
      $ctorParts += "    $mod $($b.Name)($($c.Params))`n        : base($($c.Args))`n    {`n    }"
    }
    $ctorText = ($ctorParts -join "`n`n")
  }

  # Also implement HW interfaces that were on the original declaration (for DI)
  $extraIfaces = ""
  if ($b.HwInterfaces -and @($b.HwInterfaces).Count -gt 0) {
    $extraIfaces = ", " + ((@($b.HwInterfaces) | Select-Object -Unique) -join ", ")
  }

  return "${doc}${pre}${attr}public ${abs}class $($b.Name)$gen : $sch$gen$extraIfaces$constraints`n{`n$ctorText`n}".TrimEnd() + "`n"
}

# ---------- main ----------
$files = Get-ChildItem $hwRoot -Recurse -Filter *.cs |
  Where-Object { $_.FullName -notmatch '\\_tools\\' -and (Select-String -Path $_.FullName -Pattern '\[Obsolete\(' -Quiet) }

$converted = 0; $skipped = 0; $errors = @()

foreach ($file in $files) {
  $rel = $file.FullName.Substring($hwRoot.Length + 1)
  $waveOf = Classify-File $rel
  if ($Wave -ne 'all' -and $waveOf -ne $Wave) { continue }

  $raw = [IO.File]::ReadAllText($file.FullName, [Text.UTF8Encoding]::new($false))
  $lines = $raw -split "`r?`n"
  $ns = Get-Namespace $raw
  $hasNet8 = $raw -match '(?m)^#if\s+NET8_0_OR_GREATER'
  $blocks = @(Extract-ObsoleteTypes $lines)
  if ($blocks.Count -eq 0) { $errors += "parse fail $rel"; continue }

  # enums-only file: leave untouched
  if (($blocks | Where-Object Kind -eq 'enum').Count -eq $blocks.Count) {
    Write-Host "SKIP enum: $rel"
    $skipped++
    continue
  }

  $parts = @()
  foreach ($b in $blocks) {
    if ($b.Kind -eq 'enum') {
      # keep original from FileStart through BodyEnd — approximate using XmlDoc+attr+enum body
      $slice = ($lines[$b.FileStart..$b.BodyEnd] -join "`n")
      $parts += $slice.TrimEnd()
      continue
    }
    $w = Build-WrapperType $b
    if ($null -eq $w) { $errors += "null wrapper $($b.Name) $rel"; continue }
    $parts += $w.TrimEnd()
  }

  $sb = New-Object System.Text.StringBuilder
  if ($hasNet8) { [void]$sb.AppendLine("#if NET8_0_OR_GREATER") }

  # Collect ALL usings (including after #if and HotelWise usings needed by ctors/signatures)
  $usingLines = @()
  foreach ($ln in $lines) {
    if ($ln -match '^\s*using\s+') {
      $usingLines += $ln.TrimEnd()
    }
    if ($ln -match '^\s*namespace\s+') { break }
  }
  # Deduplicate while preserving order
  $seen = New-Object 'System.Collections.Generic.HashSet[string]'
  $uniqueUsings = @()
  foreach ($u in $usingLines) {
    if ($seen.Add($u)) { $uniqueUsings += $u }
  }
  foreach ($u in $uniqueUsings) { [void]$sb.AppendLine($u) }
  if ($uniqueUsings.Count) { [void]$sb.AppendLine() }

  [void]$sb.AppendLine("namespace $ns;")
  [void]$sb.AppendLine()
  foreach ($p in $parts) {
    [void]$sb.AppendLine($p)
    [void]$sb.AppendLine()
  }
  if ($hasNet8) { [void]$sb.AppendLine("#endif") }

  [IO.File]::WriteAllText($file.FullName, $sb.ToString().TrimEnd() + "`r`n", [Text.UTF8Encoding]::new($false))
  Write-Host "OK [$waveOf]: $rel ($($blocks.Count))"
  $converted++
}

Write-Host "Done. converted=$converted skipped=$skipped errors=$($errors.Count)"
$errors | ForEach-Object { Write-Host "ERR $_" }
$errors | Set-Content "$hwRoot\_tools\gen-errors.txt"
