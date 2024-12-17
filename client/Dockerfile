FROM node:18-alpine

ARG VITE_SERVER_URL
ARG VITE_SESSION_URL
ENV VITE_SERVER_URL=http://server:8080
ENV VITE_SESSION_URL=${VITE_SESSION_URL}

WORKDIR /app

COPY package.json .

RUN npm install

RUN npm i -g serve

COPY . .

RUN npm run build

EXPOSE 3000

CMD [ "serve", "-s", "dist" ]
