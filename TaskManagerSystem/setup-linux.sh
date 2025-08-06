echo "Setting up Task Management System for Linux..."

# Verificar Docker
if ! command -v docker &> /dev/null; then
    echo "Docker not installed. Installing..."
    sudo apt update
    sudo apt install -y docker.io docker-compose
    sudo usermod -aG docker $USER
    echo "Please logout and login again, then rerun this script."
    exit 1
fi

# Verificar .NET
if ! command -v dotnet &> /dev/null; then
    echo "Installing .NET SDK..."
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-8.0
fi

# Iniciar contenedores Docker
echo "Starting Docker containers..."
docker-compose up -d

# Esperar a que SQL Server esté listo
echo "Waiting for SQL Server to start (30 seconds)..."
sleep 30

# Crear la base de datos
echo "Creating database..."
cd TMS.Infrastructure
dotnet ef database update --startup-project ../TMS.Web.API

echo "Setup complete!"
echo ""
echo "Next steps:"
echo "   1. Run: dotnet run --project TMS.Web.API"
echo "   2. Open: http://localhost:5000/swagger"
echo "   3. DB Admin: http://localhost:8080"
echo "      Server: sqlserver"
echo "      Username: sa" 
echo "      Password: TaskManager2025!"