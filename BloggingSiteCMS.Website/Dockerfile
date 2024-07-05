# Followed https://www.youtube.com/watch?v=XTqwvQAv9Lg

# Grab Node version 20, Alpine version
# Alpine is lightweight and secure, a very popular choice for node related Docker images
FROM node:20-alpine as build
# Set working directory to /app
WORKDIR /app
# Copy package.json and package-lock.json
COPY package*.json ./
# Install npm dependencies
RUN npm install
# Copy all files
COPY . .
# Expose port 5173 (Vite default)
EXPOSE 5173
# Run the application in development mode
CMD ["npm", "run", "dev"]
