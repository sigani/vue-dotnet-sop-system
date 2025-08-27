import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Callback from '../views/Callback.vue'
import Category from '../views/Category.vue'
import Settings from '../views/Settings.vue'
import Search from '../views/Search.vue'
import NotFoundPage from '../views/NotFoundPage.vue'
import SOP from '../views/SOP.vue'

const routes = [
  {
    path: '/category',
    name: 'Home',
    component: Home,
    meta: { requiresAuth: true }
  },
  {
    path: '/',
    redirect: '/category',
    meta: { requiresAuth: true},
  },
  {
    path: '/search',
    component: Search,
    meta: { requiresAuth: true }
  },
  {
    path: '/settings',
    name: 'Settings',
    component: Settings,
    meta: { requiresAuth: true }
  },
  {
    path: '/signin-oidc',
    name: 'Callback',
    component: Callback,
  },
  {
    path: '/category/:id?',
    name: 'Category',
    component: Category,
    props: true,
    meta: { requiresAuth: true } 
  },
  {
    path: '/sop/:sopId',
    name: 'SOP',
    component: SOP,
    meta: { requiresAuth: true }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: NotFoundPage
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
