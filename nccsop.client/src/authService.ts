import { UserManager, WebStorageStateStore } from "oidc-client-ts";

const settings = {
  authority: "https://loginsmanagement.ncc.local", // your OpenIddict IdP URL
  client_id: import.meta.env.VITE_CLIENT_ID,                // must match IdP registered client
  client_secret: import.meta.env.VITE_CLIENT_SECRET,
  redirect_uri: import.meta.env.VITE_REDIRECT_URI, // where IdP sends users back
  post_logout_redirect_uri: import.meta.env.VITE_POST_LOGOUT_REDIRECT_URI,
  response_type: "code",               // use Authorization Code Flow with PKCE
  scope: "openid profile roles",         // must match scopes allowed
  userStore: new WebStorageStateStore({ store: window.sessionStorage }),
};

export const userManager = new UserManager(settings);
