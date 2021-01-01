# Alpha Img - Web API

This is a part of the Alpha Img. The project consists of two applications:

- [SPA built with Vue.js 3](https://github.com/Loreno10/alpha-img.spa-vue)
- Web API built with .NET 5 (**THIS REPO**)

You can find the details of the project in the [SPA
repository](https://github.com/Loreno10/alpha-img.spa-vue).

This Web API is reponsible for delivering the images that the SPA users request.
It gets the images using Google Search API. This app was introduced mostly for
the needs of securing the Google Search API Key.

The API is deployed to Heroku at https://alpha-imgs-webapi.herokuapp.com/. The
SPA frontend that uses this API is available at
https://alpha-imgs-spa.herokuapp.com/

## Features:

- has one GET endpoint that delivers max 20 images at a time. The parameters of
  the endpoint are:
  - `searchterm` - the query for the images
  - `index` - described below
- supports paging by the usage of the `index` query parameter (starts from `1`).
  Since the Google API returns maximum 10 images per API call, this API calls it
  two times to get 20 images per request, which is enough for one page of the
  SPA.
- returns only images in the PNG format (since there seems to be a limitation on
  the Clipboard API that SPA uses to copy the images - it only works with PNGs)
- returns images sorted by size (width)

## Project setup

```
dotnet run
```

### Configuration

The correct `AllowedOrigins` urls need to be provided in the `appsettings.json`.
