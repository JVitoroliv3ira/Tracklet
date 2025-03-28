# Tracklet

**Solução de monitoramento web simplificada com SDK integrado**  

Backend em .NET 8 para coleta de métricas de acesso via SDK client-side. Foco em implementação direta e baixa latência.

---

## 🧩 Funcionalidades-Chave
- **SDK de Coleta**: Componente leve para envio de eventos (Web/JS ou .NET)
- **Sessões Automáticas**: Agrupamento de atividades em janelas de 30min
- **Metadados Ricos**: Dispositivo, geolocalização e dados técnicos
- **API REST**: Endpoints para integração e consulta de dados brutos

---

## ⚙️ Stack Técnica
- **Backend**: .NET 8 (Minimal APIs + EF Core)
- **SDK Client**: Pacote NuGet/NPM para coleta de eventos
- **Armazenamento**: PostgreSQL com otimização temporal
- **Protocolo**: HTTP/JSON com compressão

---

## 🏗️ Fluxo de Dados
```plaintext
        [Aplicação Monitorada]
              │
              ▼ (SDK Embedded)
         [Eventos]
              │
              ▼ (HTTP/JSON)
  [Tracklet API] → (.NET 8)
              │           
              ▼ (Processamento)
[PostgreSQL] → (Sessões/Métricas)
              │
              ▼ (Consulta)
        [Dashboard/BI]
```

---
