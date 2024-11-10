qdrant:
    container_name: qdrant
    image: qdrant/qdrant
    ports:
      - 6333:6333
      - 6334:6334
    volumes:
      - ./qdrant_storage:/qdrant/storage:z
    environment:
      - ENV_VARIABLE=value
      - API_KEY=value
    links:
      - "ollama"
    extra_hosts:
      - "host.docker.internal:host-gateway"