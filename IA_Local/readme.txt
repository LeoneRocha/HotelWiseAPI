# Subir os serviços
docker-compose -f D:\GITHUB\HotelWiseAPI\IA_Local\docker-compose.yml up -d

# Executar o script PowerShell
powershell -ExecutionPolicy Bypass -File "D:\GITHUB\HotelWiseAPI\IA_Local\setup-env.ps1"




https://hub.docker.com/r/ollama/ollama



2025DevAI!
llama3.2

Para rodar o arquivo `docker-compose.yml` localizado em `D:\GITHUB\HotelWiseAPI\IA_Local` no terminal do Docker Desktop, siga estes passos:

---

### 1. **Abra o terminal**
- No Docker Desktop, você pode usar o terminal integrado ou abrir um terminal como o PowerShell, Prompt de Comando ou Windows Terminal.

---

### 2. **Navegue até o diretório do `docker-compose.yml`**
Use o comando `cd` para mudar para o diretório correto. No caso do seu diretório, o comando seria:

```powershell
cd D:\GITHUB\HotelWiseAPI\IA_Local
```

---

### 3. **Execute o Docker Compose**
No terminal, execute o seguinte comando para subir os serviços definidos no `docker-compose.yml`:

```powershell
docker-compose up -d
```

- **`up`**: Inicia os serviços definidos no arquivo `docker-compose.yml`.
- **`-d`**: Roda os serviços em segundo plano (modo "detached").

---

### 4. **Verificar os contêineres ativos**
Depois de rodar o comando acima, você pode verificar se os serviços estão rodando corretamente com:

```powershell
docker ps
```

Isso mostrará os contêineres ativos e suas portas.

---

### 5. **Parar os serviços quando necessário**
Se quiser parar os serviços mais tarde, navegue para o mesmo diretório e execute:

```powershell
docker-compose down
```

---

Agora os serviços definidos no arquivo `docker-compose.yml` devem estar funcionando! Se algo der errado, você pode verificar os logs de erro com:

```powershell
docker-compose logs
```

Se precisar de ajuda adicional, é só dizer! 🚀




Para configurar o Ollama como o serviço de modelo e embeddings para o Anything LLM, você precisa garantir que o Ollama esteja funcionando corretamente e que o arquivo `.env` seja configurado adequadamente. Aqui estão os passos detalhados:

---

### 1. **Certifique-se de que o Ollama está rodando**
- O Ollama precisa estar instalado e configurado no mesmo ambiente do Anything LLM ou acessível via rede. Você pode configurar o Ollama através do contêiner Docker incluído no `docker-compose.yml`.

Se você não tiver certeza de que o Ollama está rodando, inicie-o usando o comando:
```bash
docker-compose up -d ollama
```

---

### 2. **Edite o arquivo `.env`**
- Certifique-se de que o Anything LLM está configurado para se comunicar com o Ollama.
- Abra o arquivo `.env` dentro do diretório configurado (`$STORAGE_LOCATION`).

Adicione as seguintes linhas (ou ajuste as existentes):

```env
# Configuração do Ollama como LocalAI
LOCALAI_ENDPOINT=http://ollama:11434/v1
LOCALAI_MODEL=llama2

# (Opcional) Configuração de embeddings, caso o Ollama seja utilizado para isso
EMBEDDING_SERVICE=http://ollama:11434/v1
EMBEDDING_MODEL=embedding-model-name
```

### Detalhes das configurações:
- **`LOCALAI_ENDPOINT`**: Endpoint do Ollama para comunicação. No exemplo, ele está em `http://ollama:11434/v1`, assumindo que o serviço Ollama está no mesmo ambiente Docker.
- **`LOCALAI_MODEL`**: O nome do modelo configurado no Ollama, como `llama2`.
- **`EMBEDDING_SERVICE`**: Opcional, mas útil para configurações de embeddings. Aponta para o mesmo endpoint do Ollama.
- **`EMBEDDING_MODEL`**: Especifique o nome do modelo que será usado para gerar embeddings.

