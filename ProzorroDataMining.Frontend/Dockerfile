# Stage 1: Build React application
FROM node:25 AS build
WORKDIR /app

# Copy source code
COPY . .

# Install dependencies and build the app
RUN npm install
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine

# Copy build output to nginx html folder
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80 for nginx
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
