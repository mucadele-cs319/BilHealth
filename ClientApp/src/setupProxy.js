/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-var-requires */

// API calls can't reach the ASP.NET Core backend without this proxy.
// For details, see https://create-react-app.dev/docs/proxying-api-requests-in-development/#configuring-the-proxy-manually

/*
 *
 *  IGNORE ALL ERRORS IN THIS FILE. CRA SIMPLY DOES NOT SUPPORT ANY OTHER FORMAT.
 *
 */

const { createProxyMiddleware } = require("http-proxy-middleware");
const { env } = require("process");

const aspnetcore = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "http://localhost:55892";

module.exports = function (app) {
  app.use(
    "/api",
    createProxyMiddleware({
      target: aspnetcore,
      secure: false,
      headers: {
        Connection: "Keep-Alive",
      },
    }),
  );
};
