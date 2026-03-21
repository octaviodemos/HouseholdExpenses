# =============================================================================
# Estágio 1: build — Node para instalar dependências e gerar os estáticos (dist/)
# Contexto Docker: pasta deste projeto (household-expenses-web/)
# =============================================================================
FROM node:20-alpine AS build
WORKDIR /app

# Copiar apenas package.json e package-lock.json primeiro: a camada npm ci só
# é invalidada quando as dependências mudam (melhor uso de cache do Docker).
COPY package.json package-lock.json ./
RUN npm ci

# Copiar o restante código e compilar (TypeScript + Vite → pasta dist/)
COPY . .
RUN npm run build

# =============================================================================
# Estágio 2: runtime — nginx Alpine para servir ficheiros estáticos (imagem leve)
# =============================================================================
FROM nginx:alpine AS final

# Artefactos de produção gerados pelo Vite
COPY --from=build /app/dist /usr/share/nginx/html

# Substituir o default do nginx: SPA com React Router (rotas no cliente)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Porta HTTP exposta pelo contentor
EXPOSE 80

# Manter o nginx em primeiro plano (recomendado para contentores)
CMD ["nginx", "-g", "daemon off;"]
