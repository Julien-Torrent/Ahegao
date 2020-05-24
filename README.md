# Welcome to the Ahegao repository

This repo contains the source code to :

* Create the database with the data
* Build the Ahegao website
* Build the Docker image

## Summary

This website allows the user to download [Hentai](https://en.wikipedia.org/wiki/Hentai) form various sites.
The hentai is downloaded in `.pdf` file format.

Currently supported sites :

* [Nhentai](https://nhentai.net)
* [Tsumino](https://tsumino.com)
* [Hentai2read](https://hentai2read.com)

This project is currently using [IronPdf](https://ironpdf.com/t3/) to generate the pdf from downloaded images, to use it you will need a valid licence.
Currently looking to remove IronPdf in favor of a free pdf generator.

## Setup the database

Currently the project supports the following database providers.

* SQLServer
* MySQL
* SQLite

Execute the script `Ahegao_database.sql` on your database provider.
Then change the `ConnectionString` and `SQLProvider` in `appsettings.json` according to your database configuration.

``` json
   "SQLProvider": "SQLServer",
   "ConnectionString": "Server=ServerName;Database=Ahegao;User ID=User;Password=Password;Integrated Security=False"
```

## Docker

You need to edit `docker-compose.yml` and change :

``` yml
    volumes:
      - //G/Hentai:/app/downloads/
```

by the following to choose the path where the images and pdfs will be stored.

``` yml
    volumes:
      - //Your/Full/Path:/app/downloads/
````
