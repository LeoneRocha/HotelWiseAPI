# Define o caminho para o diretorio atual
$currentDirectory = Get-Location
$outdatedPackagesFile = "$currentDirectory\outdated_packages_filtered.txt"

# Solicita ao usuario um filtro de nome (parcial ou completo)
$filter = Read-Host "Digite parte do nome para filtrar dependencias (deixe em branco para listar todas)"

# Configura o console para usar UTF-8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Lista pacotes desatualizados e salva em UTF-8
dotnet list package --outdated | Out-File -FilePath $outdatedPackagesFile -Encoding UTF8

# Funcao para buscar dependencias no NuGet
function Get-NuGetDependencies {
    param (
        [string]$packageName,
        [string]$version
    )
    $nugetUrl = "https://api.nuget.org/v3-flatcontainer/$packageName/$version/$packageName.nuspec"
    try {
        $nuspecContent = Invoke-RestMethod -Uri $nugetUrl -Method Get -ErrorAction Stop
        $xmlContent = [xml]$nuspecContent
        $dependencies = $xmlContent.package.metadata.dependencies.dependency | ForEach-Object {
            $_.id + " (" + $_.version + ")"
        }
        return $dependencies -join ", "
    } catch {
        return "Erro ao buscar dependencias para $packageName"
    }
}

# Exibe pacotes instalados formatados em tabela
if (Test-Path $outdatedPackagesFile) {
    $packages = Get-Content $outdatedPackagesFile | ForEach-Object {
        if ($_ -match '^(\S+\.csproj)\s+>\s+(\S+)\s+(\S+)\s+(\S+)\s+(\S+)$') {
            [PSCustomObject]@{
                Projeto          = $matches[1]  # Captura o nome do .csproj
                NomeDependencia  = $matches[2]  # Nome da dependencia
                VersaoAtual      = $matches[3]  # Versao atual da dependencia
                VersaoDisponivel = $matches[5]  # Versao disponivel no NuGet
                Dependencias     = Get-NuGetDependencies -packageName $matches[2] -version $matches[3]
            }
        }
    }

    # Filtra pacotes com base no nome especificado
    $packages | Where-Object { $_.NomeDependencia -match "(?i).*($filter).*" } | Format-Table Projeto, NomeDependencia, VersaoAtual, VersaoDisponivel, Dependencias -AutoSize

    Write-Output "Processo concluido! Resultados disponiveis em $outdatedPackagesFile."
} else {
    Write-Output "Nenhuma dependencia desatualizada encontrada no projeto."
}


#[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
#Set-ExecutionPolicy RemoteSigned -Scope Process 
#cd D:\GITHUB\HotelWiseAPI 
#.\check_packages.ps1
