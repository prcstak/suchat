FROM node:latest

WORKDIR /app

# Copy package.json to /app directory
COPY package.json .

RUN npm install

COPY . .

EXPOSE 5173

CMD ["npm", "run", "dev", "--host"]