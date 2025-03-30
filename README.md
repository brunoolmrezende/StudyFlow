## Sobre o Projeto

Esta **API** foi desenvolvida com **.NET 9** com o objetivo de ajudar os usuários a organizarem suas revisões de estudo de forma eficiente. Seguindo conceitos como Domain-Driven Design (DDD) e boas práticas de desenvolvimento, a API permite que os usuários registrem conteúdos para revisão e recebam notificações/lembretes via e-mail.

A API segue o padrão **RESTful**, garantindo uma comunicação eficiente e bem estruturada. Além disso, conta com documentação via **Swagger**, facilitando a exploração e o teste dos endpoints pelos desenvolvedores.

Para otimizar o desenvolvimento e garantir um código limpo e bem testado, foram utilizados diversos pacotes NuGet, incluindo:

- **Entity Framework Core**: ORM utilizado para facilitar a manipulação do banco de dados.

- **FluentValidation**: Validações simplificadas e organizadas.

- **AutoMapper**: Facilita o mapeamento entre entidades e objetos de transferência de dados.

- **XUnit & FluentAssertions**: Implementação de testes unitários garantindo a qualidade do código.

### Features

✅ **Agendamento de Revisões:** O usuário pode definir revisões com base na dificuldade do conteúdo, recebendo lembretes automáticos para reforço no aprendizado.

✅ **Notificações por E-mail:** Sistema de lembretes configuráveis que envia e-mails para lembrar os usuários de suas próximas revisões.

✅ **Autenticação e Segurança:** Login e registro de usuários com criptografia de senhas e boas práticas de segurança.

✅ **Sistema de Filtragem:** Possibilidade de buscar revisões por status e dificuldade.

✅ **Testes Automatizados:** Testes de unidade para garantir confiabilidade e estabilidade na aplicação.

✅ **Documentação Interativa:** Swagger integrado para explorar e testar os endpoints da API de forma visual.

## Construído com

![badge-dot-net]
![badge-windows]
![badge-visual-studio]
![badge-mysql]
![badge-swagger]


<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-windows]: https://img.shields.io/badge/Windows-0078D4?logo=windows&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
