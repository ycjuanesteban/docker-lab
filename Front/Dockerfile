FROM node:current-slim

WORKDIR /react/app

COPY package.json .

RUN npm install

COPY . .

CMD ["npm", "start"]