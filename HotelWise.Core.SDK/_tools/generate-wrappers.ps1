# Thin-wrapper generator: parses HW source (no SCH assembly load required).
param(
  [ValidateSet('contracts','statics','runtime','all')]
  [string]$Wave = 'all'
)

$ErrorActionPreference = 'Stop'
$hwRoot = "C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK"

function Get-FileWaveCategory([string]$RelPath) {
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
  $matchObj = [regex]::Match($Content, 'namespace\s+([\w.]+)')
  if ($matchObj.Success) { return $matchObj.Groups[1].Value }
  return $null
}

function Get-ObsoleteAttrInfo([string[]]$Lines, [int]$Idx) {
  $j = $Idx
  while ($Lines[$j] -notmatch '\]\s*$' -and $j -lt $Lines.Count - 1) { $j++ }
  $attrText = ($Lines[$Idx..$j] -join "`n").Trim()
  $rgxMatch = [regex]::Match($attrText, 'tipo (SmartCoreHub\.Core\.SDK\.[A-Za-z0-9_.]+)')
  $schTarget = if ($rgxMatch.Success) { $rgxMatch.Groups[1].Value.TrimEnd('.') } else { $null }
  return [PSCustomObject]@{
    EndIdx = $j
    AttrText = $attrText
    SchTarget = $schTarget
  }
}

function Get-DocAndPreAttrs([string[]]$Lines, [int]$AttrStart) {
  $docStart = $AttrStart - 1
  while ($docStart -ge 0 -and ($Lines[$docStart] -match '^\s*///' -or $Lines[$docStart] -match '^\s*$' -or $Lines[$docStart] -match '^\s*\[JsonConverter' -or $Lines[$docStart] -match '^\s*\[Required')) {
    $docStart--
  }
  $docStart++
  while ($docStart -lt $AttrStart -and $Lines[$docStart] -match '^\s*$') { $docStart++ }
  
  $preAttrsList = @()
  $docLinesList = @()
  for ($p = $docStart; $p -lt $AttrStart; $p++) {
    if ($Lines[$p] -match '^\s*\[' -and $Lines[$p] -notmatch 'Obsolete') {
      $preAttrsList += $Lines[$p].Trim()
    }
    if ($Lines[$p] -match '^\s*///') {
      $docLinesList += $Lines[$p].TrimEnd()
    }
  }
  return [PSCustomObject]@{
    DocStart = $docStart
    PreAttrs = $preAttrsList
    XmlDoc = $docLinesList
  }
}

function Get-BlockBoundaries([string[]]$Lines, [int]$StartLine) {
  $bodyStart = $StartLine
  while ($bodyStart -lt $Lines.Count -and $Lines[$bodyStart] -notmatch '\{') { $bodyStart++ }
  $depth = 0
  $bodyEnd = $bodyStart
  for ($b = $bodyStart; $b -lt $Lines.Count; $b++) {
    $depth += ([regex]::Matches($Lines[$b], '\{')).Count
    $depth -= ([regex]::Matches($Lines[$b], '\}')).Count
    if ($depth -eq 0 -and $b -ge $bodyStart) { $bodyEnd = $b; break }
  }
  return [PSCustomObject]@{ BodyStart = $bodyStart; BodyEnd = $bodyEnd }
}

function Get-ConstraintsAndInterfaces([string[]]$Lines, [int]$DeclStart, [int]$BodyStart) {
  $constraintList = @()
  $interfaceList = @()
  for ($d = $DeclStart; $d -le $BodyStart; $d++) {
    if ($d -ge $Lines.Count) { break }
    $curLine = $Lines[$d]
    $matchWhere = [regex]::Match($curLine, '\bwhere\s+(.+)$')
    if ($matchWhere.Success) {
      $clause = "where " + $matchWhere.Groups[1].Value.Trim()
      $clause = $clause -replace '\s*\{.*$',''
      $constraintList += $clause.Trim()
    }
    $matchDecl = [regex]::Match($curLine, '(?:interface|class)\s+\w+(?:<[^>]+>)?\s*:\s*(.+)$')
    if ($matchDecl.Success) {
      $bases = $matchDecl.Groups[1].Value -replace '\s*where\s+.*$','' -replace '\s*\{.*$',''
      foreach ($baseItem in ($bases -split ',')) {
        $trimmed = $baseItem.Trim()
        if ($trimmed -match '^I[A-Z]\w*' -and $trimmed -notmatch '^SmartCoreHub') {
          $interfaceList += $trimmed
        }
      }
    }
  }
  return [PSCustomObject]@{ Constraints = $constraintList; HwInterfaces = $interfaceList }
}

