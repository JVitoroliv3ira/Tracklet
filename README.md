# Tracklet

**Coleta e análise simplificada de métricas de acesso para aplicações web**  

O Tracklet é uma solução backend minimalista para monitorar o tráfego de aplicações web em tempo real. Projetado para desenvolvedores que buscam dados essenciais sem complexidade.

---

## 🧩 Funcionalidades
- **Rastreamento básico de acesso**: URLs, horários e metadados de dispositivo.
- **Sessões de usuário**: Agrupamento automático de atividades em janelas de 30 minutos.
- **API dual-mode**: Suporte a gRPC (alta performance) e HTTP/JSON (compatibilidade).
- **Visualização imediata**: Gráficos de barras para tendências diárias/horárias.

---

## ⚙️ Tecnologias Principais
- **Linguagem**: Go (Golang) para eficiência e concorrência.
- **Comunicação**: gRPC para operações críticas, HTTP para flexibilidade.
- **Armazenamento**: SQLite (embutido) com migração futura para PostgreSQL.
- **Visualização**: Chart.js para renderização client-side de dados.

---

## 🏗️ Arquitetura do Sistema
```plaintext
Cliente (Web/Mobile)
       │
       ▼
  [SDK Leve] → Envio de Eventos (gRPC/HTTP)
       │           
       ▼
[Serviço Tracklet] → Processamento e Armazenamento (SQLite)
       │
       ▼
[Dashboard Web] ← Consulta de Métricas via API
```

---

## 📌 Próximas Etapas
- **Filtragem de dados**: Por intervalo de datas, tipo de dispositivo ou URL.
- **Métricas avançadas**: Taxa de rejeição, tempo médio por sessão.
- **Segurança**: Autenticação básica para endpoints de API.
- **Documentação técnica**: Especificações gRPC e guia de contribuição.
