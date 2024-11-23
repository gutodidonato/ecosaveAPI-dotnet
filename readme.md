
# EcoSave

## Descrição

**EcoSave** é um projeto desenvolvido para monitoramento e otimização do consumo de energia elétrica por cômodos. A solução permite gerenciar dados de consumo, dispositivos e pontos de energia, oferecendo uma forma eficiente de monitorar e controlar o uso de energia em diferentes áreas de um ambiente. Utiliza o ASP.NET Core para o backend, com práticas de Clean Code, SOLID e boas práticas de desenvolvimento.

## Estrutura do Projeto

A solução EcoSave é organizada da seguinte forma:

### Pastas Principais

- **Controllers**: Contém os controllers responsáveis por expor a API e gerenciar as interações entre o frontend e o backend. Cada controller trata de um conjunto de operações para as entidades do sistema.
  - **ComodosController**: Gerencia as operações CRUD para os cômodos.
  - **ConsumoController**: Gerencia os dados de consumo de energia.
  - **DispositivoController**: Controla os dispositivos conectados para monitoramento.
  - **EnderecoController**: Trata os dados de localização e endereços.
  - **PontosComodosController**: Gerencia os pontos para o usuário.
  - **UsuarioController**: Gerencia os usuários, media do consumo do usuário e uso de ia para avaliação do gasto do usuário comparado com a média brasileira de gasto.

- **Data**: Contém a configuração do banco de dados e o contexto de acesso a dados, utilizando o Entity Framework para interação com o banco.

- **Models**: Define as entidades que são manipuladas pela aplicação, como:
  - **Comodo**: Representa os cômodos onde o consumo de energia é monitorado.
  - **Consumo**: Representa os dados de consumo de energia dos cômodos.
  - **Dispositivo**: Representa os dispositivos conectados aos pontos de energia.
  - **Endereco**: Contém os dados de localização de cada cômodo.
  - **Ponto**: Representa os pontos de energia nos cômodos.
  - **Usuario**: Armazena os dados dos usuários do sistema.
  - **Response**: **AvaliacaoConsumoResponse**: Cria classes de suporte para o uso da api do openAI

- **Repositories**: Contém a lógica de acesso aos dados (CRUD) e abstrai a interação com o banco de dados.

- **Services**: Implementa a lógica do uso da API do openAI para solução de ia generativa

## Funcionalidades

- **Gerenciamento de Cômodos**: Adicionar, atualizar e remover cômodos para monitoramento do consumo de energia.
- **Monitoramento de Consumo**: Acompanhar o consumo de energia de cada cômodo em tempo real.
- **Gestão de Dispositivos**: Registrar dispositivos que consomem energia e vinculá-los aos pontos de energia.
- **Controle de Pontos de Energia**: Monitorar e controlar os pontos de energia em cada cômodo.
- **Gestão de Usuários**: Acompanhar os usuários registrados no sistema, auxiliar no contexto de médias de consumo e soluções de ia.

## Práticas de Clean Code e SOLID

O projeto segue as boas práticas de **Clean Code** e os princípios **SOLID** para garantir um código limpo, modular e fácil de manter.

### Clean Code

- **Nomenclatura Clara**: As variáveis, métodos e classes possuem nomes significativos.
- **Métodos Pequenos**: Cada método possui uma única responsabilidade.
- **Evitar Duplicação**: O código é centralizado e reutilizado em locais apropriados.
- **Tratamento de Erros**: Utilização adequada de mensagens de erro e status HTTP.

### SOLID

- **Single Responsibility Principle (SRP)**: Cada classe e método tem uma única responsabilidade.
- **Open/Closed Principle (OCP)**: O código é aberto para extensão e fechado para modificação.
- **Liskov Substitution Principle (LSP)**: Subclasses podem ser substituídas por suas classes base sem afetar o comportamento do sistema.
- **Interface Segregation Principle (ISP)**: Interfaces específicas e enxutas.
- **Dependency Inversion Principle (DIP)**: Uso de injeção de dependências para desacoplamento de classes.

## OBS

rota do swagger: http://localhost:5016