function Get-ObsoleteTypes {
  param([string[]]$Lines)
  $blocks = @()
  $i = 0
  while ($i -lt $Lines.Count) {
    if ($Lines[$i] -notmatch '^\s*\[Obsolete\(') { $i++; continue }

    $attrInfo = Get-ObsoleteAttrInfo -Lines $Lines -Idx $i
    $docInfo = Get-DocAndPreAttrs -Lines $Lines -AttrStart $i

    $k = $attrInfo.EndIdx + 1
    while ($k -lt $Lines.Count -and ($Lines[$k] -match '^\s*\[' -or $Lines[$k] -match '^\s*$')) {
      if ($Lines[$k] -match '^\s*\[') { $docInfo.PreAttrs += $Lines[$k].Trim() }
      $k++
    }
    if ($k -ge $Lines.Count) { break }

    $declLine = $Lines[$k]
    $declMatch = [regex]::Match($declLine, 'public\s+(?:(abstract|sealed|static)\s+)?(class|interface|enum)\s+(\w+)(<[^>]+>)?')
    if (-not $declMatch.Success) { $i = $k + 1; continue }

    $mod = $declMatch.Groups[1].Value
    $kind = $declMatch.Groups[2].Value
    $name = $declMatch.Groups[3].Value
    $gen = $declMatch.Groups[4].Value

    $bounds = Get-BlockBoundaries -Lines $Lines -StartLine $k
    $ci = Get-ConstraintsAndInterfaces -Lines $Lines -DeclStart $k -BodyStart $bounds.BodyStart

    $schTarget = if ($attrInfo.SchTarget) { $attrInfo.SchTarget } else { "SmartCoreHub.Core.SDK.Domain.$name" }

    $blocks += [PSCustomObject]@{
      Kind = $kind
      Name = $name
      Generics = $gen
      IsAbstract = ($mod -eq 'abstract')
      IsStatic = ($mod -eq 'static')
      IsSealed = ($mod -eq 'sealed')
      Sch = $schTarget
      PreAttrs = $docInfo.PreAttrs
      XmlDoc = $docInfo.XmlDoc
      Attr = $attrInfo.AttrText
      Constraints = $ci.Constraints
      HwInterfaces = $ci.HwInterfaces
      FileStart = $docInfo.DocStart
      DeclLine = $k
      BodyStart = $bounds.BodyStart
      BodyEnd = $bounds.BodyEnd
      BodyLines = $Lines[($bounds.BodyStart + 1)..($bounds.BodyEnd - 1)]
    }
    $i = $bounds.BodyEnd + 1
  }
  return $blocks
}

function Get-PublicCtors([string[]]$BodyLines, [string]$TypeName) {
  $text = $BodyLines -join "`n"
  $ctors = @()
  $ctorMatches = [regex]::Matches($text, "(?s)public\s+$TypeName\s*\((.*?)\)\s*(?::\s*base\s*\((.*?)\))?\s*\{")
  foreach ($m in $ctorMatches) {
    $paramText = ($m.Groups[1].Value -replace '\s+', ' ').Trim()
    if ([string]::IsNullOrWhiteSpace($paramText)) { continue }
    $paramNames = @()
    foreach ($part in ($paramText -split ',')) {
      $trimmedPart = $part.Trim()
      if (-not $trimmedPart) { continue }
      $withoutDefault = ($trimmedPart -split '=')[0].Trim()
      $tokens = $withoutDefault -split '\s+'
      $paramNames += $tokens[-1].TrimStart('@')
    }
    $ctors += [PSCustomObject]@{ Params = $paramText; Args = ($paramNames -join ', ') }
  }
  return $ctors
}

