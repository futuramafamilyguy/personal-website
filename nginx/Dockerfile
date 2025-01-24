FROM nginx:latest

RUN apt-get update && apt-get install -y bash

COPY ./certbot.conf.template /templates/certbot.conf.template
COPY ./default.conf.template /templates/default.conf.template
COPY ./start-nginx.sh /scripts/start-nginx.sh

RUN chmod +x /scripts/start-nginx.sh

ENTRYPOINT ["/bin/bash", "-c", "/scripts/start-nginx.sh && nginx -g 'daemon off;'"]
