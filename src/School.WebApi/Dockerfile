# Informa a partir de qual imagem será gerada a nova imagem.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

# Define qual será o diretório de trabalho (lugar onde serão copiados os arquivos, e criadas novas pastas);
WORKDIR /app

# Copia arquivos ou diretórios locais para dentro da imagem.
COPY . ./

WORKDIR /app/School.WebApi
# Especifica que o argumento seguinte será executado, ou seja, realiza a execução de um comando;
RUN dotnet restore 

# Copia arquivos ou diretórios locais para dentro da imagem.
RUN dotnet publish -c Release -o school-api

FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app

COPY --from=builder /app/School.WebApi/school-api .

# Instrução que cria e atribui um valor para uma variável dentro da imagem.
ENV ASPNETCORE_URLS="http://*:5000"
# Expõem uma ou mais portas, isso quer dizer que o container quando iniciado poderá ser acessível através dessas portas;
EXPOSE 5000
#Informa qual comando será executado quando um container for iniciado utilizando esta imagem.
ENTRYPOINT [ "dotnet", "School.WebApi.dll" ]