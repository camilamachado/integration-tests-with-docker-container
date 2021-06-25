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
