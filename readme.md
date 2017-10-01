# OAuth

**Security is not easy!**

## Token based security

The user provides their username and password to the authorization server, and the authorization serve will return a token to the user. Then the user will use the token to interact with our apis.

Third-Parties should recieve this token as well, do not give them your user's username and password. 

Treat tokens like passwords (with expiry in mind). 

The API will need to check to see if the token is still valid.

### How do you keep a token secure

The token is sent in the header when a request is sent to the API.

How do you make sure that the token has not been changed.

The token will be signed with a private key to ensure that they have not been tampered with. To ensure that the token has not been tampered with the API that accepts the token can use a public key.


The authorization will have both a public and a private key.

As long as we validate that the token has not been tampered with, with the public key we can entrust that it is safe to use this token.


### Token (JWT is pronouced JOT)

**What is a token?**

JSON Web Token - json encapsulated and signed in a way that can be understood by our apis.

    - The private key only lives on the authorization server
    - Public key is available for token validation
    - Should always be validated by consumers

Contains information about the Authoization Server

Referrend to as Access Token or Identity Tokens.

Base64 Encoded

## OAuth2

**OAuth 2.0 is the industry standard protocol for authorization. It  focuses on client developer simplicity while providing specific authorization flows for web applications, desktop applications, mobile phones, and living room devices.**

This works for all types of applications.

### OAuth

Open standard for Authorization
Token based
    - Access Token
    - JSON Web Token (JWT)
OAuth 2.0
    - Simpler
    - Not backwards compatitable

### Authorization and Authentication

Authorization - (Access) What access you have
OAuth

Authentication - (Identitiy) who you are
OpenID Connect

**Don't roll your own, use something like IdentityServer that you can trust understand the concepts behind the security.**

### RFC 6749

OAuth

/authorize

    - This is a new access token request 

/token
    - New access token request
    - Refresh access token
    - Trade authorization code for an access token

/revocation
    - Revoke an access token

OpenID

/userinfo
/checksession
/endsession
/.well-known/openid-configuration // information about the authentication server
/.well-known/jwks // information about the JWT signing keys used for token validation

### Access Token JWT

The JWT consists of three parts

The header, the payload, and the signature.
The signature ensure that the payload hasn't changed.

#### Claims (Payload)

Issuer
Audience
Expiry
Not valid before
Client ID
Scopes - you only have access to certain things.
Custom Data

You do not want to store things that can change.

Limit access to functionality based on scopes.

OpenID Connect Scopes Examples

    - openid
    - profile 
    - email
    - address
    - offline_access

Custom Scope Examples 
    - read
    - write

### Choosing a Flow 

Flows and Grants 

Redirect Flows

- Implicit Grant - The user comes into your website and you want them to login, first you redirect them to the authorization server, and then redirect them back to your website.

- Authorization Code - This can be used to trade for an access code. When the website creates an authorization code for the token. The website will send something that is secret to the website. This is good where you need to refresh a token because the man in the middle cannot create an access token since he doesn't know th private data that lives on the website.

Credential Flows
    - Resource owner password credentials
    - Client Credentials (Client Id and Secret)

When implemented correctly the website never knows about your username and password.


## **Generating Certs for testing**

Installing everything on windows is extremely cumbersome to generate certifications. Therefore for testing purposes use the following docker command to generate your certs

``` bash
docker build -t openssl .
docker run -e CERT_NAME=${name} -v ${volume_location}:/certs openssl
```