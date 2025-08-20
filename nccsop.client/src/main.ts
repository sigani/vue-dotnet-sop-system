import './assets/main.css'
import '@mdi/font/css/materialdesignicons.css'
import { createApp } from 'vue'
import App from './App.vue'
import { createVuetify } from 'vuetify'
import 'vuetify/styles'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { createPinia } from 'pinia'
import { userManager } from './authService'
import router from './router'

async function initAuth() {
  // Check if user is already logged in
  const user = await userManager.getUser()
  if (!user) {
    await userManager.signinRedirect() // redirect to IdP login
  }
}

initAuth()

const vuetify = createVuetify({
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: { mdi },
  },
  components, directives
})

router.beforeEach(async (to, from, next) => {
  if (to.meta.requiresAuth) {
    const user = await userManager.getUser()
    if (!user) {
      await userManager.signinRedirect()
      return
    }
  }
  next()
})

createApp(App).use(vuetify).use(createPinia()).use(router).mount('#app')
