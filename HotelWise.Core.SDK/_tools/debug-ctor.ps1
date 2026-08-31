$Lines = [IO.File]::ReadAllLines("C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK\AI\Adapters\GroqApiAdapter.cs")
$k = -1
for ($i = 0; $i -lt $Lines.Count; $i++) {
  if ($Lines[$i] -match 'class GroqApiAdapter') { $k = $i }
}
$bodyStart = $k
while ($bodyStart -lt $Lines.Count -and $Lines[$bodyStart] -notmatch '\{') { $bodyStart++ }
$depth = 0
$bodyEnd = $bodyStart
for ($idx = $bodyStart; $idx -lt $Lines.Count; $idx++) {
  $depth += ([regex]::Matches($Lines[$idx], '\{')).Count
  $depth -= ([regex]::Matches($Lines[$idx], '\}')).Count
  if ($depth -eq 0 -and $idx -ge $bodyStart) { $bodyEnd = $idx; break }
}
Write-Host "k=$k bodyStart=$bodyStart bodyEnd=$bodyEnd"
$bodyLines = $Lines[$bodyStart..$bodyEnd]
Write-Host "Body count=$($bodyLines.Count)"
$text = $bodyLines -join "`n"
$m = [regex]::Match($text, '(?s)public\s+GroqApiAdapter\s*\((.*?)\)\s*(?::\s*base\s*\((.*?)\))?\s*\{')
Write-Host "Match=$($m.Success) params='$($m.Groups[1].Value)'"
$ctorLine = $bodyLines | Where-Object { $_.Contains('GroqApiAdapter(') }
Write-Host "CtorLine=$ctorLine"
