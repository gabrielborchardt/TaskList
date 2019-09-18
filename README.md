# Api.TaskList
	Tasks - Get, Post, Put, Delete
	Conexão com SQL Server - ConnectionString in AppSettings
	API com a rota /swagger (para verificar métodos)
	
# View.TaskList
	Projeto FrontEnd

### SQL Server
	CREATE TABLE TASK(
		ID int IDENTITY(1,1) PRIMARY KEY,
		TITULO varchar(1000) NOT NULL,
		STATUS varchar(1) NOT NULL,
		DESCRICAO varchar(1000),
		DATACRIACAO datetime NOT NULL,
		DATAEDICAO datetime,
		DATAREMOCAO datetime,
		DATACONCLUSAO datetime,
		CONSTRAINT CK_TASK_STATUS CHECK (STATUS IN ('A','R','C')) --A = Aberto / R = Removido / C = Concluido
	)