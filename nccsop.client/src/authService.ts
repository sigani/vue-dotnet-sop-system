import { UserManager, WebStorageStateStore } from "oidc-client-ts";

const settings = {
  authority: "https://loginsmanagement.ncc.local", // your OpenIddict IdP URL
  client_id: "nccsop_offline",                // must match IdP registered client
  client_secret: "sopsopsop",
  redirect_uri: "https://localhost:62642/signin-oidc", // where IdP sends users back
  post_logout_redirect_uri: "https://localhost:62642/signout-callback-oidc",
  response_type: "code",               // use Authorization Code Flow with PKCE
  scope: "openid profile roles",         // must match scopes allowed
  userStore: new WebStorageStateStore({ store: window.sessionStorage }),
};

export const userManager = new UserManager(settings);
