<template>
  <v-container fluid>
    <v-row class="ma-0" style="flex-wrap: wrap; gap: 16px;">
      <!-- Loop over categories from the store -->
      <template v-if="!categoryStore.activeCategoryId">
        <v-col v-for="category in categoryStore.rootCategories"
               :key="category.id"
               cols="auto"
               style="flex: 0 0 calc(20% - 16px);">
          <v-card class="mx-auto" width="270" :to="`/category/${category.id}`">
            <v-img class="align-end text-white"
                   height="250"
                   color="grey"
                   cover>
              <v-card-text class="text-h6">{{ category.name }}</v-card-text>
            </v-img>
          </v-card>
        </v-col>
      </template>

      <v-col v-for="category in categoryStore.childCategories"
             :key="category.id"
             cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" :to="`/category/${category.id}`">
          <v-img class="align-end text-white"
                 height="250"
                 color="grey"
                 cover>
            <v-card-text class="text-h6">{{ category.name }}</v-card-text>
          </v-img>
        </v-card>
      </v-col>

      <!-- Create New card -->
      <v-col cols="auto" style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" @click="dialog = true">
          <v-img class="align-end text-white" height="250" color="secondary" cover>
            <v-card-title>Create new</v-card-title>
          </v-img>
        </v-card>
      </v-col>

      <!-- Dialog -->
      <v-dialog v-model="dialog" max-width="500">
        <v-card>
          <v-card-title class="ma-2">
            New item
          </v-card-title>
          <v-radio-group v-model="selectedType" class="mx-4">
            <v-radio label="Category" value="cat"></v-radio>
            <v-radio label="SOP" value="sop"></v-radio>
          </v-radio-group>
          <v-card-text>
            <v-text-field label="Name" v-model="newName" />
            <v-file-input label="Upload an image (optional)" v-model="image" variant="solo-filled" hint="Maximum size is xyz * xyz.  This will be the cover image for the item." persistent-hint></v-file-input>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn text @click="dialog = false">Cancel</v-btn>
            <v-btn color="primary" @click="saveCategory">Create</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </v-container>

  <!-- Snackbar -->
  <v-snackbar v-model="snackbar" :timeout="3000" color="danger">
    {{ snackbarMessage }}
    <template #actions>
      <v-btn color="white" variant="text" @click="snackbar = false">
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
  import { ref, computed, onMounted, watch } from 'vue'
  import { useCategoryStore } from '@/stores/categoryStore'
  import { createCategory } from '@/services/categoryService'
  import { useRoute } from 'vue-router'

  const selectedType = ref('')
  const route = useRoute()
  const categoryStore = useCategoryStore()
  onMounted(() => {
    categoryStore.fetchCategories()
  })

  watch(
    () => route.params.id,
    (newId) => {
      categoryStore.activeCategoryId = newId ? Number(newId) : null
    },
    { immediate: true }
  )

  // Snackbar
  const snackbar = ref(false)
  const snackbarMessage = ref('')

  // Dialog & new category input
  const dialog = ref(false)
  const newName = ref('')

  async function saveCategory() {
    const trimmedName = newName.value.trim()

    //  Check if name is empty
    if (!trimmedName) {
      snackbarMessage.value = 'Category name cannot be empty.'
      snackbar.value = true
      return
    }

    //  Check if name is unique
    const isDuplicate = Object.values(categoryStore.categoriesMap).some(
      (c) => c.name.toLowerCase() === trimmedName.toLowerCase()
    )
    if (isDuplicate) {
      snackbarMessage.value = 'Category name must be unique.'
      snackbar.value = true
      return
    }

    // add to db
    try {
      const response = await createCategory({
        name: newName.value,
        parentCategoryId: route.params.id ? route.params.id : null,
      })

      const createdCategory = response.data
      categoryStore.categoriesMap[createdCategory.id] = createdCategory
      console.log('Created category:', response.data)
    } catch (error) {
      console.error('Failed to create category:', error)
    }

    // Reset
    newName.value = ''
    dialog.value = false
    snackbarMessage.value = 'Category created successfully!'
    snackbar.value = true
  }
</script>

<style>
</style>