---

### 3. **Reinicie os serviços**
- Após editar o arquivo `.env`, reinicie os contêineres para aplicar as mudanças:

```bash
docker-compose down
docker-compose up -d
```

---

### 4. **Teste a comunicação**
- Acesse o Anything LLM em `http://localhost:3001`.
- Verifique se ele consegue acessar os modelos do Ollama e utilize os endpoints para geração de texto ou embeddings.

Se algum erro ocorrer, revise o endpoint no `.env` e certifique-se de que os dois serviços (`anythingllm` e `ollama`) estão rodando corretamente.

Se precisar de mais ajuda ou encontrar problemas, é só dizer! 🚀









Sim, é possível configurar o Docker para usar **VRAM** (memória da GPU) para aplicações que dependem de aceleração por hardware, como modelos de machine learning. No entanto, o Docker sozinho não acessa diretamente a VRAM. Ele precisa de ferramentas específicas para ativar o suporte à GPU. Aqui está como funciona:

---

### **1. Pré-requisitos**
Para permitir que um contêiner Docker use a GPU (e, consequentemente, a VRAM):
- Você precisa de uma GPU compatível, como NVIDIA ou AMD.
- O driver da GPU deve estar corretamente instalado no sistema operacional anfitrião.
- Para GPUs NVIDIA, é necessário instalar o **NVIDIA Container Toolkit**.

---

### **2. Configurar Docker para NVIDIA GPUs**
Se você tiver uma GPU da NVIDIA, siga estas etapas para habilitar o uso de VRAM no Docker:

#### a) Instale os drivers NVIDIA no host:
Certifique-se de que os drivers NVIDIA estão instalados. Para verificar:
```bash
nvidia-smi
```
Se este comando retornar informações sobre a GPU, o driver está configurado corretamente.

#### b) Instale o NVIDIA Container Toolkit:
Adicione o suporte à GPU no Docker com o NVIDIA Container Toolkit:
```bash
distribution=$(. /etc/os-release;echo $ID$VERSION_ID) && \
    curl -s -L https://nvidia.github.io/nvidia-docker/gpgkey | sudo gpg --dearmor -o /usr/share/keyrings/nvidia-container-toolkit-keyring.gpg && \
    curl -s -L https://nvidia.github.io/nvidia-docker/$distribution/nvidia-docker.list | sudo tee /etc/apt/sources.list.d/nvidia-docker.list && \
    sudo apt-get update && sudo apt-get install -y nvidia-container-toolkit
```
Após instalar, reinicie o Docker:
```bash
sudo systemctl restart docker
```

#### c) Execute contêineres com acesso à GPU:
Use o parâmetro `--gpus` para ativar o uso da GPU ao executar um contêiner Docker:
```bash
docker run --gpus all <nome_da_imagem>
```

---

### **3. Configurar Docker para AMD GPUs**
Para AMD GPUs, você pode usar ferramentas como o **ROCm** (Radeon Open Compute) para habilitar o uso da GPU em contêineres.

#### Passos:
- Instale o driver ROCm no host.
- Use a ferramenta ROCm Docker para rodar contêineres com suporte a GPU.

---

### **4. Testar o uso da GPU dentro do contêiner**
Após configurar, você pode verificar se o contêiner está usando a GPU. Para contêineres NVIDIA:
```bash
docker run --gpus all nvidia/cuda:11.8.0-base nvidia-smi
```
Isso exibirá informações sobre a GPU e o uso de VRAM.

---

### **5. Integração com modelos e frameworks**
Se o modelo que você está usando (como LLaMA ou Ollama) oferece suporte a aceleração por GPU, ele automaticamente usará a VRAM disponível no contêiner. Por exemplo:
- Frameworks como TensorFlow e PyTorch possuem suporte integrado para GPUs.
- Certifique-se de que o código ou framework está configurado para usar a GPU (em vez de CPU).

---

Se precisar de ajuda para configurar o suporte a GPU no Docker ou ajustar algo para o seu caso específico, é só me chamar! 🚀
