# trevodasortedigital
🎲 Sistema Gerador de Apostas - Loterias Caixa
Este projeto é um sistema completo (backend + frontend) para geração de apostas automatizadas para as Loterias da Caixa Econômica Federal, como Mega-Sena, Lotofácil, Quina, entre outras.

Desenvolvido com:

🚀 Spring Boot (Java + JPA + MySQL) - Backend

⚛️ React - Frontend

🗃️ MySQL - Banco de dados relacional

📌 Funcionalidades
Geração automática de apostas conforme regras de cada modalidade

Cadastro e autenticação de usuários (opcional)

Armazenamento das apostas no banco de dados

Histórico de apostas

Validação e exibição das apostas geradas

Integração entre frontend e backend via API REST

🧱 Tecnologias Utilizadas
Backend
Java 17+

Spring Boot

Spring Data JPA

MySQL

Spring Security (opcional)

Swagger/OpenAPI (para documentação da API)

Frontend
React 18+

Axios (para chamadas à API)

React Router DOM

Styled Components / TailwindCSS (opcional)

Vite ou Create React App

🛠️ Como Rodar o Projeto
🔙 Backend
Clone o repositório

bash
Copiar
Editar
git clone https://github.com/seu-usuario/sistema-apostas.git
cd sistema-apostas/backend
Configure o banco de dados MySQL

Crie um banco com o nome loterias_db

Atualize o arquivo application.properties com suas credenciais

Rode a aplicação

bash
Copiar
Editar
./mvnw spring-boot:run
A API estará disponível em: http://localhost:8080

⚛️ Frontend
Vá para a pasta frontend

bash
Copiar
Editar
cd ../frontend
Instale as dependências

bash
Copiar
Editar
npm install
Rode a aplicação

bash
Copiar
Editar
npm start
A interface estará em: http://localhost:3000

📂 Estrutura do Projeto
css
Copiar
Editar
sistema-apostas/
├── backend/
│   ├── src/main/java/com/seuusuario/loterias/
│   │   ├── controller/
│   │   ├── service/
│   │   ├── repository/
│   │   ├── model/
│   │   └── config/
│   └── resources/application.properties
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   └── App.jsx
📃 Endpoints da API
Documentados via Swagger em: http://localhost:8080/swagger-ui.html

Alguns exemplos:

GET /api/apostas – Lista todas as apostas

POST /api/apostas/mega-sena – Gera uma aposta da Mega-Sena

GET /api/apostas/{id} – Detalha uma aposta

🔐 Autenticação
(Opcional - se implementado)

JWT Token

Endpoints protegidos com Spring Security

Login via API: POST /api/auth/login

📅 Planejamento Futuro
Integração com resultados oficiais da Caixa

Geração baseada em estatísticas

Exportação de apostas para PDF/CSV

Versão mobile com React Native

🙋‍♂️ Contribuições
Sinta-se à vontade para abrir issues e PRs com melhorias, sugestões ou correções. Vamos juntos deixar o sistema ainda melhor! 💡
