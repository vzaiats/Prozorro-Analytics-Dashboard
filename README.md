# 📊Prozorro Analytics Dashboard

✅Service for analytics visualization based on Prozorro data

* **Backend API**: ASP.NET Core  
* **Database**: PostgreSQL  
* **Frontend**: React

## Prerequisites 🛠️

Before running the Prozorro Analytics Dashboard, make sure the following tools are installed on your machine:

* An IDE or code editor:
  * Visual Studio 2022+ [![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022%2B-blue?logo=visual-studio&logoColor=white)](https://visualstudio.microsoft.com/)
  * VS Code with the C# extension [![VS Code](https://img.shields.io/badge/VS%20Code-blue?logo=visual-studio-code&logoColor=white)](https://code.visualstudio.com/)
* .NET 9 SDK [![.NET](https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* PostgreSQL [![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17.6+-blue?logo=postgresql&logoColor=white)](https://www.postgresql.org/download/)
* Node.js [![Node.js](https://img.shields.io/badge/25.0%2B-x?style=flat&logo=Node.js&logoColor=green&label=Node.js&color=green)](https://nodejs.org/en/download)
* Docker [![Docker](https://img.shields.io/badge/Docker-blue?logo=docker&logoColor=white)](https://www.docker.com/get-started)

## Setup

1. **Clone the repository** 📂

```bash
git clone https://github.com/vzaiats/Prozorro-Analytics-Dashboard.git
cd Prozorro-Analytics-Dashboard
```

## Run with Docker 🐳
To run inside Docker containers:
```bash
docker-compose up --build
```
After build completes, open:

📡 The API can be explored in [![Swagger](https://img.shields.io/badge/Swagger-UI-green?logo=swagger&logoColor=white)](https://swagger.io/tools/swagger-ui/) at the following URL:

```
https://localhost:5001/swagger/index.html
```

💻 The frontend will be available at:

```bash
http://localhost:3000/
```

### Accessing the Database 🗄️

You can access PostgreSQL via pgAdmin (only when Docker containers are running):

1. 🌐 Open the following URL:
```bash
http://localhost:8080
```

2. Login with: ✉️ `admin@example.com` email and 🔑 `admin123`password credentials.

3. 🐘 PostgreSQL server ProzorroDB will be automatically available inside pgAdmin.

## Run locally (without Docker) 🖥️

1. **Configure your PostgreSQL connection** ⚙️

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=prozorro;Username=<your-username>;Password=<your-password>;"
}
```

> Replace 👤 `<your-username>` and 🔑 `<your-password>` with your PostgreSQL credentials.

Database will be created automatically on first run (no need to apply migrations manually).


### Running the API 📡

Before running the backend, make sure you select the startup project:
`ProzorroDataMining.Api`

1. Run the API using Visual Studio or with the CLI:

```bash
dotnet run --project ProzorroDataMining.Api
```

🌐 The API can be explored in [![Swagger](https://img.shields.io/badge/Swagger-UI-green?logo=swagger&logoColor=white)](https://swagger.io/tools/swagger-ui/) at the following URL:

```
https://localhost:5001/swagger/index.html
```

### Running the Frontend 💻

1. Navigate to the frontend folder:
```bash
cd ProzorroDataMining.Frontend
```

2. Install all dependencies:
```bash
npm install
```

3. Start the development server:

```bash
npm start
```

🌐 The frontend will be available at:

```bash
http://localhost:3000/
```
