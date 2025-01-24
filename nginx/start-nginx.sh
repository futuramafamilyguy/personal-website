#!/bin/bash

CERT=/etc/nginx/ssl/live/$DOMAIN/fullchain.pem
KEY=/etc/nginx/ssl/live/$DOMAIN/privkey.pem
#hehe
if [[ -f /$CERT && -f $KEY ]]; then
	echo 'loading live config'
	envsubst '${SERVER_URL} ${CLIENT_URL} ${DOMAIN} ${SESSION_HUB_URL}' < /templates/default.conf.template > /etc/nginx/conf.d/default.conf
	if [ -f /etc/nginx/conf.d/certbot.conf ]; then
		rm /etc/nginx/conf.d/certbot.conf
	fi
else
	echo 'loading http-01 challenge config'
	envsubst '${DOMAIN}' < /templates/certbot.conf.template > /etc/nginx/conf.d/certbot.conf
	if [ -f /etc/nginx/conf.d/default.conf ]; then
		rm /etc/nginx/conf.d/default.conf
	fi
fi
