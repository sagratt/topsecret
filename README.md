### Technologies:
Server: .NET 6 WebApi, .NET 6 Worker, MassTransit(RabbitMQ), EF(MSSQL)

Client: React(create-react-app), SemanticUI, Axios, TypeScript

### Architecture
Backend is split into two parts, ConversionApi and ConversionService.

ConversionApi:  
- Communicates with the client
- Accepts files, returns converted files, returns conversion status
- Writes file information(name, location, conversion status) to database

ConversionService:  
- Converts files to pdf

ConversionApi and ConversionService communicate via message broker.  
ConversionApi sends commands to convert the file.  
ConversionService notifies ConversionApi when it starts and finishes file conversion process.  
ConversionApi updates conversion status in the db.  
Client checks conversion status before download.  

### How to run

0. Requires RabbitMq,  MSSQL Server, NodeJs

ConversionApi  
1. Check database connection string in *.\ConversionApi\HtmlToPdf.ConversionApi.Web\appsettings.json*   
2. `dotnet run --project .\ConversionApi\HtmlToPdf.ConversionApi.Web\HtmlToPdf.ConversionApi.Web.csproj`  
3. `dotnet run --project .\ConversionApi\HtmlToPdf.ConversionApi.Broker.Consuming\HtmlToPdf.ConversionApi.Broker.Consuming.csproj`  

ConversionService  
4. `dotnet run --project .\ConversionService\HtmlToPdf.ConversionService.Broker.Consuming\HtmlToPdf.ConversionService.Broker.Consuming.csproj`

Client  
5. `npm install --prefix .\client hmtl-to-pdf-client`
6. `npm start --prefix .\client`  
7. Check *CorsConfiguration:AllowedOrigins* configuration in *.\ConversionApi\HtmlToPdf.ConversionApi.Web\appsettings.json*. It needs to contain the address of client site.