# API - Central de Erros @Codenation em Parceria com @Itaú
## squad-3-ad-csharp-women-itau-1

### Índice
* [Objetivo](#objetivo)
* [Tecnologias e Ferramentas Utilizadas](#tecnologias-utilizadas)
* [Banco de Dados](#banco-de-dados)
* [Rodar Aplicação](#run-aplicacao)
* [Deploy da Aplicação](#deploy-da-aplicação)
* [Demonstração da Aplicação](#demonstração)
* [Apresentação](#slides)
* [Rotas](#rotas)
* [FrontEnd](#front-end)
* [Agradecimentos](#agradecimentos)

### Trabalho desenvolvido pela Squad:

- [Jaqueline Laurenti](https://www.linkedin.com/in/jaqueline-laurenti-30b15933/)

- [Juliane Ventura](https://linkedin.com/in/julianeventura)

- [Marcela do Vale](https://www.linkedin.com/in/marceladovale/)

- [Priscila Silva](https://linkedin.com/in/silva-priscila)


### Objetivo
Em projetos modernos é cada vez mais comum o uso de arquiteturas baseadas em serviços ou microsserviços. Nestes ambientes complexos, erros podem surgir em diferentes camadas da aplicação (backend, frontend, mobile, desktop) e mesmo em serviços distintos. Desta forma, é muito importante que os desenvolvedores possam centralizar todos os registros de erros em um local, de onde podem monitorar e tomar decisões mais acertadas. Neste projeto vamos implementar um sistema para centralizar registros de erros de aplicações.

A arquitetura do projeto é formada por:

### Backend - API
- criar endpoints para serem usados pelo frontend da aplicação
- criar um endpoint que será usado para gravar os logs de erro em um banco de dados relacional
- a API deve ser segura, permitindo acesso apenas com um token de autenticação válido
#### Frontend
- deve implementar as funcionalidades apresentadas nos wireframes
- deve ser acessada adequadamente tanto por navegadores desktop quanto mobile
- deve consumir a API do produto
- desenvolvida na forma de uma Single Page Application

### Tecnologias Utilizadas
- C# .NET
- Entity Framework
- Clean Code 
- Swagger

### Ferramentas Utilizadas 
- Visual Studio
- Trello
- WhatsApp
- Azure 
- Heroku
- Docker

### Banco de dados
- SQLServer criado através do Azure.

### Rodar Aplicação
![frustrated-computer-baboon](https://media.giphy.com/media/h8yZBiRGWRu8KuukDB/giphy.gif)
![frustrated-computer-baboon](https://media.giphy.com/media/h8Irzr4ilkQx3UzceS/giphy.gif) <p>
   Acessar a url: https://localhost:5001/swagger/index.html 


### Deploy da Aplicação
O deploy da aplicação foi feito através do Heroku, utilizando o Docker Image.
* [API](https://central-erros-itau.herokuapp.com/swagger/index.html)

### Demonstração da Aplicação
* [BACK-END](https://youtu.be/8GfyJ87uzwk)
* [FRONT-END](https://youtu.be/YLRLoekZKCc)

### Slides
Slides utilizados na apresentação:
[PPT-VIDEO](https://youtu.be/MHhMMXfHxuY) <p>
![frustrated-computer-baboon](https://media.giphy.com/media/hSAFqlKYrglGSTwORV/giphy.gif)


### Rotas
* AuthController
    * POST /Auth/registerUser - Realiza cadastro de um novo usuário.
    * POST /Auth/login - Realiza login do usuário cadastrado.
    * POST /Auth/logout - Realiza o logout do usuário.
    * POST /Auth/forgotPassword - Envia codigo para que o usuário possa resetar a senha.
    * GET  /Auth/resetPassword - Usuário deverá preencher Id e email para verificar o codigo de reset.
    * POST /Auth/resetPasswordConfirm - Usuário reseta a senha.

* ErrorOccurrenceController
    * POST /ErrorOccurrence - Cadastra novo erro na Central.
    * GET /ErrorOccurrence - Traz todos os erros ativos cadastrados na Central.
    * GET /ErrorOccurrence/id - Traz erro por número de ID.
    * GET /ErrorOccurrence/getErrorDetails/id - Traz detalhes de um determinado erro por ID.
    * GET /ErrorOcurrence/getFiledErrors - Traz todos os erros arquivados
    * GET /ErrorOccurrence/idAmbiente/idOrdenacao/idBusca/textoBuscado - Filtra os erros de acordo com os filtros selecionados.
    * PUT /ErrorOccurrence/setFiledErrors/id - Arquiva erro por ID.
    * PUT /ErrorOccurrence/setUnarchiveErrors/id - Desarquiva erro por ID.
    * DELETE /ErrorOccurrence/id - Deleta erro da Central por ID.

* EnvironmentController
    * GET /Environment - Traz todos os ambientes da Central.
    * DELETE /Environment/id - Deleta ambiente por ID.

* LevelController
    * GET /Level - Traz todos os levels da Central.
    * DELETE /Level/id - Deleta level por ID.

### Front-End
Front-end foi desenvolvido por Jaqueline Laurenti com a tecnologia React. <p>
![frustrated-computer-baboon](https://media.giphy.com/media/fSRxyZEZnvDtrlqJXJ/giphy.gif)


### Agradecimentos
* Codenation
* Ingrid Oliveira
* Itaú
