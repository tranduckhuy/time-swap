<h1 align="center">
  <br>
  <a href="https://swap-time.vercel.app/"><img src="/frontend/public/assets/imgs/logo-1.png" alt="TimeSwap" width="200"></a>
  <br>
</h1>

<h3 align="center">Backend Setup Guide (C# with Docker)</h3>

<p align="center">
  <a href="#prerequisites">Prerequisites</a> •
  <a href="#installation-steps">Installation Steps</a> •
  <a href="#additional-commands">Additional Commands</a>
</p>

# TimeSwap Backend Setup Guide

## Prerequisites

Ensure you have the following installed before proceeding:

- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- SSL certificates for Nginx (e.g., using [Let's Encrypt](https://letsencrypt.org/)) 
> **Note:**  
> Make sure to place your SSL certificates in the `/etc/letsencrypt` directory on your host machine. This directory should be mounted into the `reverse-proxy` container as specified in the `docker-compose.yml` file:
> 
> ```yaml
> volumes:
>   - /etc/letsencrypt:/etc/letsencrypt:ro
> ```
> This allows the Nginx container to access the certificates for HTTPS configuration.
## Installation Steps

### Clone the Repository

```bash
# Clone this repository
git clone https://github.com/tranduckhuy/time-swap.git

# Navigate to the backend directory
cd time-swap
```

### Environment Setup

#### Base Configuration

The backend uses a `base.yml` file to manage common environment variables and a `docker-compose.override.yml` file for environment variables specific to each container. Make sure these files are properly configured before proceeding.

#### Check  `base.yml` file and `docker-compose.override.yml` file for environment variables

You’ll find environment variables like the following examples:

```properties
ConnectionStrings__CoreDbConnection: Host=timeswapdb;Port=5432;Database=TimeSwapApi;Username=${CORE_DB_USER};Password=${CORE_DB_PASSWORD}
JWTSettings__SecretKey: ${JWT_SECRET}
JWTSettings__ValidAudience: ${JWT_AUDIENCE}
JWTSettings__ValidIssuer: ${JWT_ISSUER}
JWTSettings__ExpiryInSecond: ${JWT_EXPIRY}

...
- FrontendUrl=${FRONTEND_URL}
...
```

**Note:** Replace placeholder values with your actual configuration.

## Start Backend Services

Run the following command to start the backend services using Docker Compose:

```bash
# Build and start the services
$ docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

This will start:

- **Authentication Service** from `ghcr.io/tranduckhuy/timeswap-auth:latest`
- **API Service** from `ghcr.io/tranduckhuy/timeswap-api:latest`
- **Nginx** for reverse proxy management

**Note:** Ensure your `base.yml` and `docker-compose.override.yml` is correctly set up if you need custom overrides.

## Additional Commands

### Stop Services

```bash
docker-compose down
```

### Rebuild and Restart Services

```bash
docker-compose up --build -d
```

### Check Logs

```bash
docker-compose logs -f
```

### Verify Running Containers

```bash
docker ps
```

Your backend is now up and running! 🚀



