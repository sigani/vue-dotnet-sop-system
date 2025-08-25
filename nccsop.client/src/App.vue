<<script setup lang="ts">
  import { computed, onMounted, ref } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useCategoryStore } from '@/stores/categoryStore'

  const route = useRoute()
  const router = useRouter()
  const categoryStore = useCategoryStore()
  const sidebar = ref(false)

  onMounted(async () => {
    await categoryStore.fetchCategories()
    await categoryStore.fetchSops()
  })

  // Function to build breadcrumb array
  function buildBreadcrumbs(id: number, isSop: boolean) {
    const breadcrumbs: { title: string; to: string; disabled?: boolean }[] = []
    let currentId: number | null = id

    while (currentId) {
      if (isSop) {
        const sop = categoryStore.sopsMap[currentId]
        if (!sop || !sop.name) {
          return null // invalid
        }

        breadcrumbs.unshift({ title: sop.name, to: `/`, disabled: true })
        currentId = sop.categoryId
        isSop = false
      } else {
        const cat = categoryStore.categoriesMap[currentId]
        if (!cat || !cat.name) {
          return null // invalid
        }

        breadcrumbs.unshift({ title: cat.name, to: `/category/${currentId}` })
        currentId = cat.parentCategoryId
      }
    }

    breadcrumbs.unshift({ title: 'Home', to: '/' })
    return breadcrumbs
  }

  const breadcrumbs = computed(() => {
    let idParam = route.params.id
    let isSop = false

    if (!idParam) {
      isSop = true
      idParam = route.params.sopId
    }

    if (!idParam) return [{ title: 'Home', to: '/' }]

    const id = Number(idParam)
    const items = buildBreadcrumbs(id, isSop)

    if (!items) {
      router.push('/') // redirect to Home
      return [{ title: 'Home', to: '/' }] // <-- always return array
    }

    if (items.length > 0) {
      items[items.length - 1].disabled = true
    }

    return items
  })
</script>

<template>
  <v-app>
    <!-- Header / Top Navigation -->
    <v-app-bar app color="primary" dark>
      <v-app-bar-nav-icon @click="sidebar = !sidebar"/>
      <v-btn to="/category">
        <v-toolbar-title >NCCSOP</v-toolbar-title>
      </v-btn>
      <v-breadcrumbs :items="breadcrumbs" />
    </v-app-bar>

    <!-- Side Navigation -->
    <v-navigation-drawer app v-if="sidebar">
      <v-list>
        <v-list-item title="SOPs" to="/category"></v-list-item>
        <v-list-item title="Settings" to="/settings"></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Main Content (changes with routing) -->
    <v-main>
      <router-view />
    </v-main>

    <!-- Footer -->
    <v-footer app color="primary" dark>
      <span class="mx-auto">© 2025 Created by Nickels Cabinets IT Team</span>
    </v-footer>
  </v-app>
</template>

<style scoped>
  header {
    line-height: 1.5;
  }

  .logo {
    display: block;
    margin: 0 auto 2rem;
  }

  @media (min-width: 1024px) {
    header {
      display: flex;
      place-items: center;
      padding-right: calc(var(--section-gap) / 2);
    }

    .logo {
      margin: 0 2rem 0 0;
    }

    header .wrapper {
      display: flex;
      place-items: flex-start;
      flex-wrap: wrap;
    }
  }
</style>
