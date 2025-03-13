# Configurar o local de armazenamento do Anything LLM
$env:STORAGE_LOCATION="$HOME\Documents\anythingllm"

# Criar o diretório de armazenamento, caso não exista
If (!(Test-Path $env:STORAGE_LOCATION)) {
    New-Item $env:STORAGE_LOCATION -ItemType Directory
}

# Caminho para o arquivo .env
$envFile = "$env:STORAGE_LOCATION\.env"

# Criar ou editar o arquivo .env
If (!(Test-Path $envFile)) {
    New-Item $envFile -ItemType File
}

# Adicionar ou atualizar as configurações no arquivo .env
$config = @"
# Configuração do Ollama como LocalAI
LOCALAI_ENDPOINT=http://ollama:11434/v1
LOCALAI_MODEL=llama2

# (Opcional) Configuração de embeddings, caso o Ollama seja utilizado para isso
EMBEDDING_SERVICE=http://ollama:11434/v1
EMBEDDING_MODEL=embedding-model-name
"@

Set-Content -Path $envFile -Value $config

# Verificar se o docker-compose já rodou e os contêineres estão ativos
Write-Host "Aguardando contêineres iniciarem..."
Start-Sleep -Seconds 5 # Ajuste o tempo de espera se necessário

# Verificar se o Ollama está em execução
$ollamaRunning = docker ps | Select-String "ollama"
If ($ollamaRunning) {
    Write-Host "Ollama está rodando. Configuração completa!"
} Else {
    Write-Host "Ollama não foi encontrado. Verifique o `docker-compose`."
}


# Subir os serviços
#docker-compose -f D:\GITHUB\HotelWiseAPI\IA_Local\docker-compose.yml up -d

# Executar o script PowerShell
#powershell -ExecutionPolicy Bypass -File "D:\GITHUB\HotelWiseAPI\IA_Local\setup-env.ps1"