function Get-StaticPropertiesAndFields([string]$Text, [string]$SchFqn) {
  $out = @()
  foreach ($m in [regex]::Matches($Text, '(?m)^\s*public\s+const\s+(\S+)\s+(\w+)\s*=\s*([^;]+);')) {
    $out += "    public const $($m.Groups[1].Value) $($m.Groups[2].Value) = $($m.Groups[3].Value.Trim());"
  }
  foreach ($m in [regex]::Matches($Text, '(?m)^\s*public\s+static\s+readonly\s+(\S+)\s+(\w+)\s*=')) {
    $out += "    public static readonly $($m.Groups[1].Value) $($m.Groups[2].Value) = $SchFqn.$($m.Groups[2].Value);"
  }
  foreach ($m in [regex]::Matches($Text, '(?m)^\s*public\s+static\s+(\S+)\s+(\w+)\s*\{\s*get;\s*set;\s*\}')) {
    $out += "    public static $($m.Groups[1].Value) $($m.Groups[2].Value) { get => $SchFqn.$($m.Groups[2].Value); set => $SchFqn.$($m.Groups[2].Value) = value; }"
  }
  foreach ($m in [regex]::Matches($Text, '(?m)^\s*public\s+static\s+(\S+)\s+(\w+)\s*=>')) {
    $out += "    public static $($m.Groups[1].Value) $($m.Groups[2].Value) => $SchFqn.$($m.Groups[2].Value);"
  }
  return $out
}

function Get-StaticMethods([string]$Text, [string]$SchFqn) {
  $out = @()
  $methodMatches = [regex]::Matches($Text, '(?m)^\s*public\s+static\s+(?:async\s+)?([\w.<>,\[\]\?]+)\s+(\w+)(<[^>]+>)?\s*\(([^)]*)\)')
  foreach ($m in $methodMatches) {
    $ret = $m.Groups[1].Value
    if ($ret -eq 'async') { continue }
    $name = $m.Groups[2].Value
    $gen = $m.Groups[3].Value
    $params = $m.Groups[4].Value.Trim()
    $callArgs = @()
    if ($params) {
      foreach ($part in ($params -split ',')) {
        $pTrim = $part.Trim()
        if (-not $pTrim) { continue }
        $withoutModifiers = $pTrim -replace '^this\s+','' -replace '^out\s+','' -replace '^ref\s+','' -replace '^in\s+',''
        $withoutDefault = ($withoutModifiers -split '=')[0].Trim()
        $tokens = $withoutDefault -split '\s+'
        $pname = $tokens[-1].TrimStart('@')
        if ($part -match '^\s*out\s+') { $callArgs += "out $pname" }
        elseif ($part -match '^\s*ref\s+') { $callArgs += "ref $pname" }
        else { $callArgs += $pname }
      }
    }
    $argStr = $callArgs -join ', '
    $signature = "    public static $ret $name$gen($params) =>`n        $SchFqn.$name$gen($argStr);"
    $out += $signature
  }
  return $out
}

function Get-StaticMembers([string[]]$BodyLines, [string]$SchFqn) {
  $text = $BodyLines -join "`n"
  $props = Get-StaticPropertiesAndFields -Text $text -SchFqn $SchFqn
  $methods = Get-StaticMethods -Text $text -SchFqn $SchFqn
  return @($props + $methods)
}

function New-ClassCtors([string[]]$BodyLines, [string]$TypeName) {
  $ctors = @(Get-PublicCtors -BodyLines $BodyLines -TypeName $TypeName)
  $textAll = ($BodyLines -join "`n")
  $protMatches = [regex]::Matches($textAll, "(?s)protected\s+$TypeName\s*\((.*?)\)\s*(?::\s*base\s*\((.*?)\))?\s*\{")
  foreach ($m in $protMatches) {
    $params = ($m.Groups[1].Value -replace '\s+', ' ').Trim()
    if ([string]::IsNullOrWhiteSpace($params)) { continue }
    $ctorArgs = @()
    foreach ($part in ($params -split ',')) {
      $pTrim = $part.Trim(); if (-not $pTrim) { continue }
      $tokens = (($pTrim -split '=')[0].Trim() -split '\s+')
      $ctorArgs += $tokens[-1].TrimStart('@')
    }
    $ctors += [PSCustomObject]@{ Params = $params; Args = ($ctorArgs -join ', '); Mod = 'protected' }
  }
  if ($ctors.Count -eq 0) { return "" }
  $parts = @()
  foreach ($c in $ctors) {
    $mod = if ($c.Mod) { $c.Mod } else { "public" }
    $parts += "    $mod $TypeName($($c.Params))`n        : base($($c.Args))`n    {`n    }"
  }
  return ($parts -join "`n`n")
}

