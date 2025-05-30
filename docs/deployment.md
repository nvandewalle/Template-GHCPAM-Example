# Deployment Guide

## Overview

This guide covers deploying the Event Management System to production environments.

## Prerequisites

- Azure subscription (for Azure Functions backend)
- Container registry (Azure Container Registry, Docker Hub, etc.)
- Web hosting service (Azure App Service, Netlify, Vercel, etc.)

## Backend Deployment (Azure Functions)

### Option 1: Direct Deployment

1. Install Azure Functions Core Tools:
   ```bash
   npm install -g azure-functions-core-tools@4 --unsafe-perm true
   ```

2. Login to Azure:
   ```bash
   az login
   ```

3. Create Function App:
   ```bash
   az functionapp create \
     --resource-group myResourceGroup \
     --consumption-plan-location westus \
     --runtime dotnet-isolated \
     --runtime-version 8 \
     --functions-version 4 \
     --name myEventManagementApp \
     --storage-account mystorageaccount
   ```

4. Deploy:
   ```bash
   cd backend/EventManagement
   func azure functionapp publish myEventManagementApp
   ```

### Option 2: Container Deployment

1. Build and push container:
   ```bash
   cd backend/EventManagement
   docker build -t myregistry/event-management-backend:latest .
   docker push myregistry/event-management-backend:latest
   ```

2. Deploy to Azure Container Apps or similar service.

## Frontend Deployment

### Option 1: Static Site Hosting (Netlify/Vercel)

1. Build the application:
   ```bash
   cd frontend/event-management
   npm run build
   ```

2. Deploy the `dist` folder to your hosting service.

3. Configure environment variables:
   - `VITE_API_BASE_URL`: Your backend API URL

### Option 2: Container Deployment

1. Build and push container:
   ```bash
   cd frontend/event-management
   docker build -t myregistry/event-management-frontend:latest .
   docker push myregistry/event-management-frontend:latest
   ```

2. Deploy container to your hosting service.

## Environment Configuration

### Backend Environment Variables

- `AzureWebJobsStorage`: Azure Storage connection string
- `FUNCTIONS_WORKER_RUNTIME`: Set to `dotnet-isolated`

### Frontend Environment Variables

- `VITE_API_BASE_URL`: Backend API base URL

## CI/CD Pipeline

The included GitHub Actions workflows automatically:

1. Build and test the applications
2. Create container images
3. Can be extended to deploy to production

### Extending for Automatic Deployment

Add deployment steps to the workflow files:

```yaml
- name: Deploy to Azure
  uses: azure/webapps-deploy@v2
  with:
    app-name: ${{ env.AZURE_WEBAPP_NAME }}
    publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
    images: 'myregistry/event-management-backend:latest'
```

## Monitoring and Logging

### Backend (Azure Functions)

- Application Insights for monitoring and logging
- Function app logs in Azure portal
- Custom telemetry and metrics

### Frontend

- Browser developer tools for client-side debugging
- Error reporting services (Sentry, etc.)
- Analytics tools (Google Analytics, etc.)

## Security Considerations

1. **HTTPS Only**: Ensure all production deployments use HTTPS
2. **Authentication**: Implement proper authentication for production
3. **CORS**: Configure CORS appropriately for your domains
4. **Secrets**: Use Azure Key Vault or similar for sensitive configuration
5. **Rate Limiting**: Implement rate limiting to prevent abuse

## Performance Optimization

### Backend

- Use Azure Functions Premium plan for better performance
- Implement caching strategies
- Optimize database queries

### Frontend

- Enable gzip compression
- Use CDN for static assets
- Implement lazy loading for components
- Optimize bundle size

## Backup and Recovery

1. **Data Backup**: Implement regular backups for your data storage
2. **Configuration Backup**: Store configuration in version control
3. **Disaster Recovery**: Plan for disaster recovery scenarios

## Scaling

### Backend

- Azure Functions automatically scales based on demand
- Consider Premium plan for predictable scaling
- Monitor performance and adjust accordingly

### Frontend

- Static sites scale automatically with CDN
- Container deployments can use auto-scaling groups
- Monitor traffic and adjust resources as needed