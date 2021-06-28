# Testes de integração utilizando docker container 🐳
Proposta de teste de integração em uma WebAPI usando container Docker p/ isolar ambiente de teste.

### 🧾 PASSOS:

##### 1- Criado arquivo `docker-compose-integration` localizado na raiz do projeto:

- Esse arquivo chama o **Dockerfile** que está localizado na pasta `src\School.WebApi`.
- Tem como objetivo publicar a **School.WebApi** em um container, expondo a `porta 5000` p/ acessa-lo

##### 2- Criado projeto de teste School.Integration.Tests:

- Neste projeto foi criado uma classe chamada **BaseIntegrationTest**. Onde é responsável por criar/inicializar o container com a School.WebApi ao rodar os testes e excluir o container e as imagens ao finalizar os testes.
- Toda classe de teste deve herdar de **BaseIntegrationTest**.
- No arquivo **appsettings** fica configurado a porta onde a School.WebApi fica exposta usando container.

##### 3 - Sobre o OneTimeSetUp de BaseIntegrationTest

- É usado a biblioteca **Ductus.FluentDocker** para criação do container.
- Para a FluentDocker passamos o caminho do docker-compose-integration -> FromFile 
- Para a FluentDocker configuramos que deve remover o container -> RemoveOrphans
- Para a FluentDocker configuramos que deve remover todas as imagens que o docker compose criou -> RemoveAllImages

##### 4 - Sobre o OneTimeSetUp de BaseIntegrationTest

- Através da instancia Builder criada a partir da biblioteca Ductus.FluentDocker é chamado o método Dispose.

- O método Dispose chama o ComposeDown da biblioteca, que tem como objetivo, realizar todas as configurações que foram setadas anteriormente como:

- - Remover o container
  - Remover as imagens geradas por ele

### :warning:ATENÇÃO:

- Ao rodar os testes é criado algumas imagens sem repositório e sem tag <none>:<none>. Essas imagens **NÃO** são excluídas no método Dispose de Ductus.FluentDocker. A cada alteração na **School.WebApi** são criadas novas 5 imagens <none>:<none>.
- Ao atualizar a versão do Docker os testes quebram. Pois a biblioteca Ductus.FluentDocker não consegue criar o container (não sabemos o motivo correto). Foi validado a situação onde estava na versão 19.03.5 para 20.10.5 e os testes falharam.

### :pushpin: REFERÊNCIAS:

Links que contribuíram com o estudo:

https://www.lambda3.com.br/2019/05/utilizando-docker-em-testes-de-integracao/

https://github.com/mguardarini/docker-dotnetcore

https://github.com/mariotoffia/FluentDocker



## ABORDAGEM 1: Testes com integração completa

1.  **Disponibilizar Identity**:large_blue_circle::
   - Subir identity *(em container especifico de tests)*
     - Criar BaseIntegrationTest no projeto nddSmart.PrintersModule.IntegrationTests *(ver School.Integration.Tests)*
   - Obter token para criação do tenant *(usar scope nddsmart-mps-integration-api)*
     - Através de chamada http client
     - Criar BaseIntegrationTest no projeto nddSmart.PrintersModule.IntegrationTests com variável que vai armazenar esse token
2. **Subir dependências em container** :large_blue_circle:: *(usar SQL local? | usar bancos especificos p/ teste?)*
   - nddSmart.Core.Web.API (ver necessidade)
   - nddSmart.Core.Web.MPS.Integration.API
   - nddSmart.Core.Service.BaseProcessor
   - nddSmart.PrintersModule.Web.Api
   - nddSmart.PrintersModule.Service.Processor
   - nddSmart.PrintersModule.Service.SupplyProcessor
3. **Criar tenant e usuário adm** :large_blue_circle::  
   - Criar tenant com usuário adm (através de chamada http client no projeto)
     - Validar polices do usuário (TenantCatalogDb)
   - Setar senha através de script SQL? 
4. **Obter token de acesso com usuário** :red_circle::
   - Obter acesso com scope nddsmart-core-web-api *(validar se o token muda)*
   - Obter acesso com scope nddsmart-printer-web-api *(validar se o token muda)*
     - Fazer uma chamada qualquer p/ o nddSmart.PrintersModule.Web.Api p/ aplicar o esquema de BD e polices
5. **Seed dados básicos** :white_circle::
   - Criar uma organização *(através de chamada http client)*
   - Criar um impressora *(através de chamada http client)*
   - Ao final dos testes apagar os bancos?

6. **Iniciar criação dos testes e medir febre**
   - Ver possibilidade de usar o Contexto de printer diretamente p/ fazer Seed



## ABORDAGEM 2: Testes com integração média

1.  **Disponibilizar Identity** :large_blue_circle:: 
   - Subir identity *(em container especifico de tests)*
     - Criar BaseIntegrationTest no projeto nddSmart.PrintersModule.IntegrationTests *(ver School.Integration.Tests)*
     - Configurar usuários padrão (ver configs\identity-local\identityseed.acceptance)

2. **Subir dependências em container** :large_blue_circle:: *(usar SQL local? | usar bancos especificos p/ teste?)*
   - nddSmart.PrintersModule.Web.Api
   - nddSmart.PrintersModule.Service.Processor
   - nddSmart.PrintersModule.Service.SupplyProcessor
3. **Criar banco de dados do Tenant de testes de integração** :large_blue_circle:
   - Criar baseado na ferramenta dos teste de aceitação (nddSmart.Core.AcceptationTests.Tool)
   - Inserir usuários padrão conforme na configuração do identity realizada no passo acima
   - Criar uma organização 
   - Criar um impressora 
   - Ao final dos testes apagar os bancos?
4. **Adicionar o tenant no TenantCatalogDb** :red_circle::
   - Adicionar através de script SQL (verificar se tem acesso ao Contexto)
5. **Adicionar as polices no TenantCatalogDb** :red_circle::
   - Adicionar através de script SQL (verificar se tem acesso ao Contexto)

6. **Obter token de acesso com usuário** :red_circle::
   - Obter acesso com scope nddsmart-core-web-api *(validar se o token muda)*
   - Obter acesso com scope nddsmart-printer-web-api *(validar se o token muda)*
7. **Iniciar criação dos testes e medir febre**
   - Seed de dados sempre através do Contexto de printer diretamente
