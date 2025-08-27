<template>
  <v-container fluid>
    <v-row class="ma-0" style="flex-wrap: wrap; gap: 16px;">
      <!-- Loading Skeleton (unused for now) -->
      <template v-if="loading">
        <v-col v-for="n in 9"
               :key="n"
               cols="auto"
               style="flex: 0 0 calc(20% - 16px);">
          <v-card class="mx-auto" width="270">
            <!--Image skeleton-->
            <v-skeleton-loader type="image"
                               height="100%"
                               class="position-relative" />
          </v-card>
        </v-col>
      </template>

      <!-- This is for top level categories -->
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

              <div class="position-absolute" style="top: 8px; right: 8px;">
                <v-menu open-on-hover>
                  <template v-slot:activator="{ props }">
                    <v-btn v-bind="props"
                           icon
                           variant="text"
                           size="small">
                      <v-icon icon="mdi-dots-vertical"></v-icon>
                    </v-btn>
                  </template>

                  <v-list>
                    <v-list-item @click="dialog_delete = true; selectedType = 'cat'; selectedId = category.id">
                      <v-list-item-title style="color: red;">Delete</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </div>

              <v-card-text class="text-h6">
                {{ category.name }}
              </v-card-text>

            </v-img>
          </v-card>
        </v-col>
      </template>

      <!-- These are sub categories-->
      <v-col v-for="category in categoryStore.childCategories"
             :key="category.id"
             cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" :to="`/category/${category.id}`">
          <v-img class="align-end text-white"
                 height="250"
                 color="grey"
                 cover>
            <div class="position-absolute" style="top: 8px; right: 8px;">
              <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                  <v-btn v-bind="props"
                         icon
                         variant="text"
                         size="small">
                    <v-icon icon="mdi-dots-vertical"></v-icon>
                  </v-btn>
                </template>

                <v-list>
                  <v-list-item @click="dialog_delete = true; selectedType = 'cat'; selectedId = category.id">
                    <v-list-item-title style="color: red;">Delete</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>
            <v-card-text class="text-h6">{{ category.name }}</v-card-text>
          </v-img>
        </v-card>
      </v-col>

      <!-- These are the root SOPs-->
      <template v-if="!categoryStore.activeCategoryId">
        <v-col v-for="sop in categoryStore.rootSOPs"
               :key="sop.id"
               cols="auto"
               style="flex: 0 0 calc(20% - 16px);">
          <v-card class="mx-auto" width="270" :to="`/sop/${sop.id}`">
            <v-img class="align-end text-white"
                   height="250"
                   color="cyan"
                   cover>

              <div class="position-absolute" style="top: 8px; right: 8px;">
                <v-menu open-on-hover>
                  <template v-slot:activator="{ props }">
                    <v-btn v-bind="props"
                           icon
                           variant="text"
                           size="small">
                      <v-icon icon="mdi-dots-vertical"></v-icon>
                    </v-btn>
                  </template>

                  <v-list>
                    <v-list-item @click="dialog_delete = true; selectedType = 'sop'; selectedId = sop.id">
                      <v-list-item-title style="color: red;">Delete</v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </div>

              <v-card-text class="text-h6">
                {{ sop.name }}
              </v-card-text>

            </v-img>
          </v-card>
        </v-col>
      </template>

      <!-- These are the SOPs -->
      <v-col v-for="sop in categoryStore.childSops"
             :key="sop.id"
             cols="auto"
             style="flex: 0 0 calc(20% - 16px);">
        <v-card class="mx-auto" width="270" :to="`/sop/${sop.id}`">
          <v-img class="align-end text-white"
                 height="250"
                 color="cyan"
                 cover>
            <div class="position-absolute" style="top: 8px; right: 8px;">
              <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                  <v-btn v-bind="props"
                         icon
                         variant="text"
                         size="small">
                    <v-icon icon="mdi-dots-vertical"></v-icon>
                  </v-btn>
                </template>

                <v-list>
                  <v-list-item @click="dialog_delete = true; selectedType = 'sop'; selectedId = sop.id">
                    <v-list-item-title style="color: red;">Delete</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>

            <v-card-text class="text-h6">{{ sop.name }}</v-card-text>
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

      <!-- Delete Dialog-->
      <v-dialog v-model="dialog_delete" max-width="500">
        <v-card>
          <v-card-title class="ma-2">
            Delete
          </v-card-title>
          <v-card-text>
            Are you sure?  This is an irreversible action.
            <v-card-text class="text-red" v-if="selectedType=='cat'">
              All SOPs associated with this category will also be deleted.  You cannot delete categories that contain subcategories.
            </v-card-text>
          </v-card-text>
          
          <v-card-actions>
            <v-spacer />
            <v-btn text @click="dialog_delete = false">Cancel</v-btn>
            <v-btn color="red" @click="deleteItem()">Delete</v-btn>
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
  import { createCategory, deleteCategory } from '@/services/categoryService'
  import { createSOP, deleteSOP } from '@/services/sopService'
  import { useRoute } from 'vue-router'
  import type { Category, SOP } from '@/interfaces'

  const image = ref<File | null>(null)
  const selectedType = ref('')
  const selectedId = ref(0)
  const route = useRoute()
  const categoryStore = useCategoryStore()
  const loading = ref(true)

  onMounted(async () => {
    await Promise.all([
      categoryStore.fetchCategories(),
      categoryStore.fetchSops()
    ])
    loading.value = false
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
  const dialog_delete = ref(false)

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
    if (isDuplicate && selectedType.value == "cat") {
      snackbarMessage.value = 'Category name must be unique.'
      snackbar.value = true
      return
    }

    // add to db
    if (selectedType.value == "cat") {
      try {
        const response = await createCategory({
          name: newName.value,
          parentCategoryId: route.params.id ? Number(route.params.id) : null,
        } as Category)

        const createdCategory = response.data
        categoryStore.categoriesMap[createdCategory.id] = createdCategory

        snackbarMessage.value = 'Category created successfully!'
        console.log('Created category:', response.data)
      } catch (error) {
        console.error('Failed to create category:', error)
      }
    } else if (selectedType.value == "sop") {
      try {
        const response = await createSOP({
          name: newName.value,
          categoryId: route.params.id ? Number(route.params.id) : undefined,
        } as SOP)

        const createdSOP = response.data
        categoryStore.sopsMap[createdSOP.id] = createdSOP
        snackbarMessage.value = 'SOP created successfully!'
      } catch (error: unknown) {
        if (error instanceof Error) {
          snackbarMessage.value = error.message
        } else {
          snackbarMessage.value = String(error)
        }
      }
    }

    // Reset
    newName.value = ''
    dialog.value = false
    snackbar.value = true
  }

  async function deleteItem ()
  {
    if (selectedType.value == "cat") {
      try {
        const response = await deleteCategory(selectedId.value)
        snackbarMessage.value = 'Category successfully deleted!'
        delete categoryStore.categoriesMap[selectedId.value]
      }
      catch (error: unknown) {
        if (error instanceof Error) {
          snackbarMessage.value = error.message
        } else {
          snackbarMessage.value = String(error)
        }
      }
    }
    else if (selectedType.value == "sop") {
      try {
        const response = await deleteSOP(selectedId.value)
        snackbarMessage.value = 'SOP successfully deleted!'
        delete categoryStore.sopsMap[selectedId.value]
      }
      catch (error: unknown) {
        if (error instanceof Error) {
          snackbarMessage.value = error.message
        } else {
          snackbarMessage.value = String(error)
        }
      }
    }
    dialog_delete.value = false
    dialog.value = false
    snackbar.value = true
  }
</script>

<style>
</style>
