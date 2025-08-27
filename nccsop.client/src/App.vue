<<script setup lang="ts">
  import { computed, onMounted, ref, watch } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useCategoryStore } from '@/stores/categoryStore'
  import type { Category, SOP } from '@/interfaces'

  import axios from 'axios'
  import type { AxiosResponse } from 'axios'
  const API_URL = import.meta.env.VITE_API_URL
  const route = useRoute()
  const router = useRouter()
  const categoryStore = useCategoryStore()
  const sidebar = ref(false)
  const breadcrumbs = ref<{ title: string; to: string; disabled?: boolean }[]>([]);

  onMounted(async () => {
    await categoryStore.fetchCategories()
    await categoryStore.fetchSops()
  })

  // Function to build breadcrumb array
  async function buildBreadcrumbs(id: number, isSop: boolean) {
    //const breadcrumbs: { title: string; to: string; disabled?: boolean }[] = []
    let currentId: number | null | undefined = id

    while (currentId) {
      if (isSop) {
        const response: AxiosResponse<SOP> = await axios.get<SOP>(`${API_URL}/sop/${currentId}`);
        const sop: SOP = response.data;
        if (!sop || !sop.name) return null;

        breadcrumbs.value.unshift({ title: sop.name, to: `/sop/${currentId}`, disabled: true });
        currentId = sop.categoryId;
        isSop = false;
      } else {
        const response: AxiosResponse<Category> = await axios.get<Category>(`${API_URL}/category/${currentId}`);
        const cat: Category = response.data;
        if (!cat || !cat.name) return null;
        breadcrumbs.value.unshift({ title: cat.name, to: `/category/${currentId}` });
        currentId = cat.parentCategoryId;
      }
    }

    breadcrumbs.value.unshift({ title: 'Home', to: '/' })
    return breadcrumbs.value
  }

  //const breadcrumbs = computed(async () => {
  //  let idParam = route.params.id
  //  let isSop = false

  //  if (!idParam) {
  //    isSop = true
  //    idParam = route.params.sopId
  //  }

  //  if (!idParam) return [{ title: 'Home', to: '/' }]

  //  const id = Number(idParam)
  //  const items = await buildBreadcrumbs(id, isSop)

  //  if (!items) {
  //    router.push('/') // redirect to Home
  //    return [{ title: 'Home', to: '/' }] // <-- always return array
  //  }

  //  if (items.length > 0) {
  //    items[items.length - 1].disabled = true
  //  }

  //  return items
  //})

  async function loadBreadcrumbs() {
    breadcrumbs.value = []
    let idParam = route.params.id;
    let isSop = false;

    if (!idParam) {
      isSop = true;
      idParam = route.params.sopId;
    }

    if (!idParam) {
      breadcrumbs.value = [{ title: 'Home', to: '/' }];
      return;
    }

    const id = Number(idParam);
    const items = await buildBreadcrumbs(id, isSop);

    if (!items) {
      //router.push('/');
      breadcrumbs.value = [{ title: 'Home', to: '/' }];
      return;
    }

    if (items.length > 0) {
      items[items.length - 1].disabled = true;
    }

    breadcrumbs.value = items;
  }

  watch(
    () => [route.params.id, route.params.sopId, route.name],
    () => {
      loadBreadcrumbs()
    }
  )

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
    <v-navigation-drawer v-if="sidebar">
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
