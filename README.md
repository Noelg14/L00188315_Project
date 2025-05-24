# L00188315_Project

[![Terraform Create](https://github.com/Noelg14/L00188315_Project/actions/workflows/provision_infrastructure.yml/badge.svg)](https://github.com/Noelg14/L00188315_Project/actions/workflows/provision_infrastructure.yml)  
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Noelg14_L00188315_Project&metric=alert_status&token=2e0c92735cc35b6016b6a10113c2c321f40c9f35)](https://sonarcloud.io/summary/new_code?id=Noelg14_L00188315_Project)  
[![Build Project Artefact](https://github.com/Noelg14/L00188315_Project/actions/workflows/build.yml/badge.svg)](https://github.com/Noelg14/L00188315_Project/actions/workflows/build.yml)  
# Contents: 
- [About](#about-this-project)
  - [Consent Flow](#consent-flow)
- [Setting Up the Environment](#setting-up-your-environment)
  - [Azure KeyVault](#azure-keyvault)
  - [Generating Revolut Certs](#generating-revolut-certs)
  - [DB & JWT Setup](#db--token-setup)
  - [Azure Settings](#app-service-settings)



# About this Project
This Project was done by L00188315.  
The application is an Open Banking Dashboard.

## Consent flow:
![Consent Flow](md_images/image.png)

# Setting up your environment
## Azure Keyvault
Set up a keyvault and an App registration.  
Give the app registration access to the Keyvault.

Add values to the `appsettings.json`
```json
  "kvSettings": {
    "ClientId": "<Your ClientID>",
    "ClientSecret": "<your Client Secret>",
    "Scope": "<Scope>",
    "TokenUrl": "https://login.microsoftonline.com/<yourTenant>/oauth2/v2.0/token",
    "KvBaseUrl": "https://<yourvault>.vault.azure.net",
    "KvApiVersion": "7.4"
  }
```
## Generating Revolut certs:
Follow the below to generate the `private.key` and `transport.pem`.  
Create a Developer account with Revolut OpenBanking Dev tools.  
Create an App and note the Client ID  

[Preparing the Sandbox Environment ](https://developer.revolut.com/docs/guides/build-banking-apps/get-started/prepare-sandbox-environment)

Combine these using openssl
`openssl pkcs12 -export -in transport.pem -inkey private.key -out combined.pfx`

Lastly, update the `appsettings.json` or the _Environment Variables_ in Azure:
```json
  "Revolut": {
    "baseUrl": "https://sandbox-oba.revolut.com",
    "tokenUrl": "https://sandbox-oba-auth.revolut.com/token",
    "consentUrl": "https://sandbox-oba.revolut.com/account-access-consents",
    "loginUrl": "https://sandbox-oba.revolut.com/ui/index.html?response_type=code%20id_token&scope=accounts",
    "certPath": "<path to PEM file>",
    "keyPath": "<path to private key>",
    "pfxPath": "<path to PFX>",
    "redirectUri": "<redirect url setup on revolut>"
  }
```
## DB & Token Setup
Lastly, set up the database config - the below example uses SQLite
```json
  "database": {
    "type": "sqlite"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data source=app.db",
    "IdentityConnection": "Data source=app.db"
  },
```
and Add the key for the JWT secret  
> [!CAUTION]  
> It is critical the key is not shared, as this is used to sign JWTs
```json
  "token": {
    "key": "<128 character string>",
    "issuer": "<issuer name>"
  },
```
## App Service Settings
If using an Azure App Service, use the below template and populate the values  
```json
[
  {
    "name": "database__type",
    "value": "sqlite",
    "slotSetting": false
  },
  {
    "name": "kvSettings__ClientId",
    "value": "<key vault clientID>",
    "slotSetting": false
  },
  {
    "name": "kvSettings__ClientSecret",
    "value": "<key vault secret>",
    "slotSetting": false
  },
  {
    "name": "kvSettings__KvApiVersion",
    "value": "7.4",
    "slotSetting": false
  },
  {
    "name": "kvSettings__KvBaseUrl",
    "value": "https://<kv name>.vault.azure.net",
    "slotSetting": false
  },
  {
    "name": "kvSettings__Scope",
    "value": "<KV Scope>",
    "slotSetting": false
  },
  {
    "name": "kvSettings__TokenUrl",
    "value": "https://login.microsoftonline.com/<your tenant>/oauth2/v2.0/token",
    "slotSetting": false
  },
  {
    "name": "Revolut__baseUrl",
    "value": "https://sandbox-oba.revolut.com",
    "slotSetting": false
  },
  {
    "name": "Revolut__certPath",
    "value": "<path to cert>",
    "slotSetting": false
  },
  {
    "name": "Revolut__consentUrl",
    "value": "https://sandbox-oba.revolut.com/account-access-consents",
    "slotSetting": false
  },
  {
    "name": "Revolut__keyPath",
    "value": "<path to key>",
    "slotSetting": false
  },
  {
    "name": "Revolut__loginUrl",
    "value": "https://sandbox-oba.revolut.com/ui/index.html?response_type=code%20id_token&scope=accounts",
    "slotSetting": false
  },
  {
    "name": "Revolut__pfxPath",
    "value": "<path to PFX>",
    "slotSetting": false
  },
  {
    "name": "Revolut__redirectUri",
    "value": "<redirect URL setup in Revolut>",
    "slotSetting": false
  },
  {
    "name": "Revolut__tokenUrl",
    "value": "https://sandbox-oba-auth.revolut.com/token",
    "slotSetting": false
  },
  {
    "name": "token__issuer",
    "value": "<token issuer>",
    "slotSetting": false
  },
  {
    "name": "token__key",
    "value": "<token>",
    "slotSetting": false
  }
]

```