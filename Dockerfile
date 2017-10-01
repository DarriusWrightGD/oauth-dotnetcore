FROM alpine
ARG CERT_NAME
ARG PASSWORD=password
WORKDIR /certs
RUN apk update && apk add openssl
CMD openssl req -newkey rsa:2048 -subj "/C=US" -nodes -keyout $CERT_NAME.key -x509 -days 365 -out $CERT_NAME.cer && \
	openssl pkcs12 -export -in $CERT_NAME.cer -inkey $CERT_NAME.key -password pass:$PASSWORD -out $CERT_NAME.pfx