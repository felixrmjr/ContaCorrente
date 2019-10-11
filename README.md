# WR.Modelo

Instruções de uso:
- Alterar a extenção retirando o "_" do arquivo de banco de dados da pasta DB;
- Alterar o caminho do arquivo de banco de dados na connectionString do arquivo "appsettings.json";
- Abrir o Postman e importar a Collection "WRModelo.postman_collection.json" que esta na pasta Postman;

Instruções de acesso:
- Acessar o método de autenticação (token) para obter o token de acesso;
- Informar o token de acesso para ter acesso aos outros métodos;

Lista de métodos:
- CriarContaCorrente;
- Transferencia;
- Debito;
- Credito;
- Métodos genéricos (Get, Post, Put, Delete, etc)
