# 🚀 Azure App Deployment Guide with GitHub Actions (Working Method)

This guide walks through the **step-by-step deployment** of a full-stack app (e.g., Blazor + .NET + MySQL + OpenAI API) to **Azure App Service** with CI/CD via **GitHub Actions**.

---

## ✅ Step 0: Prerequisites

- GitHub repository set up (e.g., `BackpackAzureApp`)
- Azure account (Azure for Students OK)
- Local project includes Dockerfile and `appsettings.Development.json`

---

## ✅ Step 1: Containerize the app with Docker

```bash
# Build your Docker image locally
docker build -t backpackapp .

# Tag the image for ACR
docker tag backpackapp backpackcrgstry.azurecr.io/backpackapp:latest
```

---

## ✅ Step 2: Push image to Azure Container Registry (ACR)

1. Login to ACR:

```bash
az acr login --name backpackcrgstry
```

2. Push the image:

```bash
docker push backpackcrgstry.azurecr.io/backpackapp:latest
```

---

## ✅ Step 3: Azure App Service Setup

1. **Create Web App** (Linux, Docker container)
2. Enable `SCM basic auth`:
   - App Service > Configuration > General settings > `SCM basic auth`: ✅ `On`
3. Restart App Service

---

## ✅ Step 4: Setup GitHub Actions via Deployment Center

1. Go to Azure Portal > App Service > **Deployment Center**
2. Set Source to **GitHub Actions**
3. Select:
   - Organization
   - Repository
   - Branch (e.g., `main`)
   - Option: ✅ `Add a new workflow file`
4. Save → this adds `.github/workflows/main_backpack-ai-assistant.yml` (you can delete and use your own `deploy.yml` instead)

---

## ✅ Step 5: Publish Profile Authentication

1. App Service > Overview > `Download publish profile`
2. Copy contents of `.PublishSettings`
3. GitHub → Repository → Settings → Secrets → Actions:
   - `AZURE_WEBAPP_PUBLISH_PROFILE` = Paste the full XML contents

---

## ✅ Step 6: Add Registry Secrets to GitHub

Add these under GitHub → Settings → Secrets → Actions:

| Secret Name | Value |
|-------------|--------|
| `REGISTRY_USERNAME` | ACR Admin username (from Access Keys) |
| `REGISTRY_PASSWORD` | ACR Admin password |

Make sure ACR > Access Keys > Admin user = ✅ `Enabled`

---

## ✅ Step 7: Add GitHub Actions Workflow (`deploy.yml`)

Place this in `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure Web App - backpack-ai-assistant

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Log in to ACR
        uses: docker/login-action@v2
        with:
          registry: backpackcrgstry.azurecr.io
          username: ${{ secrets.REGISTRY_USERNAME }}
          password: ${{ secrets.REGISTRY_PASSWORD }}

      - name: Build and push Docker image
        run: |
          docker build -t backpackcrgstry.azurecr.io/backpackapp:latest .
          docker push backpackcrgstry.azurecr.io/backpackapp:latest

      - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v2
        with:
          app-name: backpack-ai-assistant
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          images: backpackcrgstry.azurecr.io/backpackapp:latest
```

---

## ✅ Step 8: Commit & Deploy

```bash
git add .
git commit -m "Deploy app with GitHub Actions"
git push origin main
```

GitHub Actions will trigger:
1. ACR login ✅
2. Docker image build + push ✅
3. App Service deployment ✅

---

## ✅ Step 9: Access the App

Find the URL from App Service > Overview:
```
https://<your-app-name>.azurewebsites.net
```
Visit:
- `/ai-assistant`
- `/list`
- `/add`

---

## ✅ Notes

- Restart App Service if not immediately available
- Use Log Stream or Deployment Center > Logs for debugging
- Push anytime → triggers rebuild & redeploy (CI/CD)

---

This is the most reliable and minimal method based on the working deployment!