function New-WrapperType($b) {
  $doc = if ($b.XmlDoc.Count) { ($b.XmlDoc -join "`n") + "`n" } else { "" }
  $pre = if ($b.PreAttrs.Count) { ($b.PreAttrs -join "`n") + "`n" } else { "" }
  $attr = $b.Attr + "`n"
  $sch = $b.Sch
  $gen = $b.Generics
  $constraints = if ($b.Constraints.Count) { "`n    " + ($b.Constraints -join "`n    ") } else { "" }

  if ($b.Kind -eq 'enum') { return $null }
  if ($b.Kind -eq 'interface') {
    return "${doc}${pre}${attr}public interface $($b.Name)$gen : $sch$gen$constraints`n{`n}"
  }
  if ($b.IsStatic) {
    $members = Get-StaticMembers -BodyLines @($b.BodyLines) -SchFqn $sch
    $body = if ($members.Count) { ($members -join "`n`n") } else { "    // no public members detected" }
    return "${doc}${pre}${attr}public static class $($b.Name)`n{`n$body`n}"
  }
  if ($b.Name -in @('ApplicationIAConfig','RagConfig')) { return $null }
  if ($b.Name -eq 'SearchCriteria') {
    return "${doc}${pre}${attr}public class SearchCriteria : $sch`n{`n    /// <summary>Alias legado HW → MaxRetrieve.</summary>`n    public int MaxHotelRetrieve { get => MaxRetrieve; set => MaxRetrieve = value; }`n}"
  }

  $abs = if ($b.IsAbstract) { "abstract " } else { "" }
  $ctorText = New-ClassCtors -BodyLines @($b.BodyLines) -TypeName $b.Name
  $extraIfaces = if ($b.HwInterfaces -and @($b.HwInterfaces).Count -gt 0) {
    ", " + ((@($b.HwInterfaces) | Select-Object -Unique) -join ", ")
  } else { "" }

  return "${doc}${pre}${attr}public ${abs}class $($b.Name)$gen : $sch$gen$extraIfaces$constraints`n{`n$ctorText`n}".TrimEnd() + "`n"
}

# ---------- main ----------
$files = Get-ChildItem $hwRoot -Recurse -Filter *.cs |
  Where-Object { $_.FullName -notmatch '\\_tools\\' -and (Select-String -Path $_.FullName -Pattern '\[Obsolete\(' -Quiet) }

$converted = 0; $skipped = 0; $errors = @()

foreach ($file in $files) {
  $rel = $file.FullName.Substring($hwRoot.Length + 1)
  $waveOf = Get-FileWaveCategory $rel
  if ($Wave -ne 'all' -and $waveOf -ne $Wave) { continue }

  $raw = [IO.File]::ReadAllText($file.FullName, [Text.UTF8Encoding]::new($false))
  $lines = $raw -split "`r?`n"
  $ns = Get-Namespace $raw
  $hasNet8 = $raw -match '(?m)^#if\s+NET8_0_OR_GREATER'
  $blocks = @(Get-ObsoleteTypes $lines)
  if ($blocks.Count -eq 0) { $errors += "parse fail $rel"; continue }

  if (($blocks | Where-Object Kind -eq 'enum').Count -eq $blocks.Count) {
    $skipped++
    continue
  }

  $parts = @()
  foreach ($b in $blocks) {
    if ($b.Kind -eq 'enum') {
      $slice = ($lines[$b.FileStart..$b.BodyEnd] -join "`n")
      $parts += $slice.TrimEnd()
      continue
    }
    $w = New-WrapperType $b
    if ($null -eq $w) { $errors += "null wrapper $($b.Name) $rel"; continue }
    $parts += $w.TrimEnd()
  }

  $sb = New-Object System.Text.StringBuilder
  if ($hasNet8) { [void]$sb.AppendLine("#if NET8_0_OR_GREATER") }

  $usingLines = @()
  foreach ($ln in $lines) {
    if ($ln -match '^\s*using\s+') { $usingLines += $ln.TrimEnd() }
    if ($ln -match '^\s*namespace\s+') { break }
  }
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
  Write-Output "OK [$waveOf]: $rel ($($blocks.Count))"
  $converted++
}

Write-Output "Done. converted=$converted skipped=$skipped errors=$($errors.Count)"
