# Projeto Tasks

Comando para criar migrations:
```bash
dotnet ef migrations add Initial --project Infra --startup-project API 
```

Para executar migrações no banco:
```bash
dotnet ef database update --project Infra --startup-project API
```

Para remover uma migração:
```bash
dotnet ef migrations remove --project Infra --startup-project API
```

Para fazer drop do banco:
```bash
dotnet ef database drop --project Infra --startup-project API
```
