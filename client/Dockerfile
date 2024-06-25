FROM node:18-alpine

ARG VITE_SERVER_URL

RUN echo "Build Argument VITE_SERVER_URL: ${VITE_SERVER_URL}"

ENV VITE_SERVER_URL=${VITE_SERVER_URL}

RUN echo "VITE_SERVER_URL is set to ${VITE_SERVER_URL}"

WORKDIR /app

COPY package.json .

RUN npm install

RUN npm i -g serve

COPY . .

RUN npm run build

EXPOSE 3000

CMD [ "serve", "-s", "dist" ]
