![balta](https://baltaio.blob.core.windows.net/static/images/dark/balta-logo.svg)

![Logo do App](https://github.com/balta-io/desafio-balta-may-the-fourth-backend/assets/965305/880fab7e-3998-4a0d-98ad-1d6ffc11298b)

## 🎖️ Desafio
**May the Fourth** é a quarta edição dos **Desafios .NET** realizados pelo [balta.io](https://balta.io). Durante esta jornada, fizemos parte do batalhão backend onde unimos forças para entregar um App completo.

## 📱 Projeto
Desenvolvimento de uma API completa, fornecendo recursos como criação, leitura, atualização e exclusão de dados referentes ao universo **Star Wars**.

## Participantes
### 🚀 Capitão
[Gustavo Stephano e [@zStephano](https://github.com/zStephano)]

### 💂‍♀️ Batalhão
* [Rodrigo Penaforte -  [@RodrigoPenaforte](https://github.com/RodrigoPenaforte)]
* [Marcio Cabanhas - [@maojsm](https://github.com/maojsm)]
* [Guilherme Bley - [@GuilhermeBley](https://github.com/GuilhermeBley)]
* [Igor Cardosos - [@zugor98](https://github.com/zugor98)]

## ⚙️ Tecnologias
* C# 12
* .NET 8
* ASP.NET
* Minimal APIs

## 🥋 Skills Desenvolvidas
* Comunicação
* Trabalho em Equipe
* Networking
* Muito conhecimento técnico

## 🧪 Como testar o projeto

O projeto pode ser acessado através de meios on-line, ou também ele pode rodar localmente.

### Publicação WEB
O projeto pode ser acessado através do endereço: [swagger](https://codeorder66.azurewebsites.net/swagger/index.html)

### Como rodar localmente

Nessa seção sera mostrado o passo a passo de como rodar localmente a aplicação.

#### Subir o banco de dados

Necessário ter docker instalado na máquina e executar os seguintes comandos:
- Realização do pull da imagem
```
docker pull postgres
```

- Criação do container com a imagem
```
docker run --name my-postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```

#### Adicionar string de conexão na aplicação
No arquivo ```appsettings.json``` adicione a connection string substituindo ```myConnectionString``` pelas configurações locais do banco:

```
"ConnectionStrings": {
  "SQLConnection": "myConnectionString"
}
```

#### Execução do Migrations
Com as configurações do banco feitas, agora é só realizar a criação das tabelas no banco com o Entity Framework(lembrando que é necessário o SDK do .NET 8) utilizando os seguintes comandos:
```
dotnet tool install --global dotnet-ef
```
E após:
```
dotnet ef database update
```

#### Executar aplicação WEB
Com todos os passos até agora feitos, basta somente iniciar a aplicação WEB, lembre-se de estar no diretório do projeto ```CodeOrderAPI.csproj``` e executar na linha de comando o seguinte:
```
dotnet run
```

E pronto! Sua aplicação está rodando localmente com todas as funcionalidades disponíveis.


# 💜 Participe
Quer participar dos próximos desafios? Junte-se a [maior comunidade .NET do Brasil 🇧🇷 💜](https://balta.io/discord)
